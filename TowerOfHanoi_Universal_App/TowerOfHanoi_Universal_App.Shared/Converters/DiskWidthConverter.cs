using System;
using Windows.UI.Xaml.Data;

namespace TowerOfHanoi_Universal_App.Converters
{
    /// <summary>
    /// Converter class to adjust disk width to corresponding screen width.
    /// </summary>
    public class DiskWidthConverter : IValueConverter
    {
        /// <summary>
        /// Converts disk width to adjust screen width.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="language">Language</param>
        /// <returns>DiskWidth</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var returnValue = (int)value;
            int adjustValue;
            int.TryParse(parameter.ToString(), out adjustValue);

            switch (returnValue)
            {
                case 75:
                    returnValue = 85;
                    break;
                case 100:
                    returnValue = 95;
                    break;
                case 125:
                    returnValue = 105;
                    break;
                case 150:
                    returnValue = 115;
                    break;
                case 175:
                    returnValue = 125;
                    break;
                case 200:
                    returnValue = 135;
                    break;
                case 225:
                    returnValue = 145;
                    break;
            }
            returnValue = returnValue + adjustValue;
            return returnValue;
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
