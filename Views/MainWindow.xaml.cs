using System.Windows;
using ToDoList.ViewsModels;

namespace ToDoList.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mvm)
        {
            InitializeComponent();
            this.DataContext = mvm;  //Dependency injection

        }
    }
}
