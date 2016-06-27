using ShoppingListTeam3.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListTeam3.Models
{
    public class NoteViewModel
    {
        [Key]
        public int ItemId { get; set; }
        public string Body { get; set; }
        public DateTimeOffset CreatedUtc { get; set; }
        public DateTimeOffset? ModifiedUtc { get; set; }

        public string DisplayDateTimeOffset(DateTimeOffset? input)
        {
            return (input != null) ? input.Value.LocalDateTime.ToString("G") : "Unknown";
        }
    }
}
