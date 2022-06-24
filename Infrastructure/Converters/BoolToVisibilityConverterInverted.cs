using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

/// Konwertery wartości dla bindingu
namespace ToDoList.Infrastructure.Converters
{
    /// <summary>
    /// Klasa konwetrująca wartość typu prymitywnego 'bool' w wartość typu 'Visibility' oraz odwrotnie.
    /// </summary>
    public class BoolToVisibilityConverterInverted : IValueConverter
    {
        /// <summary>
        /// Metoda konwertująca wartośc logiczną na typ 'Visibility'
        /// </summary>
        /// <param name="value">Wartość prekazywana dla konwertacji</param>
        /// <returns>Zwraca typ 'object' który może być rzutowany na typ 'Visibility' </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool visibility = false;
            if (value is bool)
            {
                visibility = (bool)value;
            }
            return visibility ? Visibility.Collapsed : Visibility.Visible;
        }
        /// <summary>
        /// Klasa wykonująca odwrotną konwersje
        /// </summary>
        /// <param name="value">Watrość przekazywana dla konwetracji</param>
        /// <returns>Zwraca typ 'object' który może być rzutowany na typ 'bool' </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;
            return (visibility == Visibility.Collapsed);
        }
    }
}
