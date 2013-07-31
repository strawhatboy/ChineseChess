using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Strawhat.Games._2DGame
{
    [XmlRoot]
    public struct Vector2D : INotifyPropertyChanged
    {
        private double _X;

        [XmlElement]
        public double X { set { if (_X != value) { _X = value; this.OnPropertyChanged("X"); } } get { return _X; } }

        private double _Y;

        [XmlElement]
        public double Y { set { if (_Y != value) { _Y = value; this.OnPropertyChanged("Y"); } } get { return _Y; } }

        public Vector2D(double x, double y)
        {
            _X = x;
            _Y = y;
            PropertyChanged = null;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Vector2D))
                return false;

            var other = (Vector2D)obj;

            if (X == other.X && Y == other.Y)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Vector2D v1, Vector2D v2)
        {
            if (v1.Equals(v2))
                return true;
            return false;
        }

        public static bool operator !=(Vector2D v1, Vector2D v2)
        {
            return !(v1 == v2);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
