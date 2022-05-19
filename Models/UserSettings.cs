namespace ToDoList.Models
{
    public class UserSettings
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }

        // Custom application settings for user

        public int WindowHeight { get; set; }
        public int WindowWidth { get; set; }
        public int GridSplitterPosition { get; set; }
        public bool DarkMode { get; set; }
    }
}
