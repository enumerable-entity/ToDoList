using System;
using System.Windows.Input;

/// Komendy wykożystywane dla powiązania widoku z ViewModel
namespace ToDoList.ViewsModels.Commands
{
    /// <summary>
    /// Klasa podstawowa dla wszytkisch komend
    /// </summary>
    public abstract class BaseCommand : ICommand
    {
        /// <summary>
        /// Wydażenie zmiany możliwości wykonania komendy
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Metoda wywoływana przed "Execute" sprawdzająca czy komenda może być wykonana
        /// </summary>
        /// <param name="parameter">Delegat przekazywany do metody, który bezpośdenio będzie sprawdzać możliwość wykonania komendy</param>
        /// <returns></returns>
        public abstract bool CanExecute(object parameter);
        /// <summary>
        /// Metoda wykonująca się gdy komenda jest wywołana
        /// </summary>
        /// <param name="parameter">Delegat przekazywany do metody, który bezpośdenio będzie wykonywać coś</param>
        /// <returns></returns>
        public abstract void Execute(object parameter);
    }
}
