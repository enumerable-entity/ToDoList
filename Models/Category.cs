using System.Collections.Generic;

namespace ToDoList.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public virtual IList<TasksList> TaskLists { get; set; }

        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}
