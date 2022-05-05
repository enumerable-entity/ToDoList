using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToDoList.Models
{
    public class Task : INotifyPropertyChanged
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime? CompleteDate { get; set; }

        public bool IsCompleted { get; set; }

        public virtual TasksList TaskList { get; set; }
        public int TaskListId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
