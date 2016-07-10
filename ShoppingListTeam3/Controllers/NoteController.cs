using System;
using System.Net;
using System.Web.Mvc;
using ShoppingListTeam3.Models;
using ShoppingListTeam3.Services;
using ShoppingListTeam3.Data;
using System.Web.Services;
using System.Data.SqlClient;

namespace ShoppingListTeam3.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        private readonly Lazy<NoteService> _svc = new Lazy<NoteService>();

        [WebMethod]
        public bool Create(int itemId, string noteBody)
        {
            using (var ctx = new ShoppingListDbContext())
            {
                var command = @"INSERT INTO Note (ItemId, Body, CreatedUtc) VALUES({0}, {1}, {2})";
                var result = ctx.Database.ExecuteSqlCommand(command, itemId, noteBody, DateTimeOffset.UtcNow);

                return result == 1;
            }
        }

        [WebMethod]
        public bool Edit(int itemId, string noteBody)
        {
            using (var ctx = new ShoppingListDbContext())
            {
                var command = @"UPDATE Note SET Body = {0}, ModifiedUtc = {1} WHERE ItemId = {2}";
                var result = ctx.Database.ExecuteSqlCommand(command, noteBody, DateTimeOffset.UtcNow, itemId);

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


