using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListTeam3.Data
{
    public class ShoppingList
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public Guid UserID { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public string Group { get; set; }

        public DateTimeOffset CreatedUtc { get; set; }

        public DateTimeOffset? ModifieddUtc { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
