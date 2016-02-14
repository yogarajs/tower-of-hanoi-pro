using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using TowerOfHanoi_Universal_App.Common;
using Windows.UI;
using GameState = TowerOfHanoi_Universal_App.Common.Enums.GameState;

namespace TowerOfHanoi_Universal_App.Logic
{
    /// <summary>
    /// Game class which is responsible for game logic and serves as ViewModel.
    /// </summary>
    [DataContract]
    public class Game : INotifyPropertyChanged
    {
        #region Members

        int currentLevel;
        int bestMoves;
        int playerMoves;
        bool canUndoLastMove;
        bool canGoToPreviousLevel;
        bool canGoToNextLevel;
        ObservableCollection<Disk> poleADisks;
        ObservableCollection<Disk> poleBDisks;
        ObservableCollection<Disk> poleCDisks;
        StringBuilder playerMoveDetailText;
        GameCommand gotoPreviousLevelCommand;
        GameCommand gotoNextLevelCommand;
        GameCommand undoLastMoveCommand;
        GameCommand restartLevelCommand;
        GameCommand gameSoundCommand;

        [DataMember]
        ObservableCollection<Disk> DiskSource;

        [DataMember]
        List<PlayerMove> PlayerMoveDetails;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets current level.
        /// <remarks>This property is also used to calculate number of disk for the current level.</remarks>
        /// </summary>
        [DataMember]
        public int CurrentLevel
        {
            get
            {
                return currentLevel;
            }
            set
            {
                if (currentLevel != value)
                {
                    currentLevel = value;
                    OnPropertyChanged("CurrentLevel");
                }
            }
        }

        /// <summary>
        /// Gets or sets best move count for the current level.
        /// </summary>
        [DataMember]
        public int BestMoves
        {
            get
            {
                return bestMoves;
            }
            set
            {
                if (bestMoves != value)
                {
                    bestMoves = value;
                    OnPropertyChanged("BestMoves");
                }
            }
        }

        /// <summary>
        /// Gets or sets player move count.
        /// </summary>
        [DataMember]
        public int PlayerMoves
        {
            get
            {
                return playerMoves;
            }
            set
            {
                if (playerMoves != value)
                {
                    playerMoves = value;
                    OnPropertyChanged("PlayerMoves");
                }
            }
        }

        /// <summary>
        /// Gets or sets whether player can goto previous level.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the player can goto previous level; otherwise, <c>false</c>.
        /// </returns>
        [DataMember]
        public bool CanGoToPreviousLevel
        {
            get
            {
                return canGoToPreviousLevel;
            }
            set
            {
                if (canGoToPreviousLevel != value)
                {
                    canGoToPreviousLevel = value;
                    OnPropertyChanged("CanGoToPreviousLevel");
                }
            }
        }

        /// <summary>
        /// Gets or sets whether player can goto next level.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the player can goto next level; otherwise, <c>false</c>.
        /// </returns>
        [DataMember]
        public bool CanGoToNextLevel
        {
            get
            {
                return canGoToNextLevel;
            }
            set
            {
                if (canGoToNextLevel != value)
                {
                    canGoToNextLevel = value;
                    OnPropertyChanged("CanGoToNextLevel");
                }
            }
        }

        /// <summary>
        /// Gets or sets whether player can undo last move.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the player can undo last move; otherwise, <c>false</c>.
        /// </returns>
        [DataMember]
        public bool CanUndoLastMove
        {
            get
            {
                return canUndoLastMove;
            }
            set
            {
                if (canUndoLastMove != value)
                {
                    canUndoLastMove = value;
                    OnPropertyChanged("CanUndoLastMove");
                }
            }
        }

        /// <summary>
        /// Gets or sets text to be displayed in player move details section.
        /// </summary>
        [DataMember]
        public StringBuilder PlayerMoveDetailText
        {
            get
            {
                return playerMoveDetailText;
            }
            set
            {
                if (playerMoveDetailText != value)
                {
                    playerMoveDetailText = value;
                    OnPropertyChanged("PlayerMoveDetailText");
                }
            }
        }

