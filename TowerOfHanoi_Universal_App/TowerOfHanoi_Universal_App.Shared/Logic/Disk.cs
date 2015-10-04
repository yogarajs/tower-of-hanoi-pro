using System.ComponentModel;
using System.Runtime.Serialization;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace TowerOfHanoi_Universal_App.Logic
{
    /// <summary>
    /// Disk class
    /// </summary>
    [DataContract]
    public class Disk : INotifyPropertyChanged
    {
        private bool isDraggable;

        /// <summary>
        /// Gets or sets Ordinal of the disk.
        /// </summary>
        [DataMember]
        public int Ordinal { get; set; }

        /// <summary>
        /// Gets or sets whether disk can be draggable.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the disk can be draggable; otherwise, <c>false</c>.
        /// </returns>
        [DataMember]
        public bool IsDraggable
        {
            get
            {
                return isDraggable;
            }
            set
            {
                if (isDraggable != value)
                {
                    isDraggable = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("IsDraggable"));
                }
            }
        }

        /// <summary>
        /// Gets or sets Foreground color of the disk.
        /// </summary>
        [DataMember] 
        public Color Color { get; set; }

        /// <summary>
        /// Gets Foreground brush for the disk.
        /// </summary>
        public SolidColorBrush Brush
        {
            get
            {
                return new SolidColorBrush(this.Color);
            }
        }

        /// <summary>
        /// Gets or sets height of the disk.
        /// </summary>
        [DataMember]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets width of the disk.
        /// </summary>
        [DataMember]
        public int Width { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Disk()
        {

        }
    }
}
