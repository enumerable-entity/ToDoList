namespace ToDoList.Models
{
    internal class ListItemm
    {
        private string title;
        public int Id { get; set; }
        public string Title
        {
            get { return title; }
            set
            {
                title = value;

            }
        }
    }
}
