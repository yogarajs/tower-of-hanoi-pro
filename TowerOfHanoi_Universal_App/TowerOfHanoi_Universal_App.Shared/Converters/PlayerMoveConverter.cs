using System;
using Windows.UI.Xaml.Data;

namespace TowerOfHanoi_Universal_App.Converters
{
    public class PlayerMoveConverter : IValueConverter
    {
        /// <summary>
        /// Player move converter.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="language">Language</param>
        /// <returns>Theme path</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var playerMove = (int)value;
            return playerMove == 0 ? "-" : playerMove.ToString();
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
