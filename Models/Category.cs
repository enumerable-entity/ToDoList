using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ToDoList.Models
{
    /// <summary>
    /// Model reprezentujący kategorie zadań w danym systemie
    /// </summary>
    public class Category : INotifyPropertyChanged
    {
        /// <summary>
        /// Identyfikator kategorii
        /// </summary>
        public int Id { get; set; }
        private string _title;
        /// <summary>
        /// Nazwa kategorii
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Lista listów zadań, należących do danej kategorii
        /// </summary>
        public virtual ObservableCollection<TasksList> TaskLists { get; set; }
        /// <summary>
        /// Wskazuje na tryb w którym znajduje sie kategoria na widoku 
        /// </summary>
        [NotMapped]
        public bool IsInEditMode { get; set; }
        /// <summary>
        /// Wsazuje na to, czy dana kategoria jest rozwinięta w TreeView 
        /// </summary>
        [NotMapped]
        public bool IsExpanded { get; set; }
        /// <summary>
        /// Wsazuje na to, czy dana kategoria jest wybrana w TreeView 
        /// </summary>
        [NotMapped]
        public bool IsSelected { get; set; }
        /// <summary>
        /// Użytkownik któremu należy dana kategoria
        /// </summary>
        public virtual User User { get; set; }
        /// <summary>
        /// Identyfikator użytkownika, któremu należy dana kategoria
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Wydażenie wywoływane gdy zmienia się stan modelu 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Metoda wywołująca wydarzenie 
        /// </summary>
        /// <param name="propertyName">Nazwa property który sie zmieniło. Przekazywana za pośrednictwem Reflection</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
