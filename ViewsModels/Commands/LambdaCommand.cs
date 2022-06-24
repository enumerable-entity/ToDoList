using System;

namespace ToDoList.ViewsModels.Commands
{
    /// <summary>
    /// Klasa implementująca komende.
    /// Uniwersalna klasa, która przyjmuje delegat, reprezentujący wykonywane działanie
    /// </summary>
    public class LambdaCommand : BaseCommand
    {
        /// <summary>
        /// Delegat wykonywany gdy jest wywoływana komenda
        /// </summary>
        private readonly Action<object> _Execute;
        /// <summary>
        /// Delegat wykonywany przed wykonaniem komendy
        /// </summary>
        private readonly Func<object, bool> _CanExecute;
        /// <summary>
        /// Konstruktor klasy inicjalizujący delegaty
        /// </summary>
        /// <param name="Execute">Delegat wykonujący działanie</param>
        /// <param name="CanExecute">Delegat sprawdzający czy można wykonać komende</param>
        /// <exception cref="ArgumentNullException">Wyjątek rzucany jeśli delegat wykonujący działąnie jest 'null'</exception>
        public LambdaCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _CanExecute = CanExecute;
        }
        /// <summary>
        /// Metoda wywoływana dla sprawdzania możliwości wykonania komendy
        /// </summary>
        /// <param name="parameter">Delegat w którym jest wykonywane sprawdzanie</param>
        /// <returns></returns>
        public override bool CanExecute(object parameter) => _CanExecute?.Invoke(parameter) ?? true;
        /// <summary>
        /// Metoda wywoływana dla wykonania działania przez komende
        /// </summary>
        /// <param name="parameter">Delegat wykonujący działanie</param>
        public override void Execute(object parameter) => _Execute(parameter);
    }
}
