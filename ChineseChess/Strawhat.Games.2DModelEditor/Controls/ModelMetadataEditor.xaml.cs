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
using System.Collections.ObjectModel;

namespace Strawhat.Games._2DModelEditor.Controls
{
    /// <summary>
    /// Interaction logic for ModelMetadataEditor.xaml
    /// </summary>
    public partial class ModelMetadataEditor : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<_2DGameModelType> _ModelTypes;
        public ObservableCollection<_2DGameModelType> ModelTypes
        {
            get
            {
                return _ModelTypes;
            }
            set
            {
                if (_ModelTypes != value)
                {
                    _ModelTypes = value;
                    this.OnPropertyChanged("ModelTypes");
                }
            }
        }

        private _2DGameModel _GameModel;
        public _2DGameModel GameModel
        {
            set
            {
                if (_GameModel != value)
                {
                    _GameModel = value;
                    this.OnPropertyChanged("GameModel");
                }
            }
            get
            {
                return _GameModel;
            }
        }

        public Action<string, object> MetadataChanged { set; get; }

        public ModelMetadataEditor()
        {
            InitializeComponent();
            this.ModelTypes = new ObservableCollection<_2DGameModelType>();
            this.ModelTypes.Add(_2DGameModelType.Normal);
            this.ModelTypes.Add(_2DGameModelType.SingleDirection);
            this.ModelTypes.Add(_2DGameModelType.HorizontalMirrored);
            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnMetadataChanged(string propertyName, object newValue)
        {
            if (MetadataChanged != null)
            {
                this.MetadataChanged(propertyName, newValue);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.Instance.SetCurrentTabEditted();
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            MainWindow.Instance.SetCurrentTabEditted();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.SetCurrentTabEditted();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainWindow.Instance.SetCurrentTabEditted();
        }
    }
}
