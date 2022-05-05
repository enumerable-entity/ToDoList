using System.Windows;
using ToDoList.ViewsModels;

namespace ToDoList
{
    public partial class MainWindow : Window
    {
        public MainWindow(ToDoListDBContext dBcontext, MainViewModel mvm)
        {
            InitializeComponent();
            this.DataContext = mvm;  //Dependency injection
        }
    }
}