        /// <summary>
        /// Gets or sets disks in Pole A.
        /// </summary>
        [DataMember]
        public ObservableCollection<Disk> PoleADisks
        {
            get
            {
                return poleADisks;
            }
            set
            {
                poleADisks = value;
                OnPropertyChanged("PoleADisks");
            }
        }

        /// <summary>
        /// Gets or sets disks in Pole B.
        /// </summary>
        [DataMember]
        public ObservableCollection<Disk> PoleBDisks
        {
            get
            {
                return poleBDisks;
            }
            set
            {
                poleBDisks = value;
                OnPropertyChanged("PoleBDisks");
            }
        }

        /// <summary>
        /// Gets or sets disks in Pole C.
        /// </summary>
        [DataMember]
        public ObservableCollection<Disk> PoleCDisks
        {
            get
            {
                return poleCDisks;
            }
            set
            {
                poleCDisks = value;
                OnPropertyChanged("PoleCDisks");
            }
        }

        /// <summary>
        /// Gets or sets game settings.
        /// </summary>
        [DataMember]
        public GameSettings GameSettings { get; set; }

        #endregion

        #region Auto Properties

        /// <summary>
        /// Gets or sets hours for the level.
        /// </summary>
        [DataMember]
        public int Hours { get; set; }

        /// <summary>
        /// Gets or sets minutes for the level.
        /// </summary>
        [DataMember]
        public int Minutes { get; set; }

        /// <summary>
        /// Gets or sets seconds for the level.
        /// </summary>
        [DataMember]
        public int Seconds { get; set; }

        #endregion

        #region Game Commands

        /// <summary>
        /// Command to execute when Undo button is clicked.
        /// </summary>
        public GameCommand UndoLastMoveCommand
        {
            get
            {
                if (undoLastMoveCommand == null)
                {
                    undoLastMoveCommand = new GameCommand(() => this.UndoLastMove());
                }
                return undoLastMoveCommand;
            }
            set
            {
                undoLastMoveCommand = value;
            }
        }

        /// <summary>
        /// Command to execute when goto previous level button is clicked.
        /// </summary>
        public GameCommand GotoPreviousLevelCommand
        {
            get
            {
                if (gotoPreviousLevelCommand == null)
                {
                    gotoPreviousLevelCommand = new GameCommand(() => this.GotoPreviousLevel());
                }
                return gotoPreviousLevelCommand;
            }
            set
            {
                gotoPreviousLevelCommand = value;
            }
        }

        /// <summary>
        /// Command to execute when goto next level button is clicked.
        /// </summary>
        public GameCommand GotoNextLevelCommand
        {
            get
            {
                if (gotoNextLevelCommand == null)
                {
                    gotoNextLevelCommand = new GameCommand(() => this.GotoNextLevel());
                }
                return gotoNextLevelCommand;
            }
            set
            {
                gotoNextLevelCommand = value;
            }
        }

        /// <summary>
        /// Command to execute when restart button is clicked.
        /// </summary>
        public GameCommand RestartLevelCommand
        {
            get
            {
                if (restartLevelCommand == null)
                {
                    restartLevelCommand = new GameCommand(() => this.InitLevel());
                }
                return restartLevelCommand;
            }
            set
            {
                restartLevelCommand = value;
            }
        }

        /// <summary>
        /// Command to execute when Sound button is clicked.
        /// </summary>
        public GameCommand GameSoundSettingsCommand
        {
            get
            {
                if (gameSoundCommand == null)
                {
                    gameSoundCommand = new GameCommand(() => this.OnOrOffGameSound());
                }
                return gameSoundCommand;
            }
            set
            {
                gameSoundCommand = value;
            }
        }

        #endregion

        #region Constructor

