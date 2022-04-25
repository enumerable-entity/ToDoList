
using System.Windows;
using System.Windows.Controls;
using ToDoList.Models;

namespace ToDoList
{
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {

            InitializeComponent();
            Button mybutton = new Button();
            mybutton.Width = 100;
            mybutton.Height = 50;
            mybutton.Content = "This is new button";
            MainGrid.Children.Add(mybutton);
            
        }
        // добавление
        //private void Add_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    ListItemWindow listItemWindow = new ListItemWindow(new ListItemm());
        //    if (listItemWindow.ShowDialog() == true)
        //    {
        //        ListItemm phone = listItemWindow.ListItem;
        //        db.Phones.Add(phone);
        //        db.SaveChanges();
        //    }
        //}
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string text = textbox1.Text;
            if (text != "") MessageBox.Show(text);

            MessageBox.Show("WPF IS GOOD");
        }
    }
}
