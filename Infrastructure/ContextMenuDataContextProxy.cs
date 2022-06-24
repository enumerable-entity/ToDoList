using System.Windows;
/// Klasy wykonujące wspomagające funkcje
namespace ToDoList.Infrastructure
{
    /// <summary>
    /// Klasa wykonująca forwarding DataContext'u dla menu kontekstowego
    /// </summary>
    public class ContextMenuDataContextProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new ContextMenuDataContextProxy();
        }
        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register("Data", typeof(object), typeof(ContextMenuDataContextProxy), new UIPropertyMetadata(null));
    }
}
