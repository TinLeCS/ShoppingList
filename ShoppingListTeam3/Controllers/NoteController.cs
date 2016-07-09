using System;
using System.Net;
using System.Web.Mvc;
using ShoppingListTeam3.Models;
using ShoppingListTeam3.Services;
using ShoppingListTeam3.Data;
using System.Web.Services;

namespace ShoppingListTeam3.Controllers
{
    public class NoteController : Controller
    {
        private readonly Lazy<NoteService> _svc = new Lazy<NoteService>();

        [WebMethod]
        public bool Create(int itemId, string noteBody)
        {
            using (var ctx = new ShoppingListDbContext())
            {
                var result = ctx.Database.ExecuteSqlCommand($"INSERT INTO Note (ItemId, Body, CreatedUtc) VALUES({itemId}, '{noteBody}', '{DateTimeOffset.Now}')");

                return result == 1;
            }
        }

        [WebMethod]
        public bool Edit(int itemId, string noteBody)
        {
            using (var ctx = new ShoppingListDbContext())
            {
                var result = ctx.Database.ExecuteSqlCommand($"UPDATE Note SET Body = '{noteBody}', ModifiedUtc = '{DateTimeOffset.Now}' WHERE ItemId = {itemId}");

                return result == 1;
            }
        }

        // GET: Item/Delete/5
        public ActionResult Delete(int? id, int shoppingListID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _svc.Value.DeleteNote(id);
            return RedirectToAction("../Item/Index", new { id = id, shoppingListID = shoppingListID });
        }
    }
}


