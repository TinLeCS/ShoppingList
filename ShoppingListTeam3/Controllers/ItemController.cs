using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingListTeam3.Data;
using ShoppingListTeam3.Models;
using ShoppingListTeam3.Services;
using PagedList;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ShoppingListTeam3.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private const int MB = 1024 * 1024;
        private readonly Lazy<ItemService> _svc = new Lazy<ItemService>();
        private readonly Lazy<NoteService> _noteSvc = new Lazy<NoteService>();

        // GET: Item
        public ActionResult Index(int? id, int? shoppingListID, int? page)
        {
            {
                var viewModel = new ItemWithNoteViewModel();

                ViewBag.shoppingListID = id;
                int pageSize = 5;
                int pageNumber = page ?? 1;

                viewModel.Items = _svc.Value.GetItemsByShoppingListID(id.Value).ToPagedList(pageNumber, pageSize);
                if (shoppingListID != null)
                {
                    ViewBag.itemID = id.Value;
                    ViewBag.shoppingListID = shoppingListID.Value;
                    viewModel.Items = _svc.Value.GetItemsByShoppingListID(shoppingListID.Value).ToPagedList(pageNumber, pageSize);
                    viewModel.Note = _noteSvc.Value.GetNoteByItemID(id.Value);
                }
                    
                return View(viewModel);
            }
        }

        // GET: Item/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemViewModel itemViewModel = _svc.Value.GetItemByID(id);
            if (itemViewModel == null)
            {
                return HttpNotFound();
            }
            return View(itemViewModel);
        }

        // GET: Item/Create
        public ActionResult Create(int shoppingListID)
        {
            ViewBag.shoppingListID = shoppingListID;
            return View(new ItemViewModel());
        }

        // POST: Item/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Content,Priority,IsChecked")] ItemViewModel itemViewModel, int shoppingListID)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Files["Image"];
                var allowedExtensions = new[] { ".jpg", ".png", ".jpeg", ".bmp", ".gif" };
                var extension = Path.GetExtension(file.FileName);

                if (file.ContentLength > 0 && !allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("", "You are only allowed to upload a JPG, JPEG, PNG, BMP, GIF file.");
                    return View(itemViewModel);
                }

                if (file.ContentLength > 4 * MB)
                {
                    ModelState.AddModelError("", "The image size cannot exceed 4 MB.");
                    return View(itemViewModel);
                }

                var result = _svc.Value.CreateItem(itemViewModel, shoppingListID);
                if (result[0] != 1)
                {
                    ModelState.AddModelError("", " Unable to add item.");
                    return View(itemViewModel);
                }

                // Get the uploaded image from the Files collection
                if (file != null && file.ContentLength > 0)
                {
                    // create an Item folder inside ~/Content folder
                    Directory.CreateDirectory(Server.MapPath("~/Content/Item"));

                    // store the image inside ~/Content/Item folder
                    // a little trick to prevent file stored as the same name with a different type
                    var path = GetDefaultPath(result[1]);

                    var image = Image.FromStream(file.InputStream, true, true);

                    image = ResizeImage(image, 200, 200);
                    image.Save(path);
                }
                return RedirectToAction("Index", new { id = shoppingListID });
            }

            return View(itemViewModel);
        }

        // GET: Item/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemViewModel itemViewModel = _svc.Value.GetItemByID(id);
            ViewBag.shoppingListID = itemViewModel.ShoppingListID;
            if (itemViewModel == null)
            {
                return HttpNotFound();
            }
            return View(itemViewModel);
        }

        // POST: Item/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Content,Priority,IsChecked")] ItemViewModel itemViewModel, int shoppingListID)
        {

            if (ModelState.IsValid)
            {
                var file = Request.Files["Image"];
                var allowedExtensions = new[] { ".jpg", ".png", ".jpeg", ".bmp", ".gif" };
                var extension = Path.GetExtension(file.FileName);

                if (file.ContentLength > 0 && !allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("", "You are only allowed to upload a JPG, JPEG, PNG, BMP, GIF file.");
                    return View(itemViewModel);
                }

                if (file.ContentLength > 4 * MB)
                {
                    ModelState.AddModelError("", "The image size cannot exceed 4 MB.");
                    return View(itemViewModel);
                }

                var result = _svc.Value.UpdateItem(itemViewModel);
                if (result[0] != 1)
                {
                    ModelState.AddModelError("", " Unable to add item.");
                    return View(itemViewModel);
                }

                // Get the uploaded image from the Files collection
                if (file != null && file.ContentLength > 0)
                {
                    // create an Item folder inside ~/Content folder
                    Directory.CreateDirectory(Server.MapPath("~/Content/Item"));

                    // store the image inside ~/Content/Item folder
                    // a little trick to prevent file stored as the same name with a different type
                    var path = GetDefaultPath(result[1]);

                    var image = Image.FromStream(file.InputStream, true, true);

                    image = ResizeImage(image, 200, 200);
                    image.Save(path);
                }
                return RedirectToAction("Index", new { id = shoppingListID });
            }

            return View(itemViewModel);
        }

        // GET: Item/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemViewModel itemViewModel = _svc.Value.GetItemByID(id);
            ViewBag.shoppingListID = itemViewModel.ShoppingListID;
            if (itemViewModel == null)
            {
                return HttpNotFound();
            }
            return View(itemViewModel);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int shoppingListID)
        {
            _svc.Value.DeleteItem(id);
            DeleteFile(id);

            return RedirectToAction("Index", new { id = shoppingListID });
        }

        public ActionResult Clear(int? shoppingListID)
        {
            if (shoppingListID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _svc.Value.DeleteAllItems(shoppingListID, Server.MapPath("~/Content/Item"));
            return RedirectToAction("Index", new { id = shoppingListID});
        }

        public string GetDefaultPath(int id)
        {
            return Path.Combine(Server.MapPath("~/Content/Item"), id + ".jpg");
        }

        public void DeleteFile(int id)
        {
            string path = GetDefaultPath(id);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
            }
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }

}


