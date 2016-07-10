using ShoppingListTeam3.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListTeam3.Models
{
    public class ItemViewModel
    {
        [Key]
        public int ID { get; set; }
        public int ShoppingListID { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int Priority { get; set; }
        [Display(Name = "Checked")]
        [UIHint("Checked")]
        public bool? IsChecked { get; set; }
        [Display(Name = "Created Date")]
        public DateTimeOffset CreatedUtc { get; set; }
        [Display(Name = "Modified Date")]
        public DateTimeOffset? ModifiedUtc { get; set; }
        public string Color
        {
            get
            {
                using (var ctx = new ShoppingListDbContext())
                {
                    return ctx.ShoppingLists.Where(e => e.ID == ShoppingListID).SingleOrDefault().Color;
                }
            }
        }

        public string ShoppingListName
        {
            get
            {
                using (var ctx = new ShoppingListDbContext())
                {
                    return ctx.ShoppingLists.Where(e => e.ID == ShoppingListID).SingleOrDefault().Name;
                }
            }
        }

        public string GetShoppingListName(int id)
        {

            using (var ctx = new ShoppingListDbContext())
            {
                return ctx.ShoppingLists.Where(e => e.ID == id).SingleOrDefault().Name;
            }
        }

        public string ConvertToPriorityMessage(int priority)
        {
            switch (priority)
            {
                case 1:
                    return "Grab it now!";
                case 2:
                    return "Need it soon";
                case 3:
                    return "It can wait";
                default:
                    return "Nothing should be shown here";
            }
        }


    }
}
