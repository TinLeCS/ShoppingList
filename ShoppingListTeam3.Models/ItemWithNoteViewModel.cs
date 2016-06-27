using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace ShoppingListTeam3.Models
{
    public class ItemWithNoteViewModel
    {
        public IPagedList<ItemViewModel> Items { get; set; }

        public NoteViewModel Note { get; set; }
    }
}
