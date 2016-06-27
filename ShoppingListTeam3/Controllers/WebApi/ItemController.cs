using ShoppingListTeam3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShoppingListTeam3.Controllers.WebApi
{
    [RoutePrefix("api/Item")]
    public class ItemController : ApiController
    {
        private bool SetCheckState(int id, bool state)
        {
            using (var ctx = new ShoppingListDbContext())

                return ctx.Database.ExecuteSqlCommand($"UPDATE Item SET IsChecked = '{state}' WHERE ID = {id}") == 1;
        }

        [Route("{id}")]
        [HttpPost]
        public bool ToggleCheck(int id)
        {
            return SetCheckState(id, true);
        }

        [Route("{id}")]
        [HttpDelete]
        public bool ToggleUnchecked(int id)
        {
            return SetCheckState(id, false);
        }
    }
}
