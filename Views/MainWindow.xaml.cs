using System.Windows;
using ToDoList.ViewsModels;

/// Widoki aplikacji, według wzorca MVVM
namespace ToDoList.Views
{
    /// <summary>
    /// Reprezentuje główne okno programu.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Konstruktor głównego okna.
        /// Przyjmuje ViewModel poprzed wstrzykiwanie zależnośći
        /// </summary>
        /// <param name="mvm">ViewModel powiązany z danym widokiem</param>
        public MainWindow(MainViewModel mvm)
        {
            InitializeComponent();
            this.DataContext = mvm;  //Dependency injection

        }
    }
}
