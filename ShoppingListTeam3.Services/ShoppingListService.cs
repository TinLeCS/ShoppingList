using ShoppingListTeam3.Data;
using ShoppingListTeam3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListTeam3.Services
{
    public class ShoppingListService
    {
        private readonly Guid _userID;

        public ShoppingListService(Guid UserID)
        {
            _userID = UserID;
        }


        public IEnumerable<ShoppingListViewModel> GetList()
        {
            using (var ctx = new ShoppingListDbContext())
            {
                return ctx.ShoppingLists.Where(e => e.UserID == _userID).Select(
                    e => new ShoppingListViewModel { ID = e.ID, Name = e.Name, Color = e.Color, Group = e.Group, CreatedUtc = e.CreatedUtc, ModifiedUtc = e.ModifieddUtc }
                ).ToArray();
            }
        }

        public ShoppingListViewModel GetListByID(int? id)
        {
            ShoppingList entity;

            using (var ctx = new ShoppingListDbContext())
            {
                entity = ctx.ShoppingLists.SingleOrDefault(e => e.ID == id);
            }
            if (entity != null)
                return new ShoppingListViewModel
                {
                    ID = entity.ID,
                    Name = entity.Name,
                    Group = entity.Group,
                    Color = entity.Color,
                    CreatedUtc = entity.CreatedUtc,
                    ModifiedUtc = entity.ModifieddUtc
                };
            else
                return null;
        }

        public bool CreateList(ShoppingListViewModel vm)
        {
            using (var ctx = new ShoppingListDbContext())
            {
                var entity =
                    new ShoppingList
                    {
                        UserID = _userID,
                        Name = vm.Name,
                        Color = vm.Color,
                        Group = (vm.Group == null || vm.Group == "") ? "General" : vm.Group,
                        CreatedUtc = DateTimeOffset.UtcNow,
                    };

                ctx.ShoppingLists.Add(entity);

                return ctx.SaveChanges() == 1;
            }
        }

        public bool UpdateList(ShoppingListViewModel vm)
        {
            using (var ctx = new ShoppingListDbContext())
            {
                var entity = ctx.ShoppingLists.SingleOrDefault(e => e.ID == vm.ID);

                entity.Name = vm.Name;
                entity.Color = vm.Color;
                entity.Group = vm.Group;
                entity.ModifieddUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteList(int? id)
        {
            using (var ctx = new ShoppingListDbContext())
            {
                //ctx.Database.ExecuteSqlCommand($"DELETE FROM Item WHERE ShoppingListID = {id}");

                var entity = ctx.ShoppingLists.SingleOrDefault(e => e.ID == id);

                // TODO: Handle note not found
                ctx.ShoppingLists.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
