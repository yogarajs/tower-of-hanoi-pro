using System;
using System.Diagnostics;
using System.Linq;
using TowerOfHanoi_Universal_App.Common;
using TowerOfHanoi_Universal_App.Logic;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using GameState = TowerOfHanoi_Universal_App.Common.Enums.GameState;
using GameTheme = TowerOfHanoi_Universal_App.Common.Enums.GameTheme;

namespace TowerOfHanoi_Universal_App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        #region Members

        NavigationHelper navigationHelper;
        Game game;
        LeaderBoard leaderBoard;
        DispatcherTimer levelTimer;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        #endregion

        #region Construtor

        public GamePage()
        {
            this.InitializeComponent();
            game = new Game();
            leaderBoard = new LeaderBoard();
            levelTimer = new DispatcherTimer();
            levelTimer.Tick += levelTimer_Tick;
            DataContext = game;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            Application.Current.Resuming += new EventHandler<Object>(OnResuming);
        }
       
        #endregion

        #region Events

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            ApplicationView.GetForCurrentView().SuppressSystemOverlays = true;
            await StatusBar.GetForCurrentView().HideAsync();
            SetTheme(game.GameSettings.GameTheme);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        void levelTimer_Tick(object sender, object e)
        {
            ++game.Seconds;
            if (game.Seconds == 60)
            {
                game.Minutes++;
                game.Seconds = 0;
            }
            if (game.Minutes == 60)
            {
                game.Hours++;
                game.Minutes = 0;
            }
            TxtTimer.Text = String.Format("{0:00}:{1:00}:{2:00}", game.Hours, game.Minutes, game.Seconds);
        }

        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (MenuPopup.IsOpen)
            {
                MenuPopup.IsOpen = false;
                e.Handled = true;
            }
            else if (!navigationHelper.CanGoBack())
            {
                levelTimer.Stop();
            }
        }

        void OnResuming(object sender, object e)
        {
            levelTimer.Start();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            levelTimer.Interval = new TimeSpan(0, 0, 1);
            if (e.PageState != null)
            {
                game = e.PageState[Constants.GAME_STATE] as Game;
                this.DataContext = game;
            }
            levelTimer.Start();
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            levelTimer.Stop();
            e.PageState[Constants.GAME_STATE] = this.DataContext;
        }

        void ListViewBase_OnDragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            var disk = e.Items.First() as Disk;
            var sourcePole = sender as ListView;
            sourcePole.ReorderMode = ListViewReorderMode.Disabled;
            e.Data.Properties.Add(Constants.SOURCE_DISK, disk);
            e.Data.Properties.Add(Constants.SOURCE_POLE, sourcePole);
        }

        void UIElement_OnDrop(object sender, DragEventArgs e)
        {
            var message = String.Empty;
            object sourceDiskObject;
            e.Data.Properties.TryGetValue(Constants.SOURCE_DISK, out sourceDiskObject);
            var sourceDisk = sourceDiskObject as Disk;

            object sourcePoleObject;
            e.Data.Properties.TryGetValue(Constants.SOURCE_POLE, out sourcePoleObject);
            var sourcePole = sourcePoleObject as ListView;
            var targetPole = sender as ListView;

            if (sourcePole != null && sourceDisk != null && !sourcePole.Name.Equals(targetPole.Name) && sourceDisk.IsDraggable)
            {
                if (game.MoveDisk(sourcePole.Tag.ToString(), targetPole.Tag.ToString()))
                {
                    if (game.GameSettings.IsGameSoundEnabled)
                        DiskDropSound.Play();
                }
                else
                {
                    ShowMessage(Constants.WARNING, Windows.UI.Xaml.Visibility.Visible, Windows.UI.Xaml.Visibility.Collapsed, Windows.UI.Xaml.Visibility.Collapsed, false);
                }

                var gameState = game.GetGameState();
                switch (gameState)
                {
                    case GameState.LevelInprogress:
                        break;
                    case GameState.LevelCompleted:
                        ShowMessage(Constants.LEVEL_COMPLETED, Windows.UI.Xaml.Visibility.Collapsed, Windows.UI.Xaml.Visibility.Visible, Windows.UI.Xaml.Visibility.Collapsed, true);
                        break;
                    case GameState.LevelCompletedWithBestMoves:
                        if (game.GameSettings.IsGameSoundEnabled)
                            LevelCompletedSound.Play();
                        ShowMessage(Constants.LEVEL_WIN, Windows.UI.Xaml.Visibility.Collapsed, Windows.UI.Xaml.Visibility.Visible, Windows.UI.Xaml.Visibility.Collapsed, true);
                        break;
                    case GameState.GameCompleted:
                        if (game.GameSettings.IsGameSoundEnabled)
                            LevelCompletedSound.Play();
                        ShowMessage(Constants.GAME_OVER, Windows.UI.Xaml.Visibility.Collapsed, Windows.UI.Xaml.Visibility.Collapsed, Windows.UI.Xaml.Visibility.Visible, true);
                        break;
                }
            }
        }

        void Pole_Holding(object sender, Windows.UI.Xaml.Input.HoldingRoutedEventArgs e)
        {
            var disk = e.OriginalSource as Rectangle;
            if (disk != null && disk.AllowDrop)
            {
                var sourcePole = sender as ListView;
                sourcePole.ReorderMode = ListViewReorderMode.Enabled;
                Debug.WriteLine(sourcePole.Name);
            }
        }

        void Pole_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var sourcePole = sender as ListView;
            if (sourcePole != null)
            {
                sourcePole.ReorderMode = ListViewReorderMode.Enabled;
                Debug.WriteLine(sourcePole.Name);
            }
        }

        void LevelChooserGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            MenuPopup.IsOpen = false;
            var newLevel = Convert.ToInt32((e.ClickedItem as string));
            if (newLevel != game.CurrentLevel)
            {
                game.CurrentLevel = newLevel;
                game.InitLevel();
            }
            LevelChooser.Hide();
        }

        void ThemeChooserGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            MenuPopup.IsOpen = false;
            var newTheme = (GameTheme)Convert.ToInt32((e.ClickedItem as Image).Tag.ToString());
            if (newTheme != game.GameSettings.GameTheme)
            {
                game.GameSettings.GameTheme = newTheme;
                SetTheme(newTheme);
            }
            ThemeChooser.Hide();
        }

        void GameMessagePopup_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var tag = (GameState)Convert.ToInt32((sender as Button).Tag.ToString());
            LevelCompletedSound.Stop();
            MessagePopupClosed(tag);
            GameMessagePopup.Hide();
            levelTimer.Start();
        }

        void Leaderboard_Flyout_Opened(object sender, object e)
        {
            BindLeaderBoard();
        }

        void Level_Chooser_Flyout_Opened(object sender, object e)
        {
            LevelChooserGrid.ItemsSource = game.GetGameLevels();
        }

        void MenuStackButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            MenuPopup.IsOpen = true;
            e.Handled = true;
            levelTimer.Stop();
        }

        void GameMessagePopup_Closed(object sender, object e)
        {
            var gameState = game.GetGameState();
            MessagePopupClosed(gameState);
        }

        void PopupBackButtonTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            MenuPopup.IsOpen = false;
            var flyoutBackButton = sender as AppBarButton;
            if (flyoutBackButton != null)
            {
                switch (flyoutBackButton.Tag.ToString())
                {
                    case "Help":
                        HelpFlyout.Hide();
                        break;
                    case "Level":
                        LevelChooser.Hide();
                        break;
                    case "Theme":
                        ThemeChooser.Hide();
                        break;
                    case "LeaderBoard":
                        LeaderBoards.Hide();
                        break;
                }
            }
        }

        async void RateAndReviewButtonTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(Constants.RATE_AND_REVIEW_URL + Windows.ApplicationModel.Store.CurrentApp.AppId));
        }

        void MenuPopupButtonTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            MenuPopup.IsOpen = false;
        }

        void MenuPopupClosed(object sender, object e)
        {
            levelTimer.Start();
        }

        #endregion

        #region Private Methods

        void ShowMessage(string message, Windows.UI.Xaml.Visibility warningButtonVisibility, Windows.UI.Xaml.Visibility confirmationButtonVisibility, Windows.UI.Xaml.Visibility btnRateAndReviewVisibility, bool updateLeaderBoard)
        {
            levelTimer.Stop();
            if (updateLeaderBoard)
            {
                leaderBoard.UpdateLeaderBoards(game.CurrentLevel, game.PlayerMoves, game.Hours, game.Minutes, game.Seconds);
            }
            TxtMessage.Text = message;
            BtnStackWarning.Visibility = warningButtonVisibility;
            BtnStackConfirmation.Visibility = confirmationButtonVisibility;
            BtnRateAndReview.Visibility = btnRateAndReviewVisibility;
            FlyoutBase.ShowAttachedFlyout(this);
        }

        void MessagePopupClosed(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.LevelInprogress:
                    break;
                case GameState.LevelCompleted:
                    game.InitLevel();
                    break;
                case GameState.LevelCompletedWithBestMoves:
                    game.GotoNextLevel();
                    break;
                case GameState.GameCompleted:
                    game.CurrentLevel = 1;
                    game.InitLevel();
                    break;
            }
        }

        void SetTheme(GameTheme newTheme)
        {
            switch (newTheme)
            {
                case GameTheme.Day:
                    var black = new SolidColorBrush(Colors.Black);
                    Line1.Stroke = black;
                    Line2.Stroke = black;
                    Line3.Stroke = black;
                    UndoMove.Foreground = black;
                    UndoMove.RequestedTheme = ElementTheme.Light;
                    TxtTimer.RequestedTheme = ElementTheme.Light;
                    break;
                case GameTheme.Night:
                    var white = new SolidColorBrush(Colors.White);
                    Line1.Stroke = white;
                    Line2.Stroke = white;
                    Line3.Stroke = white;
                    UndoMove.Foreground = white;
                    UndoMove.RequestedTheme = ElementTheme.Dark;
                    TxtTimer.RequestedTheme = ElementTheme.Dark;
                    break;
            }
        }

        async void BindLeaderBoard()
        {
            LeaderboardGrid.ItemsSource = await leaderBoard.GetLeaderBoards();
        }

        #endregion      
    }
}
