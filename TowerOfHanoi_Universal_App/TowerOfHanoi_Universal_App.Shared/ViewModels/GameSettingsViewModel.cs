
namespace TowerOfHanoi_Universal_App.ViewModels
{
    /// <summary>
    /// GameSettings view model
    /// </summary>
    public class GameSettingsViewModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets appbar visibility
        /// </summary>
        public bool IsAppbarSticky { get; set; }

        /// <summary>
        /// Gets or sets whether to show player move details text.
        /// </summary>
        public bool IsPlayerMoveDetailsVisible { get; set; }

        /// <summary>
        /// Gets or sets whether game sound is enabled or not.
        /// </summary>
        public bool IsGameSoundEnabled { get; set; }

        #endregion
    }
}
