
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ToDoList.Models;

namespace ToDoList
{
    public partial class MainWindow : Window
    {

        private ToDoListDBContext _context;

        public MainWindow(ToDoListDBContext dBcontext)
        {
            _context = dBcontext;
            InitializeComponent();

            User admin = new User()
            {
                LoginName = "admin",
                Password = "admin"
            };
            UserSettings defaultSettings = new UserSettings()
            {
                WindowHeight = 900,
                WindowWidth = 1200,
                DarkMode = true,
                GridSplitterPosition = 200,
                User = admin
            };

            Category defaultCategory = new Category()
            {
                Title = "Default Category",
                User = admin
            };
            TasksList defaultTaskList = new TasksList()
            {
                Name = "Default task list",
                Category = defaultCategory,
            };
            Task defaultTask = new Task()
            {
                Content = "This is a default task",
                CompleteDate = new System.DateTime(2025, 11, 04, 04, 00, 00),
                IsCompleted = false,
                TaskList = defaultTaskList
            };


            _context.Users.Add(admin);
            _context.UserSettings.Add(defaultSettings);
            _context.Categories.Add(defaultCategory);
            _context.TasksLists.Add(defaultTaskList);
            _context.Tasks.Add(defaultTask);


            //_context.Categories.Where(x => x.UserId == 1).ToList();

            //var a = (from Category in _context.Categories
            //        where Category.UserId == 1
            //        select Category.Title).ToList();

            _context.SaveChanges();

            var variable = _context.GetUserCategories(1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
