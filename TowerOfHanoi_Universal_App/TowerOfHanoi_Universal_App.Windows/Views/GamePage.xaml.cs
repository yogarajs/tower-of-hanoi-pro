﻿using System;
using System.Linq;
using System.Threading.Tasks;
using TowerOfHanoi_Universal_App.Common;
using TowerOfHanoi_Universal_App.Logic;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
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

        Game game;
        NavigationHelper navigationHelper;
        LeaderBoard leaderBoard;

        #endregion

        #region Properties

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
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
            this.DataContext = game;
            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
            SizeChanged += GamePage_SizeChanged;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        #endregion

        #region Events

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            SetTheme(game.GameSettings.GameTheme);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.PageState != null)
            {
                game = e.PageState[Constants.GAME_STATE] as Game;
                this.DataContext = game;
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            e.PageState[Constants.GAME_STATE] = this.DataContext;
        }

        void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Add(new SettingsCommand(
            "options", "Options", (handler) => ShowGameSettingsFlyout()));
        }

        void GamePage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var state = string.Empty;
            var applicationView = ApplicationView.GetForCurrentView();
            var size = Window.Current.Bounds;

            if (applicationView.IsFullScreen && applicationView.Orientation == ApplicationViewOrientation.Landscape)
            {
                state = "FullScreenLandscape";
                ChangeDataTemplate(Resources["Standard300x300ItemTemplate"] as DataTemplate);
            }
            else if (size.Width >= 1024 && size.Width <= 1440)
            {
                state = "Filled";
                ChangeDataTemplate(Resources["Filled300x300ItemTemplate"] as DataTemplate);
            }
            else
            {
                state = "Narrow";
                ChangeDataTemplate(Resources["Narrow150x300ItemTemplate"] as DataTemplate);
            }
            VisualStateManager.GoToState(this, state, true);
        }

        void ListViewBase_OnDragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            var disk = e.Items.First() as Disk;
            e.Data.Properties.Add(Constants.SOURCE_DISK, disk);
            e.Data.Properties.Add(Constants.SOURCE_POLE, sender as ListView);
        }

        async void UIElement_OnDrop(object sender, DragEventArgs e)
        {
            object sourceDiskObject;
            e.Data.Properties.TryGetValue(Constants.SOURCE_DISK, out sourceDiskObject);
            var sourceDisk = sourceDiskObject as Disk;

            object sourcePoleObject;
            e.Data.Properties.TryGetValue(Constants.SOURCE_POLE, out sourcePoleObject);
            var sourcePole = sourcePoleObject as ListView;

            var targetPole = sender as ListView;

            if (!sourcePole.Name.Equals(targetPole.Name) && sourceDisk.IsDraggable)
            {
                if (game.MoveDisk(sourcePole.Tag.ToString(), targetPole.Tag.ToString()))
                {
                    if (game.GameSettings.IsGameSoundEnabled)
                        DiskDrop.Play();
                    PlayerMoveDetails.ChangeView(PlayerMoveDetails.ScrollableWidth, PlayerMoveDetails.ScrollableHeight, null, false);
                }
                else
                {
                    await new MessageDialog(Constants.WARNING, Constants.APP_NAME).ShowAsync();
                }

                var gameState = game.GetGameState();
                var messageDialgog = new MessageDialog(string.Empty, Constants.APP_NAME);
                switch (gameState)
                {
                    case GameState.LevelInprogress:
                        break;
                    case GameState.LevelCompleted:
                        messageDialgog.Commands.Add(new UICommand(Constants.RESTART_LEVEL, new UICommandInvokedHandler(CommandHandler)));
                        ShowMessage(messageDialgog, Constants.LEVEL_COMPLETED);
                        break;
                    case GameState.LevelCompletedWithBestMoves:
                        if (game.GameSettings.IsGameSoundEnabled)
                            LevelCompleted.Play();
                        messageDialgog.Commands.Add(new UICommand(Constants.GOTO_NEXT_LEVEL, new UICommandInvokedHandler(CommandHandler)));
                        ShowMessage(messageDialgog,Constants.LEVEL_WIN);
                        break;
                    case GameState.GameCompleted:
                        if (game.GameSettings.IsGameSoundEnabled)
                            LevelCompleted.Play();
                        ShowMessage(messageDialgog, Constants.GAME_OVER);
                        break;
                    default:
                        break;
                }
            }
        }

        void OnGameSettingsChanged(object sender)
        {
            var gameSettingsFlyout = sender as GameSettingsFlyout;
            game.GameSettings.IsPlayerMoveDetailsVisible = (gameSettingsFlyout.FindName("PlayerMoveDetailVisibilityToggleSwitch") as ToggleSwitch).IsOn;
            game.GameSettings.IsAppbarSticky = (gameSettingsFlyout.FindName("AppBarVisibilityToggleSwitch") as ToggleSwitch).IsOn;
            game.GameSettings.IsGameSoundEnabled = (gameSettingsFlyout.FindName("GameSoundEnabledToggleSwitch") as ToggleSwitch).IsOn;
            game.GameSettings.SaveAppSettings();
        }

        void LevelChooserGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
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
            var newTheme = (GameTheme)Convert.ToInt32((e.ClickedItem as Image).Tag.ToString());
            this.game.GameSettings.GameTheme = newTheme;
            SetTheme(newTheme);
            ThemeChooser.Hide();
        }

        void MessageBoxBtn_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var tag = (GameState)Convert.ToInt32((sender as Button).Tag.ToString());
            LevelCompleted.Stop();
        }

        void Leaderboard_Flyout_Opened(object sender, object e)
        {
            BindLeaderBoard();
        }

        void Level_Chooser_Flyout_Opened(object sender, object e)
        {
            LevelChooserGrid.ItemsSource = game.GetGameLevels();
        }

        async void RateAndReviewButtonTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            await RateAndReview();
        }

        async void ShowMessage(MessageDialog messageDialgog, string message)
        {
            leaderBoard.UpdateLeaderBoards(game.CurrentLevel, game.PlayerMoves);
            messageDialgog.Content = message;
            messageDialgog.Commands.Add(new UICommand(Constants.RATE_AND_REVIEW, new UICommandInvokedHandler(CommandHandler)));
            await messageDialgog.ShowAsync();
        }

        #endregion

        #region Private Methods

        void ShowGameSettingsFlyout()
        {
            var gameSettingsFlyout = new GameSettingsFlyout();
            gameSettingsFlyout.IconSource = new BitmapImage(new Uri("ms-appx:///Assets/SmallLogo.scale-100.png"));
            gameSettingsFlyout.SettingsChanged += OnGameSettingsChanged;
            gameSettingsFlyout.IsAppbarSticky = game.GameSettings.IsAppbarSticky;
            gameSettingsFlyout.IsPlayerMoveDetailsVisible = game.GameSettings.IsPlayerMoveDetailsVisible;
            gameSettingsFlyout.IsGameSoundEnabled = game.GameSettings.IsGameSoundEnabled;
            gameSettingsFlyout.Show();
        }

        void ChangeDataTemplate(DataTemplate dataTemplate)
        {
            PoleA.ItemTemplate = dataTemplate;
            PoleB.ItemTemplate = dataTemplate;
            PoleC.ItemTemplate = dataTemplate;
        }

        async void CommandHandler(IUICommand command)
        {
            var commandLabel = command.Label;
            LevelCompleted.Stop();
            switch (commandLabel)
            {
                case Constants.RESTART_LEVEL:
                    game.InitLevel();
                    break;
                case Constants.GOTO_NEXT_LEVEL:
                    game.GotoNextLevel();
                    break;
                case Constants.RATE_AND_REVIEW:
                    await RateAndReview();
                    break;
                default:
                    break;
            }
        }

        async void BindLeaderBoard()
        {
            LeaderboardGrid.ItemsSource = await leaderBoard.GetLeaderBoards();
        }

        async Task RateAndReview()
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(Constants.RATE_AND_REVIEW_URL + Windows.ApplicationModel.Store.CurrentApp.AppId));
        }

        void SetTheme(GameTheme newTheme)
        {
            this.BottomAppBar.RequestedTheme = (newTheme == GameTheme.Day) ? ElementTheme.Light : ElementTheme.Dark;
        }

        #endregion    
    }
}
