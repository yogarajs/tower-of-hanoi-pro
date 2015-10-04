using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using TowerOfHanoi_Universal_App.Common;
using Windows.Storage;
using GameTheme = TowerOfHanoi_Universal_App.Common.Enums.GameTheme;

namespace TowerOfHanoi_Universal_App.Logic
{
    /// <summary>
    /// Game settings class
    /// </summary>
    [DataContract]
    public class GameSettings : INotifyPropertyChanged
    {
        #region Members

        bool isAppbarSticky;
        bool isPlayerMoveDetailsVisible;
        bool isGameSoundEnabled;
        GameTheme gameTheme;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether game sound is enabled or not.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the game sound is enabled; otherwise, <c>false</c>.
        /// </returns>
        [DataMember]
        public bool IsGameSoundEnabled
        {
            get
            {
                return isGameSoundEnabled;
            }
            set
            {
                if (isGameSoundEnabled != value)
                {
                    isGameSoundEnabled = value;
                    SaveSetting(value, Constants.IS_GAME_SOUND_ENABLED);
                }
            }
        }

        /// <summary>
        /// Gets or sets whether app bar is sticky.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the app bar is sticky; otherwise, <c>false</c>.
        /// </returns>
        [DataMember]
        public bool IsAppbarSticky
        {
            get
            {
                return isAppbarSticky;
            }
            set
            {
                if (isAppbarSticky != value)
                {
                    isAppbarSticky = value;
                    SaveSetting(value, Constants.IS_APPBAR_STICKY);
                }
            }
        }

        /// <summary>
        /// Gets or sets whether player move details section is visible.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the player move details section is visible; otherwise, <c>false</c>.
        /// </returns>
        [DataMember]
        public bool IsPlayerMoveDetailsVisible
        {
            get
            {
                return isPlayerMoveDetailsVisible;
            }
            set
            {
                if (isPlayerMoveDetailsVisible != value)
                {
                    isPlayerMoveDetailsVisible = value;
                    SaveSetting(value, Constants.IS_PLAYER_MOVE_DETAILS_VISIBLE);
                }
            }
        }

        /// <summary>
        /// Gets or sets game theme.
        /// </summary>
        [DataMember]
        public GameTheme GameTheme
        {
            get
            {
                return gameTheme;
            }
            set
            {
                if (gameTheme != value)
                {
                    gameTheme = value;
                    SaveSetting(value.ToString(), Constants.GAME_THEME);
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public GameSettings()
        {
            LoadAppSettings();
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <inheritdoc select="*"/>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves application level settings
        /// </summary>
        public void SaveAppSettings()
        {
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            roamingSettings.Values[Constants.IS_PLAYER_MOVE_DETAILS_VISIBLE] = this.IsPlayerMoveDetailsVisible;
            roamingSettings.Values[Constants.IS_APPBAR_STICKY] = this.IsAppbarSticky;
            roamingSettings.Values[Constants.IS_GAME_SOUND_ENABLED] = this.IsGameSoundEnabled;
            roamingSettings.Values[Constants.GAME_THEME] = this.GameTheme.ToString();
        }

        #endregion

        #region Private Methods

        GameSettings LoadAppSettings()
        {
            ApplicationData.Current.DataChanged += Current_DataChanged;
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values[Constants.IS_PLAYER_MOVE_DETAILS_VISIBLE] != null)
                this.isPlayerMoveDetailsVisible = (bool)roamingSettings.Values[Constants.IS_PLAYER_MOVE_DETAILS_VISIBLE];
            if (roamingSettings.Values[Constants.IS_APPBAR_STICKY] != null)
                this.isAppbarSticky = (bool)roamingSettings.Values[Constants.IS_APPBAR_STICKY];
            if (roamingSettings.Values[Constants.IS_GAME_SOUND_ENABLED] != null)
                this.isGameSoundEnabled = (bool)roamingSettings.Values[Constants.IS_GAME_SOUND_ENABLED];
            if (roamingSettings.Values[Constants.GAME_THEME] != null)
                this.gameTheme = (GameTheme)Enum.Parse(typeof(GameTheme), roamingSettings.Values[Constants.GAME_THEME].ToString());

            return this;
        }

        void Current_DataChanged(ApplicationData sender, object args)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Saves a value to the local settings.
        /// </summary>
        /// <typeparam name="T">The type of the value to save.</typeparam>
        /// <param name="value">The value to save.</param>
        /// <param name="setting">The name of the setting to save the value under.</param>
        void SaveSetting<T>(T value, string setting)
        {
            ApplicationData.Current.RoamingSettings.Values[setting] = value;
            OnPropertyChanged(setting);
        }

        void OnPropertyChanged(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
