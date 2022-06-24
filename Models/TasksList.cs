using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ToDoList.Models
{
    /// <summary>
    /// Model reprezentujący listu tasków w danym systemie.
    /// </summary>
    public class TasksList : INotifyPropertyChanged
    {
        /// <summary>
        /// Identyfikator listy zadań
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nazwa listy zadań
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Wskazuje na tryb w którym znajduje sie lista zadań na widoku 
        /// </summary>
        [NotMapped]
        public bool IsInEditMode { get; set; }
        /// <summary>
        /// Wsazuje na to, czy dana lista jest wybrana w TreeView 
        /// </summary>
        [NotMapped]
        public bool IsSelected { get; set; }
        /// <summary>
        /// Lista zadań, które należą do danej listy
        /// </summary>
        public virtual IList<Task> Tasks { get; set; }
        /// <summary>
        /// Kategoria, do krórej należy dana lista zadań
        /// </summary>
        public virtual Category? Category { get; set; }
        /// <summary>
        /// Identyfikator kategorii, do której należy dana lista zadań
        /// </summary>
        public int? CategoryId { get; set; }
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
