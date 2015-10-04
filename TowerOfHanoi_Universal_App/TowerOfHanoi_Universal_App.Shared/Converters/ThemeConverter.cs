using System;
using Windows.UI.Xaml.Data;
using GameTheme = TowerOfHanoi_Universal_App.Common.Enums.GameTheme;

namespace TowerOfHanoi_Universal_App.Converters
{
    public class ThemeConverter : IValueConverter
    {
        /// <summary>
        /// Theme converter for game theme.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="language">Language</param>
        /// <returns>Theme path</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var gameTheme = (GameTheme)value;
            var themePath = string.Empty;
            switch (gameTheme)
            {
                case GameTheme.Day:
                    themePath = "ms-appx:///Assets/Day_Theme.png";
                    break;
                case GameTheme.Night:
                    themePath = "ms-appx:///Assets/Night_Theme.png";
                    break;
            }
            return themePath;
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
