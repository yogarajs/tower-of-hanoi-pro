using System;
using System.Text;
using TowerOfHanoi_Universal_App.Common;
using TowerOfHanoi_Universal_App.Logic;
using TowerOfHanoi_Universal_App.ViewModels;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TowerOfHanoi_Universal_App.Views
{
    public sealed partial class GameSettingsFlyout : SettingsFlyout
    {
        #region Members

        public delegate void SettingsDelegate(object sender);
        public event SettingsDelegate SettingsChanged;
        StringBuilder bestMoveDetailText;
        public GameSettingsViewModel GameSettingsViewModel { get; set; }

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
            this.DataContext = GameSettingsViewModel;
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