        public Game()
        {
            CurrentLevel = 1;
            BestMoves = 0;
            PlayerMoves = 0;
            CanUndoLastMove = false;
            CanGoToPreviousLevel = false;
            CanGoToNextLevel = true;
            PlayerMoveDetails = new List<PlayerMove>();
            PlayerMoveDetailText = new StringBuilder();
            OnPropertyChanged("PlayerMoveDetailText");
            DiskSource = new ObservableCollection<Disk>
                        {
                            new Disk() { IsDraggable = true, Ordinal = 1, Color = Color.FromArgb(255, 148, 0, 211), Height = 36, Width = 75 },
                            new Disk() { IsDraggable = false, Ordinal = 2, Color = Color.FromArgb(255, 106, 0, 225), Height = 36, Width = 100 },
                            new Disk() { IsDraggable = false, Ordinal = 3, Color = Color.FromArgb(255, 0, 0, 255), Height = 36, Width = 125 },
                            new Disk() { IsDraggable = false, Ordinal = 4, Color = Color.FromArgb(255, 0, 100, 0), Height = 36, Width = 150 },
                            new Disk() { IsDraggable = false, Ordinal = 5, Color = Color.FromArgb(255, 255, 255, 0), Height = 36, Width = 175 },
                            new Disk() { IsDraggable = false, Ordinal = 6, Color = Color.FromArgb(255, 255, 140, 0), Height = 36, Width = 200 },
                            new Disk() { IsDraggable = false, Ordinal = 7, Color = Color.FromArgb(255, 255, 0, 0), Height = 36, Width = 225 }
                        };
            GameSettings = new GameSettings();
            InitLevel();
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <inheritdoc select="*"/>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes level.
        /// </summary>
        /// <remarks>This is called for each level and when player restarts the level</remarks>
        public void InitLevel()
        {
            CanGoToPreviousLevel = (CurrentLevel > 1);
            CanGoToNextLevel = (CurrentLevel < 7);
            BestMoves = ((int)Math.Pow(2, CurrentLevel)) - 1;
            PlayerMoves = 0;
            CanUndoLastMove = PlayerMoves > 0;
            PlayerMoveDetails = new List<PlayerMove>();
            PlayerMoveDetailText = new StringBuilder();
            Hours = Minutes = Seconds = 0;

            var disks = new ObservableCollection<Disk>();
            for (var i = 0; i < CurrentLevel; i++)
            {
                disks.Add(DiskSource[i]);
            }

            PoleADisks = new ObservableCollection<Disk>(disks);
            PoleBDisks = new ObservableCollection<Disk>();
            PoleCDisks = new ObservableCollection<Disk>();

            PoleADisks.CollectionChanged += PoleDisks_CollectionChanged;
            PoleBDisks.CollectionChanged += PoleDisks_CollectionChanged;
            PoleCDisks.CollectionChanged += PoleDisks_CollectionChanged;
        }

        /// <summary>
        /// Undo last move.
        /// </summary>
        public void UndoLastMove()
        {
            var lastMove = PlayerMoveDetails.Where(move => !move.IsUndo).OrderBy(move => move.MoveOrdinal).LastOrDefault();
            if (lastMove != null)
                MoveDisk(lastMove.TargetPoleId, lastMove.SourcePoleId, true);
        }

        /// <summary>
        /// On/Off game sounds.
        /// </summary>
        void OnOrOffGameSound()
        {
            // Change the bool value and call SaveSettings method.
            var isGameSoundEnabled = this.GameSettings.IsGameSoundEnabled;
            this.GameSettings.IsGameSoundEnabled = !isGameSoundEnabled;
        }

        /// <summary>
        /// Goto previous level.
        /// </summary>
        public void GotoPreviousLevel()
        {
            CurrentLevel--;
            InitLevel();
        }

        /// <summary>
        /// Goto next level.
        /// </summary>
        public void GotoNextLevel()
        {
            CurrentLevel++;
            InitLevel();
        }

        /// <summary>
        /// Moves disk from source pole to target pole.
        /// </summary>
        /// <param name="sourcePoleId">Souce pole ID</param>
        /// <param name="targetPoleId">Target pole ID</param>
        /// <param name="isUndo"><c>true</c> if the method called to undo last move; otherwise, <c>false</c>.</param>
        public bool MoveDisk(string sourcePoleId, string targetPoleId, bool isUndo = false)
        {
            // Get source pole from source pole ID.
            var sourcePole = GetPole(sourcePoleId);

            // Get target pole from target pole ID.
            var targetPole = GetPole(targetPoleId);

            // Get top disk from source pole.
            var sourceDisk = GetTopDisk(sourcePole);

            // Get top disk from target pole.
            var targetDisk = GetTopDisk(targetPole);

            // target disk will be null when target pole doesn't have any disk.
            if (targetDisk == null || (sourceDisk != null && sourceDisk.Ordinal < targetDisk.Ordinal))
            {
                sourcePole.Remove(sourceDisk);
                targetPole.Insert(0, sourceDisk);

                if (isUndo)
                {
                    PlayerMoves--;
                    var lastMove = PlayerMoveDetails.Where(move => !move.IsUndo).OrderBy(move => move.MoveOrdinal).LastOrDefault();
                    PlayerMoveDetails.Remove(lastMove);
                    PlayerMoveDetails.Add(new PlayerMove()
                    {
                        IsUndo = isUndo
                    });
                    RemovePlayerMoveDetails();
                }
                else
                {
                    PlayerMoves++;
                    AppendPlayerMoveDetails(sourcePoleId, targetPoleId);
                    PlayerMoveDetails.Add(new PlayerMove()
                    {
                        MoveOrdinal = PlayerMoves,
                        SourcePoleId = sourcePoleId,
                        TargetPoleId = targetPoleId,
                    });
                }
                if (sourcePole.Any())
                {
                    sourcePole[0].IsDraggable = true;
                }
                if (targetPole.Count > 1)
                {
                    targetPole[1].IsDraggable = false;
                }
                CanUndoLastMove = PlayerMoves > 0;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets current game state.
        /// </summary>
        /// <returns>GameState</returns>
        public GameState GetGameState()
        {
            var gameState = GameState.LevelInprogress;
            if (PoleCDisks.Count == CurrentLevel)
            {
                if (CurrentLevel == 7)
                    gameState = GameState.GameCompleted;
                else if (BestMoves == PlayerMoves)
                    gameState = GameState.LevelCompletedWithBestMoves;
                else
                    gameState = GameState.LevelCompleted;
            }
            return gameState;
        }

        /// <summary>
        /// Gets game levels.
        /// </summary>
        /// <returns>List of game levels.</returns>
        public List<string> GetGameLevels()
        {
            return new List<string>()
            {
                "1", "2", "3", "4", "5", "6", "7"
            };
        }

        #endregion

        #region Private Methods

        void OnPropertyChanged(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        void PoleDisks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Disk item in e.OldItems)
                {
                    //Removed items
                    item.PropertyChanged -= PropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Disk item in e.NewItems)
                {
                    //Added items
                    item.PropertyChanged += PropertyChanged;
                }
            }
        }

        void AppendPlayerMoveDetails(string sourcePoleId, string targetPoleId)
        {
            PlayerMoveDetailText.AppendFormat(Constants.MOVED_DISK, playerMoves, sourcePoleId, targetPoleId);
            OnPropertyChanged("PlayerMoveDetailText");
        }

        void RemovePlayerMoveDetails()
        {
            PlayerMoveDetailText.AppendLine(Constants.UNDO);
            OnPropertyChanged("PlayerMoveDetailText");
        }

        ObservableCollection<Disk> GetPole(string poleId)
        {
            var pole = new ObservableCollection<Disk>();
            switch (poleId)
            {
                case "A":
                    pole = poleADisks;
                    break;
                case "B":
                    pole = poleBDisks;
                    break;
                case "C":
                    pole = poleCDisks;
                    break;
            }
            return pole;
        }

        Disk GetTopDisk(ObservableCollection<Disk> pole)
        {
            return pole.Any() ? pole[0] : null;
        }

        #endregion
    }
}
