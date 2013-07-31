using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Strawhat.Games;
using System.Collections.ObjectModel;

namespace Strawhat.Games._2DGame
{
    public class _2DGameModelAnimation : GameObject
    {
#if DEBUG
        public static readonly string STAND_BY = "Standby";
#else
        public const string STAND_BY = "Standby";
#endif

        private double _Interval;
        public double Interval
        {
            set
            {
                if (_Interval != value)
                {
                    _Interval = value;
                    this.OnPropertyChanged("Interval");
                }
            }
            get
            {
                return _Interval;
            }
        }

        private ObservableCollection<_2DGameModelFrame> _Frames;
        public ObservableCollection<_2DGameModelFrame> Frames
        {
            set
            {
                if (_Frames != value)
                {
                    _Frames = value;
                    this.OnPropertyChanged("Frames");
                }
            }
            get
            {
                return _Frames;
            }
        }
        public _2DGameModelAnimation()
        {
            this.Frames = new ObservableCollection<_2DGameModelFrame>();
        }
    }
}
