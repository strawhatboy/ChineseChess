using Strawhat.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;

namespace Strawhat.Games._2DGame
{
    public class _2DGameModelFrame : GameObject
    {
        private ObservableCollection<_2DGameModelAttachmentPoint> _AttachmentPoints;
        public ObservableCollection<_2DGameModelAttachmentPoint> AttachmentPoints
        {
            set
            {
                if (_AttachmentPoints != value)
                {
                    _AttachmentPoints = value;
                    this.OnPropertyChanged("AttachmentPoints");
                }
            }
            get
            {
                return _AttachmentPoints;
            }
        }

        private BitmapImage _Image;
        public BitmapImage Image
        {
            set
            {
                if (_Image != value)
                {
                    _Image = value;
                    this.SetOriginAttachmentPoint(value);
                    this.OnPropertyChanged("Image");
                }
            }
            get
            {
                return _Image;
            }
        }

        private int _OccupiedFrameCount = 1;
        public int OccupiedFrameCount
        {
            set
            {
                if (_OccupiedFrameCount != value)
                {
                    _OccupiedFrameCount = value;
                    this.OnPropertyChanged("OccupiedFrameCount");
                }
            }
            get
            {
                return _OccupiedFrameCount;
            }
        }

        public _2DGameModelFrame()
        {
            AttachmentPoints = new ObservableCollection<_2DGameModelAttachmentPoint>();
        }

        public _2DGameModelFrame(BitmapImage image) : this()
        {
            this.Image = image;
        }

        private void SetOriginAttachmentPoint(BitmapImage image)
        {
            if (image != null)
            {
                var origin = this.AttachmentPoints.FirstOrDefault(a => a.Name == _2DGameModelAttachmentPoint.ORIGIN);
                if (origin == null)
                {
                    origin = new _2DGameModelAttachmentPoint();
                    origin.Name = _2DGameModelAttachmentPoint.ORIGIN;
                    origin.AttachAngle = 0.0;
                    origin.AttachPosition = new Vector2D(image.PixelWidth / 2, image.PixelHeight / 2);
                    this.AttachmentPoints.Add(origin);
                }
            }
        }
    }
}
