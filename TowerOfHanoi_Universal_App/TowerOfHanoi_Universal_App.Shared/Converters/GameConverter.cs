using System;
using Windows.UI.Xaml.Data;

namespace TowerOfHanoi_Universal_App.Converters
{
    /// <summary>
    /// Converter class to convert game values to view.
    /// </summary>
    public class GameConverter : IValueConverter
    {
        /// <summary>
        /// Converts game values to represent in view.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="language">Language</param>
        /// <returns>UI format of game values</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string returnValue = String.Empty;
            string labelText = parameter as string;

            switch (labelText)
            {
                case "1": returnValue = String.Format("Best Moves: {0}", (int)value);
                    break;
                case "2": returnValue = String.Format("Level: {0}", (int)value);
                    break;
                case "3": returnValue = String.Format("Player Moves: {0}", (int)value);
                    break;
            }
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
