using System.Collections.Generic;

namespace ToDoList.Models
{
    public class TasksList
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual IList<Task> Tasks { get; set; }

        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }

    }
}
