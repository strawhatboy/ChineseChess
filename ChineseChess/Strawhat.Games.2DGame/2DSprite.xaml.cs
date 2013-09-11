using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Strawhat.Games._2DGame
{
    /// <summary>
    /// Interaction logic for _2DSpirit.xaml
    /// </summary>
    public partial class _2DSprite : UserControl, INotifyPropertyChanged
    {
        private BitmapImage _Texture;
        public BitmapImage Texture
        {
            get
            {
                return _Texture;
            }
            set
            {
                if (_Texture != value)
                {
                    _Texture = value;
                    this.OnPropertyChanged("Texture");
                }
            }
        }

        private _2DGameObject _GameObject;
        public _2DGameObject GameObject
        {
            get
            {
                return _GameObject;
            }
            set
            {
                SetGameObject(value);
                _GameObject = value;
                this.OnPropertyChanged("GameObject");
            }
        }

        private void SetGameObject(_2DGameObject value)
        {
            if (this._GameObject != null)
            {
                this._GameObject.TextureChanged -= SetTexture;

            }
            value.TextureChanged += SetTexture;
        }

        private void SetTexture(BitmapImage image)
        {
            this.Texture = image;
        }

        private void SetLocation(Vector2D location)
        {
            this.SetValue(Canvas.LeftProperty, location.X);
            this.SetValue(Canvas.TopProperty, location.Y);
        }

        public _2DSprite()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
