using System.Collections.Generic;

namespace ToDoList.Models
{
    public class ItemList
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual IList<ListItem> Items { get; set; }

        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }

    }
}
