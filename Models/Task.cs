using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;


/// Modele danego systemu zarządzania zadaniami
namespace ToDoList.Models
{

    /// <summary>
    /// Model reprezentujący zadanie w danym systemie.
    /// </summary>
    public class Task : INotifyPropertyChanged
    {
        /// <summary>
        /// Identyfikator zadania
        /// </summary>
        public int Id { get; set; }
        private string _content;
        /// <summary>
        /// Zawartość zadania
        /// </summary>
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Wskazuje na tryb w którym znajduje sie zadanie na widoku
        /// </summary>
        [NotMapped]
        public bool IsInEditMode { get; set; }
        /// <summary>
        /// Data zakończenia zadania
        /// </summary>
        public DateTime? CompleteDate { get; set; }
        private bool _isCompleted;
        /// <summary>
        /// Status zadania - zakończone lub nie zakończone
        /// </summary>
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                _isCompleted = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Lista zadań, do której należy dane zadanie
        /// </summary>
        public virtual TasksList TaskList { get; set; }
        /// <summary>
        /// Identyfikator listy zadań, do której należy dane zadanie
        /// </summary>
        public int TaskListId { get; set; }
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
