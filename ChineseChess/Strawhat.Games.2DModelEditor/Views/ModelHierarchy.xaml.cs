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
using Strawhat.Games._2DGame;
using System.ComponentModel;
using Strawhat.Games._2DModelEditor.Models;
using Strawhat.Games._2DModelEditor.ExtensionMethods;
using System.Collections.ObjectModel;

namespace Strawhat.Games._2DModelEditor.Views
{
    /// <summary>
    /// Interaction logic for ModelHierarchy.xaml
    /// </summary>
    public partial class ModelHierarchy : UserControl, INotifyPropertyChanged
    {
#if DEBUG
        public static readonly string DEGREE = "°";
#else
        public const string DEGREE = "°";
#endif

        public const string METADATA_STRING = "Meta Data";
        public const string FRAMES_STRING = "Frames";

        private _2DGameModel _GameModel;
        public _2DGameModel GameModel
        {
            set
            {
                if (_GameModel != value)
                {
                    _GameModel = value;
                    this.OnPropertyChanged("GameModel");
                    SetGameModel(value);
                }
            }
            get
            {
                return _GameModel;
            }
        }

        public string MetadataString { get { return METADATA_STRING; } }
        public string FramesString { get { return FRAMES_STRING; } }

        public event Action<object> MetadataSelected;
        public event Action<object> FramesSelected;
        public event Action<double, object> AngleSelected;

        public ModelHierarchy()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void SetGameModel(_2DGameModel value)
        {
            var keys = from p in value.Details.Keys orderby p select p;
            foreach (var angle in keys)
            {
                TreeViewItem newAngle = new TreeViewItem();
                newAngle.Header = string.Format("{0}{1}", angle, DEGREE);
                this.framesTreeViewItem.Items.Add(newAngle);
            }
            this.rootTreeViewItem.ExpandSubtree();
        }

        public void AddAngle(double angle, ObservableCollection<_2DGameModelAnimation> animations)
        {
            TreeViewItem newAngle = new TreeViewItem();
            newAngle.Header = string.Format("{0}{1}", angle, DEGREE);
            this.framesTreeViewItem.Items.Add(newAngle);

            if (animations != null)
            {
                this.GameModel.Details.Add(angle, animations);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnMetadataSelected(object selectedObject)
        {
            if (this.MetadataSelected != null)
                this.MetadataSelected(selectedObject);
        }

        private void OnFramesSelected(object selectedObject)
        {
            if (this.FramesSelected != null)
                this.FramesSelected(selectedObject);
        }

        private void OnAngleSelected(double angle, object selectedObject)
        {
            if (this.AngleSelected != null)
                this.AngleSelected(angle, selectedObject);
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem treeViewItem = e.NewValue as TreeViewItem;
            if (treeViewItem != null)
            {
                //SetContextMenu(treeViewItem);
                string header = (string)treeViewItem.Header;
                switch (header)
                {
                    case METADATA_STRING:
                        this.OnMetadataSelected(treeViewItem);
                        break;
                    case FRAMES_STRING:
                        this.OnFramesSelected(treeViewItem);
                        break;
                    default:
                        if (header.EndsWith(DEGREE))
                        {
                            this.OnAngleSelected(treeViewItem.GetAngleFromTreeViewItem(), treeViewItem);
                        }
                        break;
                }
            }
        }

        

        private void framesTreeViewItem_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            
        }

        private void SetContextMenu(TreeViewItem selectedTreeViewItem)
        {
            var treeViewItem = selectedTreeViewItem;
            if (treeViewItem != null)
            {
                if ((string)treeViewItem.Header == FRAMES_STRING)
                {
                    var contextMenu = treeViewItem.ContextMenu;
                    foreach (var item in contextMenu.Items)
                    {
                        var childItem = item as MenuItem;
                        if (childItem != null)
                        {
                            if (childItem.Name != "FRAMES_NEW_ANGLE")
                            {
                                childItem.IsEnabled = false;
                            }
                            else
                            {
                                childItem.IsEnabled = true;
                            }
                        }
                    }
                }
                else if (treeViewItem.IsAngleTreeViewItem())
                {
                    var contextMenu = ((TreeViewItem)treeViewItem.Parent).ContextMenu;
                    foreach (var item in contextMenu.Items)
                    {
                        var childItem = item as MenuItem;
                        if (childItem != null)
                        {
                            childItem.IsEnabled = true;
                        }

                        if (!GlobalSession.Instance.GameModelAnimationClipboard.HasData && (string)childItem.Name == "FRAMES_PASTE")
                        {
                            childItem.IsEnabled = false;
                        }
                    }
                }
            }
        }
    }
}
