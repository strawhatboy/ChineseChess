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

namespace Strawhat.Games._2DModelEditor.Controls
{
    /// <summary>
    /// Interaction logic for AttachmentPointPropertyEditor.xaml
    /// </summary>
    public partial class AttachmentPointPropertyEditor : UserControl, INotifyPropertyChanged
    {
        private string _lastSelectAttachmentPointName;

        private _2DGameModelFrame _GameModelFrame;
        public _2DGameModelFrame GameModelFrame
        {
            set
            {
                if (_GameModelFrame != value)
                {
                    _GameModelFrame = value;
                    this.OnPropertyChanged("GameModelFrame");
                    var attachmentPoint = _GameModelFrame.AttachmentPoints.FirstOrDefault(a => a.Name == _lastSelectAttachmentPointName);
                    if (attachmentPoint != null)
                    {
                        this.SelectedAttachmentPoint = attachmentPoint;
                    }
                }
            }
            get
            {
                return _GameModelFrame;
            }
        }

        private _2DGameModelAttachmentPoint _SelectedAttachmentPoint;
        public _2DGameModelAttachmentPoint SelectedAttachmentPoint
        {
            set
            {
                if (_SelectedAttachmentPoint != value)
                {
                    _SelectedAttachmentPoint = value;
                    _lastSelectAttachmentPointName = _SelectedAttachmentPoint.Name;
                    this.OnPropertyChanged("SelectedAttachmentPoint");
                }
            }
            get
            {
                return _SelectedAttachmentPoint;
            }
        }

        public AttachmentPointPropertyEditor()
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
    }
}
