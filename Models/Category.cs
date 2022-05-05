using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToDoList.Models
{
    public class Category : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _title;
        public string Title {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public virtual IList<TasksList> TaskLists { get; set; }

        public virtual User User { get; set; }
        public int UserId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
