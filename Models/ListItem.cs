using System;

namespace ToDoList.Models
{
    public class ListItem
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime? CompleteDate { get; set; }

        public bool IsCompleted { get; set; }

        public virtual ItemList ItemList { get; set; }
        public int ItemListId { get; set; }

    }
}
