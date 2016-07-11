using ShoppingListTeam3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingListTeam3.Models;
using System.IO;

namespace ShoppingListTeam3.Services
{
    public class ItemService
    {
        public IEnumerable<ItemViewModel> GetItemsByShoppingListID(int? id)
        {
            using (var ctx = new ShoppingListDbContext())
            {
                string query = "SELECT * FROM Item WHERE ShoppingListID = @p0";
                return ctx.Items.SqlQuery(query, id).Select(
                    e => new ItemViewModel { ID = e.ID, ShoppingListID = id.Value, Priority = e.Priority, Content = e.Content, IsChecked = e.IsChecked, CreatedUtc = e.CreatedUtc, ModifiedUtc = e.ModifiedUtc }
                ).ToArray();
            }
        }

        public ItemViewModel GetItemByID(int? id)
        {
            Item entity;

            using (var ctx = new ShoppingListDbContext())
            {
                entity = ctx.Items.SingleOrDefault(e => e.ID == id);
            }
            if (entity != null)
                return new ItemViewModel
                {
                    ID = entity.ID,
                    ShoppingListID = entity.ShoppingListID,
                    Content = entity.Content,
                    IsChecked = entity.IsChecked,
                    Priority = entity.Priority,
                    CreatedUtc = entity.CreatedUtc,
                    ModifiedUtc = entity.ModifiedUtc
                };
            else
                return null;
        }

        public int[] CreateItem(ItemViewModel vm, int shoppingListId)
        {
            using (var ctx = new ShoppingListDbContext())
            {
                var entity =
                    new Item
                    {
                        ShoppingListID = shoppingListId,
                        Content = vm.Content,
                        Priority = vm.Priority,
                        IsChecked = vm.IsChecked.Value,
                        CreatedUtc = DateTimeOffset.UtcNow,
                    };

                ctx.Items.Add(entity);

                int[] result = new int[] { ctx.SaveChanges(), entity.ID };

                return result;
            }
        }

        public int[] UpdateItem(ItemViewModel vm)
        {
            using (var ctx = new ShoppingListDbContext())
            {
                var entity = ctx.Items.SingleOrDefault(e => e.ID == vm.ID);

                entity.Content = vm.Content;
                entity.Priority = vm.Priority;
                entity.IsChecked = vm.IsChecked.Value;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                int[] result = new int[] { ctx.SaveChanges(), entity.ID };

                return result;
            }
        }

        public bool DeleteItem(int? id)
        {
            using (var ctx = new ShoppingListDbContext())
            {
                //ctx.Database.ExecuteSqlCommand($"DELETE FROM Review WHERE ProductID = {id}");

                var entity = ctx.Items.SingleOrDefault(e => e.ID == id);

                // TODO: Handle note not found
                ctx.Items.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }

        public void DeleteAllItems(int? shoppingListID, string path)
        {
            using (var ctx = new ShoppingListDbContext())
            {
                //ctx.Database.ExecuteSqlCommand($"DELETE FROM Item WHERE ShoppingListID = {shoppingListID}");
                var items = ctx.Items.Where(item => item.ShoppingListID == shoppingListID);
                foreach(var item in items)
                {
                    ctx.Items.Remove(item);
                    DeleteFile(path + "\\" + item.ID + ".jpg");
                }
                ctx.SaveChanges();
            }          
        }

        public void DeleteFile(string path)
        {
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
            }
        }
    }
}
