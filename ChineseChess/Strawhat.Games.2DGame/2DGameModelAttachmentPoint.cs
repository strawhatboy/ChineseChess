using Strawhat.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Strawhat.Games._2DGame
{
    [XmlRoot]
    public class _2DGameModelAttachmentPoint : GameObject
    {
#if DEBUG
        public static readonly string ORIGIN = "Origin";
#else
        public const string ORIGIN = "Origin";
#endif
        
        private Vector2D _AttachPosition;

        [XmlElement]
        public Vector2D AttachPosition
        {
            set
            {
                if (_AttachPosition != value)
                {
                    _AttachPosition = value;
                    this.OnPropertyChanged("AttachPosition");
                }
            }
            get
            {
                return _AttachPosition;
            }
        }

        private double _AttachAngle;

        [XmlElement]
        public double AttachAngle 
        {
            set
            {
                if (_AttachAngle != value)
                {
                    _AttachAngle = value;
                    this.OnPropertyChanged("AttachAngle");
                }
            }
            get
            {
                return _AttachAngle;
            }
        }
    }
}
