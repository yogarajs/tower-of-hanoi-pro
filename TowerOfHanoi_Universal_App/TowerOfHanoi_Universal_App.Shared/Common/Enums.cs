using System.Runtime.Serialization;

namespace TowerOfHanoi_Universal_App.Common
{
    /// <summary>
    /// Class contains Enums
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// GameState enum
        /// </summary>
        public enum GameState
        {
            LevelInprogress = 0,
            LevelCompleted = 1,
            LevelCompletedWithBestMoves = 2,
            GameCompleted = 3
        }

        /// <summary>
        /// GameTheme enum
        /// </summary>
        [DataContract]
        public enum GameTheme
        {
            [EnumMember]
            Day = 0,
            [EnumMember]
            Night = 1,
        }
    }
}
