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

namespace ShoppingListTeam3.Controllers
{
    public class ItemController : Controller
    {
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
                _svc.Value.CreateItem(itemViewModel, shoppingListID);
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
                _svc.Value.UpdateItem(itemViewModel);
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
            return RedirectToAction("Index", new { id = shoppingListID });
        }

        public ActionResult Clear(int? shoppingListID)
        {
            if (shoppingListID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _svc.Value.DeleteAllItems(shoppingListID);
            return RedirectToAction("Index", new { id = shoppingListID});
        }
    }

}


