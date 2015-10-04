using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace TowerOfHanoi_Universal_App.Converters
{
    /// <summary>
    /// Converter class to convert bool value to Visibility.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts bool value to Visibility.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="language">Language</param>
        /// <returns>Visibility</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var returnValue = (bool)value;
            return returnValue ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Now we don't need this.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="language">Language</param>
        /// <returns>error</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
