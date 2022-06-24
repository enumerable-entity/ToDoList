namespace ToDoList.Models
{

    /// <summary>
    /// Model reprezentujący ustawienia użytkonika programu.
    /// </summary>
    public class UserSettings
    {
        /// <summary>
        /// Identyfikator ustawień
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Użytkownik któremu należą dane ustawienia
        /// </summary>
        public virtual User User { get; set; }
        /// <summary>
        /// Identyfikator użytkownika, któremu należą dane ustawienia
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Szerokość okna programu
        /// </summary>
        public int WindowHeight { get; set; }
        /// <summary>
        /// Wysokość okna programu
        /// </summary>
        public int WindowWidth { get; set; }
        /// <summary>
        /// Pozycja rozdzielacza
        /// </summary>
        public int GridSplitterPosition { get; set; }
        /// <summary>
        /// Tryb kolorowy programu
        /// </summary>
        public bool DarkMode { get; set; }
    }
}
