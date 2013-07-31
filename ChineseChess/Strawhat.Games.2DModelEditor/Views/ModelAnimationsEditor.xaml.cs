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
using Strawhat.Games._2DGame;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Strawhat.Games._2DModelEditor.Controls;

namespace Strawhat.Games._2DModelEditor.Views
{
    /// <summary>
    /// Interaction logic for ModelFramesEditor.xaml
    /// </summary>
    public partial class ModelAnimationsEditor : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<_2DGameModelAnimation> _GameModelAnimations;
        public ObservableCollection<_2DGameModelAnimation> GameModelAnimations
        {
            set
            {
                if (_GameModelAnimations != value)
                {
                    _GameModelAnimations = value;
                    this.OnPropertyChanged("GameModelAnimations");
                    this.SetContents(value);
                }
            }
            get
            {
                //return _GameModelAnimations;
                return this.GetContents();
            }
        }

        private BitmapImage _SelectedFrameImage;
        public BitmapImage SelectedFrameImage
        {
            set
            {
                if (_SelectedFrameImage != value)
                {
                    _SelectedFrameImage = value;
                    this.OnPropertyChanged("SelectedFrameImage");
                }
            }
            get
            {
                //return _GameModelAnimations;
                return this._SelectedFrameImage;
            }
        }


        public ModelAnimationsEditor()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public void SetContents(IEnumerable<_2DGameModelAnimation> animations)
        {
            //Clear all
            this.framesStackPanel.Children.Clear();

            foreach (var animation in animations)
            {
                FramesContainer framesContainer = new FramesContainer();
                framesContainer.GameModelAnimation = animation;
                framesContainer.SelectedFrameChanged += new Action<FramesContainer, _2DGameModelFrame>(framesContainer_SelectedFrameChanged);
                framesContainer.SelectedFramesChanged += new Action<FramesContainer, ObservableCollection<_2DGameModelFrame>>(framesContainer_SelectedFramesChanged);
                this.framesStackPanel.Children.Add(framesContainer);
            }
        }

        void framesContainer_SelectedFramesChanged(FramesContainer framesContainer, ObservableCollection<_2DGameModelFrame> objs)
        {
            MainWindow.Instance.SelectedObject = new Models.SelectedObject(objs, new object[] { framesContainer });
        }

        void framesContainer_SelectedFrameChanged(FramesContainer framesContainer, _2DGameModelFrame obj)
        {
            this.framePropertyEditor.GameModelFrame = obj;
            this.attachmentPointPropertyEditor.GameModelFrame = obj;
            this.SelectedFrameImage = obj.Image;
            MainWindow.Instance.SelectedObject = new Models.SelectedObject(new List<_2DGameModelFrame>() { obj }, new object[] { framesContainer });
        }

        public ObservableCollection<_2DGameModelAnimation> GetContents()
        {
            ObservableCollection<_2DGameModelAnimation> result = new ObservableCollection<_2DGameModelAnimation>();
            foreach (var uiElement in this.framesStackPanel.Children)
            {
                var container = uiElement as FramesContainer;
                if (container != null)
                {
                    result.Add(container.GameModelAnimation);
                }
            }

            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
