using System;
using Windows.UI.Xaml.Data;

namespace TowerOfHanoi_Universal_App.Converters
{
    /// <summary>
    /// Converter class to convert level to selected index of combobox and viceversa.
    /// </summary>
    public class LevelConverter : IValueConverter
    {
        /// <summary>
        /// Converts current level to selected index.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="language">Language</param>
        /// <returns>SelectedIndex</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var returnValue = (int)value;
            return returnValue - 1;
        }

        /// <summary>
        /// Converts selected index to current level.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="language">Language</param>
        /// <returns>CurrentLevel</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var returnValue = (int)value;
            return returnValue + 1;
        }
    }
}
