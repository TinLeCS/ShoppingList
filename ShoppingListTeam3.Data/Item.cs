using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListTeam3.Data
{
    public class Item
    {
        [Key]
        public int ID { get; set; }
        public int ShoppingListID { get; set; }
        public string Content { get; set; }
        public int Priority { get; set; }
        public bool IsChecked { get; set; }
        public DateTimeOffset CreatedUtc { get; set; }
        public DateTimeOffset? ModifiedUtc { get; set; }

        public virtual ShoppingList ShoppingList { get; set; }
        //public virtual Note Note { get; set; }
    }
}
