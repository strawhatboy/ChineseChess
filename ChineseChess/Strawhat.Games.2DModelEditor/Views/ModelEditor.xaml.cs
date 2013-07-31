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
using Strawhat.Games._2DModelEditor.Controls;

namespace Strawhat.Games._2DModelEditor.Views
{
    /// <summary>
    /// Interaction logic for ModelEditor.xaml
    /// </summary>
    public partial class ModelEditor : UserControl
    {
        private _2DGameModel _GameModel;
        public _2DGameModel GameModel
        {
            set
            {
                if (_GameModel != value)
                {
                    _GameModel = value;
                    SetGameModel(value);
                }
            }
            get
            {
                return _GameModel;
            }
        }

        public Dictionary<TreeViewItem, UIElement> NavigateMappingTable { set; get; }

        public UIElement CurrentEditPaneContent { set; get; }

        public ModelEditor()
        {
            InitializeComponent();
            NavigateMappingTable = new Dictionary<TreeViewItem, UIElement>();
        }

        private void SetGameModel(_2DGameModel value)
        {
            //throw new NotImplementedException();
            this.modelHierarchy.GameModel = value;
        }

        private void modelHierarchy_MetadataSelected(object selectedObject)
        {
            if (!CheckPaneContent(selectedObject))
            {
                ModelMetadataEditor editor = new ModelMetadataEditor();
                editor.GameModel = this.GameModel;
                AddContentToCurrentEditPane(selectedObject, editor);
            }
            MainWindow.Instance.SelectedObject = new Models.SelectedObject(selectedObject, new object[] { this.GameModel });
        }

        private void modelHierarchy_FramesSelected(object selectedObject)
        {
            if (!CheckPaneContent(selectedObject))
            {
                //ModelFramesEditor editor = new ModelFramesEditor();
                //AddContentToCurrentEditPane(selectedObject, editor);
            }
            MainWindow.Instance.SelectedObject = new Models.SelectedObject(selectedObject, new object[] { this.GameModel });
        }

        private void modelHierarchy_AngleSelected(double angle, object selectedObject)
        {
            if (!CheckPaneContent(selectedObject))
            {
                ModelAnimationsEditor editor = new ModelAnimationsEditor();
                editor.GameModelAnimations = this.GameModel.Details[angle];
                AddContentToCurrentEditPane(selectedObject, editor);
            }
            MainWindow.Instance.SelectedObject = new Models.SelectedObject(selectedObject, new object[] { this.GameModel });
        }

        private bool CheckPaneContent(object selectedObject)
        {
            TreeViewItem item = selectedObject as TreeViewItem;
            if (item != null)
            {
                if (this.NavigateMappingTable.ContainsKey(item))
                {
                    HideCurrentPaneContent();
                    this.CurrentEditPaneContent = this.NavigateMappingTable[item];
                    this.NavigateMappingTable[item].Visibility = System.Windows.Visibility.Visible;
                    return true;
                }
            }
            return false;
        }

        private void AddContentToCurrentEditPane(object selectedObject, UIElement uiElement)
        {
            TreeViewItem item = selectedObject as TreeViewItem;
            this.NavigateMappingTable.Add(item, uiElement);
            if (item != null)
            {
                HideCurrentPaneContent();
                this.CurrentEditPaneContent = uiElement;
                this.editPane.Children.Add(uiElement);
            }
        }

        private void HideCurrentPaneContent()
        {
            if (this.CurrentEditPaneContent != null)
            {
                this.CurrentEditPaneContent.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
