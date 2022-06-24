using System.Collections.Generic;

namespace ToDoList.Models
{

    /// <summary>
    /// Model reprezentujący użytkownika w danym systemie.
    /// Implementacja danej funkcjonalności będzie realizowana póżniej, z powodu braku czasu
    /// </summary>
    public class User
    {
        /// <summary>
        /// Identyfikator użytkownika
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Login użytkownika
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// Hasło użytkownika do systemu
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Stan użytkownika w danym systemie
        /// </summary>
        public bool IsAuthenticated { get; set; }
        /// <summary>
        /// Spersonalizowane ustawienia aplikacji użytkownika
        /// </summary>
        public UserSettings Settings { get; set; }
        /// <summary>
        /// Lista wszytkich kategorii danego użytkownika
        /// </summary>
        public virtual List<Category> Categories { get; set; }

    }
}
