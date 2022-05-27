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
        private bool _isCompleted;
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                _isCompleted = value;
                OnPropertyChanged();
            }
        }

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
