using System;
using System.Net;
using System.Web.Mvc;
using ShoppingListTeam3.Models;
using ShoppingListTeam3.Services;

namespace ShoppingListTeam3.Controllers
{
    public class NoteController : Controller
    {
        private readonly Lazy<NoteService> _svc = new Lazy<NoteService>();

        // GET: Item/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoteViewModel itemViewModel = _svc.Value.GetNoteByItemID(id);
            if (itemViewModel == null)
            {
                return HttpNotFound();
            }
            return View(itemViewModel);
        }

        // GET: Item/Create
        public ActionResult Create(int id, int shoppingListID)
        {
            ViewBag.shoppingListID = shoppingListID;
            ViewBag.itemID = id;
            return View();
        }

        // POST: Item/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemId,Body")] NoteViewModel noteViewModel, int id, int shoppingListID)
        {
            if (ModelState.IsValid)
            {
                _svc.Value.CreateNote(noteViewModel, id);
                return RedirectToAction("../Item/Index", new { id = id, shoppingListID = shoppingListID});
            }

            return View(noteViewModel);
        }

        // GET: Item/Edit/5
        public ActionResult Edit(int? id, int shoppingListID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoteViewModel itemViewModel = _svc.Value.GetNoteByItemID(id);
            ViewBag.itemID = id;
            ViewBag.shoppingListID = shoppingListID;
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
        public ActionResult Edit([Bind(Include = "ItemId,Body")] NoteViewModel noteViewModel, int id, int shoppingListID)
        {
            if (ModelState.IsValid)
            {
                _svc.Value.UpdateNote(noteViewModel);
                return RedirectToAction("../Item/Index", new { id = id, shoppingListID = shoppingListID});
            }
            return View(noteViewModel);
        }

        // GET: Item/Delete/5
        public ActionResult Delete(int? id, int shoppingListID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _svc.Value.DeleteItem(id);
            return RedirectToAction("../Item/Index", new { id = id, shoppingListID = shoppingListID });
        }
    }
}


