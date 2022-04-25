using System.Windows;
using ToDoList.Models;

namespace ToDoList
{

    public partial class ListItemWindow : Window
    {
        //public ListItemm ListItem { get; set; }
        //public ListItemWindow(ListItemm listItem)
        //{
        //    InitializeComponent();

        //    ListItem = listItem;
        //    this.DataContext = ListItem;
        //}
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
