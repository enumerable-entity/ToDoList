using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ToDoList.Models
{
    public class Task : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _content;
        public string Content { get {
                return _content;
            }
            set {
                _content = value;
                OnPropertyChanged();
            } }
        [NotMapped]
        public bool IsInEditMode { get; set; }
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
