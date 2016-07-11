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
using Microsoft.AspNet.Identity;
using PagedList;

namespace ShoppingListTeam3.Controllers
{
    [Authorize]
    public class ShoppingListController : Controller
    {
        private readonly Lazy<ShoppingListService> _svc;
        private readonly Lazy<ItemService> _itemSvc = new Lazy<ItemService>();

        public ShoppingListController()
        {
            _svc =
                new Lazy<ShoppingListService>(
                    () =>
                    {
                        var UserID = Guid.Parse(User.Identity.GetUserId());
                        return new ShoppingListService(UserID);
                    });
        }

        // GET: ShoppingList
        public ActionResult Index(int? page)
        {
            var ShoppingList = _svc.Value.GetList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(ShoppingList.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult GroupView()
        {
            var ShoppingListByGroup = _svc.Value.GetList();
            return View(ShoppingListByGroup);
        }

        // GET: ShoppingList/Details/5
        //public ActionResult Details(int? id, string p1)
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingListViewModel shoppingListViewModel = _svc.Value.GetListByID(id);
            //ViewBag.p1 = p1;
            if (shoppingListViewModel == null)
            {
                return HttpNotFound();
            }
            return View(shoppingListViewModel);
        }

        // GET: ShoppingList/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShoppingList/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Color,Group")] ShoppingListViewModel shoppingListViewModel)
        {
            if (ModelState.IsValid)
            {
                _svc.Value.CreateList(shoppingListViewModel);
                return RedirectToAction("Index");
            }

            return View(shoppingListViewModel);
        }

        // GET: ShoppingList/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingListViewModel shoppingListViewModel = _svc.Value.GetListByID(id);
            if (shoppingListViewModel == null)
            {
                return HttpNotFound();
            }
            return View(shoppingListViewModel);
        }

        // POST: ShoppingList/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Color,Group")] ShoppingListViewModel shoppingListViewModel)
        {
            if (ModelState.IsValid)
            {
                _svc.Value.UpdateList(shoppingListViewModel);
                return RedirectToAction("Index");
            }
            return View(shoppingListViewModel);
        }

        // GET: ShoppingList/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingListViewModel shoppingListViewModel = _svc.Value.GetListByID(id);
            if (shoppingListViewModel == null)
            {
                return HttpNotFound();
            }
            return View(shoppingListViewModel);
        }

        // POST: ShoppingList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _itemSvc.Value.DeleteAllItems(id, Server.MapPath("~/Content/Item"));
            _svc.Value.DeleteList(id);
            return RedirectToAction("Index");
        }
    }
}
