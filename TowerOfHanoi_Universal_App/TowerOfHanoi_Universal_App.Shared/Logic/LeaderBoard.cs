using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using TowerOfHanoi_Universal_App.Common;
using Windows.Storage;

namespace TowerOfHanoi_Universal_App.Logic
{
    /// <summary>
    /// Class for Game LeaderBoards
    /// </summary>
    public class LeaderBoard
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
        public int PlayerMoves { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LeaderBoard()
        {

        }

        /// <summary>
        /// Constructor to build object graph.
        /// </summary>
        /// <param name="level">Level</param>
        public LeaderBoard(int level)
        {
            Level = level;
            BestMoves = ((int)Math.Pow(2, level)) - 1;
            PlayerMoves = 0;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets leader board data from file.
        /// </summary>
        /// <returns>LeaderBoard data.</returns>
        public async Task<List<LeaderBoard>> GetLeaderBoards()
        {
            var leaderBoards = new List<LeaderBoard>();
            var jsonSerializer = new DataContractJsonSerializer(typeof(List<LeaderBoard>));

            try
            {
                if (await IsFileExists())
                {
                    leaderBoards = await ReadLeaderBoard();
                }
                else
                {
                    leaderBoards = BuildLeaderBoard();
                    WriteLeaderBoard(leaderBoards);
                }
            }
            catch (Exception)
            {
            }
            return leaderBoards;
        }

        /// <summary>
        /// Updates player moves for the given level in leader board.
        /// </summary>
        /// <param name="level">Level</param>
        /// <param name="playerMove">Player move</param>
        public async void UpdateLeaderBoards(int level, int playerMove)
        {
            var leaderBoards = new List<LeaderBoard>();
            var jsonSerializer = new DataContractJsonSerializer(typeof(List<LeaderBoard>));

            try
            {
                if (await IsFileExists())
                {
                    leaderBoards = await ReadLeaderBoard();
                    UpdatePlayerMoveForTheLevel(level, playerMove, leaderBoards);
                    WriteLeaderBoard(leaderBoards);
                }
                else
                {
                    leaderBoards = BuildLeaderBoard();
                    UpdatePlayerMoveForTheLevel(level, playerMove, leaderBoards);
                    WriteLeaderBoard(leaderBoards);
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region Private methods

        private async Task<bool> IsFileExists()
        {
            bool isFileExists = false;
            try
            {
                var file = await ApplicationData.Current.RoamingFolder.GetFileAsync(Constants.LEADERBOARD_FILEPATH);
                isFileExists = file != null;
            }
            catch (FileNotFoundException)
            {
            }
            return isFileExists;
        }

        private async Task<List<LeaderBoard>> ReadLeaderBoard()
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(List<LeaderBoard>));
            using (var stream = await ApplicationData.Current.RoamingFolder.OpenStreamForReadAsync(Constants.LEADERBOARD_FILEPATH))
            {
                return (List<LeaderBoard>)jsonSerializer.ReadObject(stream);
            }
        }

        private List<LeaderBoard> BuildLeaderBoard()
        {
            var leaderBoards = new List<LeaderBoard>();
            for (int i = 1; i < 8; i++)
            {
                leaderBoards.Add(new LeaderBoard(i));
            }
            return leaderBoards;
        }

        private async void WriteLeaderBoard(List<LeaderBoard> leaderBoards)
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(List<LeaderBoard>));
            using (var stream = await ApplicationData.Current.RoamingFolder.OpenStreamForWriteAsync(Constants.LEADERBOARD_FILEPATH, CreationCollisionOption.ReplaceExisting))
            {
                jsonSerializer.WriteObject(stream, leaderBoards);
            }
        }

        private void UpdatePlayerMoveForTheLevel(int level, int playerMove, List<LeaderBoard> leaderBoards)
        {
            var levelDetail = leaderBoards.Where(x => x.Level == level).First();
            if (levelDetail.PlayerMoves == 0)
            {
                levelDetail.PlayerMoves = playerMove;
            }
            else if (levelDetail.PlayerMoves > playerMove)
            {
                levelDetail.PlayerMoves = playerMove;
            }
        }

        #endregion
    }
}
