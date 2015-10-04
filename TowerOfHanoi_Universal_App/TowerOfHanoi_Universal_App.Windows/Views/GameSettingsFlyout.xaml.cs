using System;
using System.Text;
using TowerOfHanoi_Universal_App.Common;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TowerOfHanoi_Universal_App.Logic;

namespace TowerOfHanoi_Universal_App.Views
{
    public sealed partial class GameSettingsFlyout : SettingsFlyout
    {
        #region Members

        public delegate void SettingsDelegate(object sender);
        public event SettingsDelegate SettingsChanged;
        public bool IsAppbarSticky;
        public bool IsPlayerMoveDetailsVisible;
        public bool IsGameSoundEnabled;
        StringBuilder bestMoveDetailText;

        #endregion

        #region Constructor

        public GameSettingsFlyout()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Events

        void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (SettingsChanged != null)
            {
                SettingsChanged(this);
            }
        }

        void SettingsFlyout_Loaded(object sender, RoutedEventArgs e)
        {
            this.AppBarVisibilityToggleSwitch.IsOn = IsAppbarSticky;
            this.PlayerMoveDetailVisibilityToggleSwitch.IsOn = IsPlayerMoveDetailsVisible;
            this.GameSoundEnabledToggleSwitch.IsOn = IsGameSoundEnabled;
        }

        void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BestMoves != null && LevelSelector.SelectedIndex != 0)
            {
                var selectedLevel = LevelSelector.SelectedIndex;
                var bestMoves = Math.Pow(2, selectedLevel) - 1;
                bestMoveDetailText = GameHelper.CalculateBestMoves(selectedLevel);
                bestMoveDetailText.AppendFormat(Constants.TOTAL_MOVES, bestMoves.ToString());
                BestMoves.Text = bestMoveDetailText.ToString();
                StkPnlBestMoves.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        void CopyButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var dataPackage = new DataPackage();
            dataPackage.SetText(bestMoveDetailText.ToString());
            Clipboard.SetContent(dataPackage);
        }

        #endregion
    }
}
