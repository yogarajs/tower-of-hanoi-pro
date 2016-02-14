namespace TowerOfHanoi_Universal_App.ViewModels
{
    /// <summary>
    /// Leaderboard view model
    /// </summary>
    public class LeaderBoardViewModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets Level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets BestMoves.
        /// </summary>
        public int BestMoves { get; set; }

        /// <summary>
        /// Gets or sets PlayerMoves.
        /// </summary>
        public string PlayerMoves { get; set; }

        /// <summary>
        /// Gets or sets time for the level.
        /// </summary>
        public string Time { get; set; }

        #endregion
    }
}
