using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace TowerOfHanoi_Universal_App.Converters
{
    public class IconConverter : IValueConverter
    {
        /// <summary>
        /// Icon converter for game sound setting's button.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="language">Language</param>
        /// <returns>Icon</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isEnabled = (bool)value;
            return isEnabled ? new SymbolIcon(Symbol.Volume) : new SymbolIcon(Symbol.Mute);
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
