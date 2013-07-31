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
using Strawhat.Games._2DGame;

namespace Strawhat.Games._2DModelEditor.Controls
{
    /// <summary>
    /// Interaction logic for FramePropertyEditor.xaml
    /// </summary>
    public partial class FrameGeneralPage : UserControl, INotifyPropertyChanged
    {
        private _2DGameModelFrame _GameModelFrame;
        public _2DGameModelFrame GameModelFrame
        {
            set
            {
                if (_GameModelFrame != value)
                {
                    _GameModelFrame = value;
                    this.OnPropertyChanged("GameModelFrame");
                }
            }
            get
            {
                return _GameModelFrame;
            }
        }

        public FrameGeneralPage()
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.Instance.SetCurrentTabEditted();
        }
    }
}
