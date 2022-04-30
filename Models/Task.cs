using System;

namespace ToDoList.Models
{
    public class Task
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime? CompleteDate { get; set; }

        public bool IsCompleted { get; set; }

        public virtual TasksList TaskList { get; set; }
        public int TaskListId { get; set; }

    }
}
