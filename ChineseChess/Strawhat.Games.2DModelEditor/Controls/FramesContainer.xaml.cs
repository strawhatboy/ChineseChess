using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Strawhat.Games._2DGame;
using Strawhat.Games._2DGame.Infrastructure;
using Microsoft.Win32;
using Strawhat.Games._2DModelEditor.Models;
using System.IO;
using System.Collections.ObjectModel;

namespace Strawhat.Games._2DModelEditor.Controls
{
    /// <summary>
    /// Interaction logic for FramesContainer.xaml
    /// </summary>
    public partial class FramesContainer : UserControl, INotifyPropertyChanged
    {
        private _2DGameModelAnimation _GameModelAnimation;
        public _2DGameModelAnimation GameModelAnimation
        {
            set
            {
                if (_GameModelAnimation != value)
                {
                    _GameModelAnimation = value;
                    this.OnPropertyChanged("GameModelAnimation");
                }
            }
            get
            {
                return _GameModelAnimation;
            }
        }

        private _2DGameModelFrame _SelectedFrame;
        public _2DGameModelFrame SelectedFrame
        {
            get
            {
                return _SelectedFrame;
            }
            set
            {
                if (_SelectedFrame != value)
                {
                    _SelectedFrame = value;
                    this.OnPropertyChanged("SelectedFrame");
                    this.OnSelectedFrameChanged(this, value);
                }
            }
        }

        public event Action<FramesContainer, _2DGameModelFrame> SelectedFrameChanged;

        public event Action<FramesContainer, ObservableCollection<_2DGameModelFrame>> SelectedFramesChanged;

        public RelayCommand ImportFrameCommand
        {
            get
            {
                return new RelayCommand(param => this.ImportFrame());
            }
        }

        private void ImportFrame()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = StringResources._TEXTURE_FILE_FILTER_;
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == true)
            {
                this.ImportFramesFromFile(dialog.FileNames);
                MainWindow.Instance.SetCurrentTabEditted();
            }
        }

        private void ImportFrameFromFile(string filePath)
        {
            BitmapImage image = _2DGameTextureHelper.LoadBitmapImageToMemory(filePath);
            _2DGameModelFrame frame = new _2DGameModelFrame(image);
            frame.Name = this.GameModelAnimation.Name;
            if (this.SelectedFrame != null)
            {
                var index = this.GameModelAnimation.Frames.IndexOf(this.SelectedFrame);
                this.GameModelAnimation.Frames.Insert(index, frame);
            }
            else
            {
                this.GameModelAnimation.Frames.Add(frame);
            }
        }

        private void ImportFramesFromFile(IEnumerable<string> filePathes)
        {
            foreach (var file in filePathes)
            {
                this.ImportFrameFromFile(file);
            }
        }

        public FramesContainer()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnSelectedFrameChanged(FramesContainer container, _2DGameModelFrame frame)
        {
            if (this.SelectedFrameChanged != null)
            {
                this.SelectedFrameChanged(container, frame);
            }
        }

        private void OnSelectedFramesChanged(FramesContainer container, ObservableCollection<_2DGameModelFrame> frames)
        {
            if (this.SelectedFramesChanged != null)
            {
                this.SelectedFramesChanged(container, frames);
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox == null)
                return;
            ObservableCollection<_2DGameModelFrame> frames = new ObservableCollection<_2DGameModelFrame>();
            foreach (var frame in listBox.SelectedItems)
            {
                var _frame = frame as _2DGameModelFrame;
                if (_frame != null)
                {
                    frames.Add(_frame);
                }
            }

            this.OnSelectedFramesChanged(this, frames);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.Instance.SetCurrentTabEditted();
        }
    }
}
