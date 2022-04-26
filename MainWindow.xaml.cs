
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
            //Button mybutton = new Button();
            //mybutton.Width = 100;
            //mybutton.Height = 50;
            //mybutton.Content = "This button";
            //MainGrid.Children.Add(mybutton);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //string text = textbox1.Text;
            //if (text != "") MessageBox.Show(text);
            //MessageBox.Show("WPF IS GOOD");
        }
    }
}
