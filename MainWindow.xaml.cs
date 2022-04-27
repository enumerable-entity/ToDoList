
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ToDoList.Models;

namespace ToDoList
{
    public partial class MainWindow : Window
    {

        private ToDoListContext _context;

        public MainWindow(ToDoListContext context)
        {

            _context = context;

            InitializeComponent();
            //Button mybutton = new Button();
            //mybutton.Width = 100;
            //mybutton.Height = 50;
            //mybutton.Content = "This button";
            //MainGrid.Children.Add(mybutton);

            _context.Users.Add(new User()
            {
                LoginName = "Noti",
                Password = "blabalb"
            });

            System.Collections.Generic.List<Category> categories = _context.Users.Find(1).Categories;

            _context.Categories.Where(x => x.UserId == 1).ToList();

            var a = (from Category in _context.Categories
                    where Category.UserId == 1
                    select Category.Name).ToList();

            _context.SaveChanges();

            var variable = _context.GetUserCategories(1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
