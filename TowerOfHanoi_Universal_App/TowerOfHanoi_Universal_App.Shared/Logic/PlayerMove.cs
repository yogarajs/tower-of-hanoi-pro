using System.Runtime.Serialization;

namespace TowerOfHanoi_Universal_App.Logic
{
    /// <summary>
    /// Player move class.
    /// </summary>
    [DataContract]
    public class PlayerMove
    {
        /// <summary>
        /// Gets or sets Move ordinal.
        /// </summary>
        [DataMember]
        public int MoveOrdinal { get; set; }

        /// <summary>
        /// Gets or sets SourcePoleId.
        /// </summary>
        [DataMember]
        public string SourcePoleId { get; set; }

        /// <summary>
        /// Gets or sets TargetPoleId.
        /// </summary>
        [DataMember]
        public string TargetPoleId { get; set; }

        /// <summary>
        /// Gets or sets whether last move is undo.
        /// </summary>
        /// <returns>
        /// <c>true</c> if move is undo of last move; otherwise, <c>false</c>.
        /// </returns>
        [DataMember]
        public bool IsUndo { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PlayerMove()
        {
            MoveOrdinal = 0;
            SourcePoleId = string.Empty;
            TargetPoleId = string.Empty;
            IsUndo = false;
        }
    }
}
