
namespace TowerOfHanoi_Universal_App.Common
{
    /// <summary>
    /// Constant class
    /// </summary>
    public static class Constants
    {
        #region Common
        
        public const string RESTART_LEVEL = "restart level";
        public const string GOTO_NEXT_LEVEL = "goto next level";
        public const string RATE_AND_REVIEW = "rate and review";
        public const string RATE_AND_REVIEW_URL = "ms-windows-store:reviewapp?appid=";

        #endregion

        #region Game messages

        public const string APP_NAME = "Tower of Hanoi";
        public const string LEVEL_COMPLETED = "Level completed! Try to complete level in best moves.";
        public const string LEVEL_WIN = "Congrats! You have completed level in best moves!!";
        public const string GAME_OVER = "Congrats! You have completed the Game. Thanks for playing, Please share your reviews.";
        public const string WARNING = "Cannot place large disk on top of small disk";
        public const string MOVE_DISK = ". Move disk from {0} to {1} \r\n";
        public const string MOVED_DISK = "{0}. Moved disk from Pole {1} to Pole {2}.\r\n";
        public const string UNDO = "------- Undid previous move -------";
        public const string SOURCE_DISK = "sourceDisk";
        public const string SOURCE_POLE = "sourcePole";
        public const string TOTAL_MOVES = "----------------------------------\r\nTotal moves: {0}\r\n----------------------------------";
        public const string BEST_MOVES = "BEST MOVES FOR LEVEL {0}: {1} move(s)";
        #endregion

        #region Game settings

        public const string IS_PLAYER_MOVE_DETAILS_VISIBLE = "IsPlayerMoveDetailsVisible";
        public const string IS_APPBAR_STICKY = "IsAppbarSticky";
        public const string IS_GAME_SOUND_ENABLED = "IsGameSoundEnabled";
        public const string GAME_THEME = "GameTheme";
        public const string LEADERBOARD_FILEPATH = "TOH_LeaderBoards.json";
        public const string GAME_STATE = "tohGame";
       
        #endregion
    }
}
