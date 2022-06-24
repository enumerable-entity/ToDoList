using System.ComponentModel;
using System.Windows;

namespace ToDoList.Views
{
    /// <summary>
    /// Okno informacji o danym programie
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
        }
    }
}
