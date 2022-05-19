using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToDoList.Models
{
    public class TasksList : INotifyPropertyChanged
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual IList<Task> Tasks { get; set; }

        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
