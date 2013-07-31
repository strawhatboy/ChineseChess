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
using Strawhat.Games._2DGame.Infrastructure;
using System.ComponentModel;
using Strawhat.Games._2DGame;
using Strawhat.Games._2DModelEditor.Controller;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Strawhat.Games._2DModelEditor.Controls;
using Strawhat.Games._2DModelEditor.Views;
using Strawhat.Games._2DModelEditor.Models;

namespace Strawhat.Games._2DModelEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
#if DEBUG
        private static readonly string READY = "Ready";
        private static readonly string EDITTED_FLAG = "*";
#else
        private const string READY = "Ready";
        private const string EDITTED_FLAG = "*";
#endif

        private static MainWindow _Instance;
        public static MainWindow Instance
        {
            get { return _Instance; }
        }

        private SelectedObject _SelectedObject;
        public SelectedObject SelectedObject
        {
            set
            {
                if (_SelectedObject != value)
                {
                    _SelectedObject = value;
                    this.OnSelectedObjectChanged(_SelectedObject);
                }
            }
            get
            {
                return _SelectedObject;
            }
        }

        public event Action<SelectedObject> SelectedObjectChanged;

        public MainWindow()
        {
            _Instance = this;
            InitializeComponent();
            App.CompositionContainer.ComposeParts(this);
            this.AddHandler(ClosableTabItem.CloseTabEvent, new RoutedEventHandler(this.CloseTab));
            //this.EdittedFlags = new Dictionary<TabItem, bool>();
            //this.GameModelMappings = new Dictionary<TabItem, _2DGameModel>();
            this.ContextMapping = new Dictionary<TabItem, MainWindowTabItemContext>();
            this.DataContext = this;
            
        }

        #region Commands Definition

        public RelayCommand NewFileCommand
        {
            get
            {
                return new RelayCommand(param => this.NewFile());
            }
        }

        public RelayCommand OpenFileCommand
        {
            get
            {
                return new RelayCommand(param => this.OpenFile());
            }
        }

        private bool _CanCloseFile;
        public bool CanCloseFile
        {
            get
            {
                return _CanCloseFile;
            }
            set
            {
                if (_CanCloseFile != value)
                {
                    _CanCloseFile = value;
                    this.OnPropertyChanged("CanClose");
                }
            }
        }

        public RelayCommand CloseFileCommand
        {
            get
            {
                return new RelayCommand(param => this.CloseFile(), param2 => this.CanCloseFile);
            }
        }

        private bool _CanCloseAll;
        public bool CanCloseAll
        {
            get
            {
                return _CanCloseAll;
            }
            set
            {
                if (_CanCloseAll != value)
                {
                    _CanCloseAll = value;
                    this.OnPropertyChanged("CanCloseAll");
                }
            }
        }

        public RelayCommand CloseAllCommand
        {
            get
            {
                return new RelayCommand(param => this.CloseAll(), param2 => this.CanCloseAll);
            }
        }

        private bool _CanSaveFile;
        public bool CanSaveFile
        {
            get
            {
                return _CanSaveFile;
            }
            set
            {
                if (_CanSaveFile != value)
                {
                    _CanSaveFile = value;
                    this.OnPropertyChanged("CanSave");
                }
            }
        }

        public RelayCommand SaveFileCommand
        {
            get
            {
                return new RelayCommand(param => this.SaveFile(), param2 => this.CanSaveFile);
            }
        }

        private bool _CanSaveAs;
        public bool CanSaveAs
        {
            get
            {
                return _CanSaveAs;
            }
            set
            {
                if (_CanSaveAs != value)
                {
                    _CanSaveAs = value;
                    this.OnPropertyChanged("CanSaveAs");
                }
            }
        }

        public RelayCommand SaveAsCommand
        {
            get
            {
                return new RelayCommand(param => this.SaveAs(), param2 => this.CanSaveAs);
            }
        }

        private bool _CanSaveAll;
        public bool CanSaveAll
        {
            get
            {
                return _CanSaveAll;
            }
            set
            {
                if (_CanSaveAll != value)
                {
                    _CanSaveAll = value;
                    this.OnPropertyChanged("CanSaveAll");
                }
            }
        }

        public RelayCommand SaveAllCommand
        {
            get
            {
                return new RelayCommand(param => this.SaveAll(), param2 => this.CanSaveAll);
            }
        }

        public RelayCommand UndoCommand
        {
            get
            {
                return new RelayCommand(param => this.Undo());
            }
        }

        public RelayCommand RedoCommand
        {
            get
            {
                return new RelayCommand(param => this.Redo());
            }
        }

        private bool _CanCut;
        public bool CanCut
        {
            get
            {
                return _CanCut;
            }
            set
            {
                if (_CanCut != value)
                {
                    _CanCut = value;
                    this.OnPropertyChanged("CanCut");
                }
            }
        }

        public RelayCommand CutCommand
        {
            get
            {
                return new RelayCommand(param => this.Cut(), param2 => this.CanCut);
            }
        }

        private bool _CanCopy;
        public bool CanCopy
        {
            get
            {
                return _CanCopy;
            }
            set
            {
                if (_CanCopy != value)
                {
                    _CanCopy = value;
                    this.OnPropertyChanged("CanCopy");
                }

            }
        }

        public RelayCommand CopyCommand
        {
            get
            {
                return new RelayCommand(param => this.Copy(), param2 => this.CanCopy);
            }
        }

        private bool _CanPaste;
        public bool CanPaste
        {
            get
            {
                return _CanPaste;
            }
            set
            {
                if (_CanPaste != value)
                {
                    _CanPaste = value;
                    this.OnPropertyChanged("CanPaste");
                }
            }
        }

        public RelayCommand PasteCommand
        {
            get
            {
                return new RelayCommand(param => this.Paste(), param2 => this.CanPaste);
            }
        }

        private bool _CanDelete;
        public bool CanDelete
        {
            get
            {
                return _CanDelete;
            }
            set
            {
                if (_CanDelete != value)
                {
                    _CanDelete = value;
                    this.OnPropertyChanged("CanDelete");
                }
            }
        }

        public RelayCommand DeleteCommand
        {
            get
            {
                return new RelayCommand(param => this.Delete(), param2 => this.CanDelete);
            }
        }

        private bool _CanSelectAll;
        public bool CanSelectAll
        {
            get
            {
                return _CanSelectAll;
            }
            set
            {
                if (_CanSelectAll != value)
                {
                    _CanSelectAll = value;
                    this.OnPropertyChanged("CanSelectAll");
                }
            }
        }

        public RelayCommand SelectAllCommand
        {
            get
            {
                return new RelayCommand(param => this.SelectAll(), param2 => this.CanSelectAll);
            }
        }

        private bool _CanModelPreview;
        public bool CanModelPreview
        {
            get
            {
                return _CanModelPreview;
            }
            set
            {
                if (_CanModelPreview != value)
                {
                    _CanModelPreview = value;
                    this.OnPropertyChanged("CanModelPreview");
                }
            }
        }

        public RelayCommand ModelPreviewCommand
        {
            get
            {
                return new RelayCommand(param => this.ModelPreview(), param2 => this.CanModelPreview);
            }
        }

        public event Action NewFileCommandExecuted;
        public event Action OpenFileCommandExecuted;
        public event Action CloseFileCommandExecuted;
        public event Action CloseAllCommandExecuted;
        public event Action SaveFileCommandExecuted;
        public event Action SaveAsCommandExecuted;
        public event Action SaveAllCommandExecuted;
        public event Action UndoCommandExecuted;
        public event Action RedoCommandExecuted;
        public event Action CutCommandExecuted;
        public event Action CopyCommandExecuted;
        public event Action PasteCommandExecuted;
        public event Action DeleteCommandExecuted;
        public event Action SelectAllCommandExecuted;
        public event Action ModelPreviewCommandExecuted;

        private void OnCommandExecuted(Action command)
        {
            if (command != null)
                command();
        }

        public void NewFile()
        {
            OnCommandExecuted(this.NewFileCommandExecuted);
            //MessageBox.Show("New File!!!!!!!!!!");
        }

        public void OpenFile()
        {
            OnCommandExecuted(this.OpenFileCommandExecuted);
        }

        public void CloseFile()
        {
            OnCommandExecuted(this.CloseFileCommandExecuted);
        }

        public void CloseAll()
        {
            OnCommandExecuted(this.CloseAllCommandExecuted);
        }

        public void SaveFile()
        {
            OnCommandExecuted(this.SaveFileCommandExecuted);
        }

        public void SaveAs()
        {
            OnCommandExecuted(this.SaveAsCommandExecuted);
        }

        public void SaveAll()
        {
            OnCommandExecuted(this.SaveAllCommandExecuted);
        }

        public void Undo()
        {
            OnCommandExecuted(this.UndoCommandExecuted);
        }

        public void Redo()
        {
            OnCommandExecuted(this.RedoCommandExecuted);
        }

        public void Cut()
        {
            OnCommandExecuted(this.CutCommandExecuted);
        }

        public void Copy()
        {
            OnCommandExecuted(this.CopyCommandExecuted);
        }

        public void Paste()
        {
            OnCommandExecuted(this.PasteCommandExecuted);
        }

        public void Delete()
        {
            OnCommandExecuted(this.DeleteCommandExecuted);
        }

        public void SelectAll()
        {
            OnCommandExecuted(this.SelectAllCommandExecuted);
        }

        public void ModelPreview()
        {
            OnCommandExecuted(this.ModelPreviewCommandExecuted);
        }
        #endregion

        [ImportMany(typeof(BaseController))]
        public List<BaseController> Controllers { set; get; }

        public Dictionary<TabItem, MainWindowTabItemContext> ContextMapping { set; get; }

        //public Dictionary<TabItem, _2DGameModel> GameModelMappings { set; get; }

        //public Dictionary<TabItem, 

        //private Dictionary<TabItem, bool> EdittedFlags { set; get; }

        private TabItem _SelectedTabItem;
        public TabItem SelectedTabItem
        {
            set
            {
                if (_SelectedTabItem != value)
                {
                    _SelectedTabItem = value;
                    this.OnPropertyChanged("SelectedTabItem");
                }
            }
            get
            {
                return _SelectedTabItem;
            }
        }

        internal void LoadFiles(string[] fileNames)
        {
            foreach (var fileName in fileNames)
            {
                LoadFile(fileName);
            }
        }

        private void OnSelectedObjectChanged(SelectedObject selectedObject)
        {
            if (this.SelectedObjectChanged != null)
            {
                this.SelectedObjectChanged(selectedObject);
            }
        }

        internal void LoadFile(string fileName)
        {
            //BackgroundWorker bgw = new BackgroundWorker();
            //bgw.DoWork += (obj, args) =>
            //{
                _2DGameModel gameModel = null;
                Trace.WriteLine(string.Format("Opening file {0}", fileName));
                try
                {
                    gameModel = _2DGameModel.LoadFromFile(fileName);
                    Trace.WriteLine(string.Format("File {0} opened.", fileName));
                }
                catch (Exception e)
                {
                    Trace.WriteLine(string.Format("Exception got when open a 2D model file{0}:{1}", Environment.NewLine, e.ToString()));
                    return;
                }
                Dispatcher.Invoke(new Action(() =>
                {
                    this.AddModel(gameModel, fileName);
                    this.SetStatusMsg(READY);
                }));
            //};
            //bgw.RunWorkerAsync();
        }

        internal void SaveModel(_2DGameModel model, string savedFileName)
        {
            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += (e, args) =>
            {
				Trace.WriteLine(string.Format("Saving model {0} to '{1}'", model.Name, savedFileName));
                model.SaveToFile(savedFileName);
                Trace.WriteLine(string.Format("Completed saving model {0} to '{1}'", model.Name, savedFileName));
            };
            bgw.RunWorkerCompleted += (e, args) =>
            {
            	this.SetStatusMsg(READY);
            };
            bgw.RunWorkerAsync();
        }

        internal void SaveModels(IEnumerable<_2DGameModel> models)
        {

        }

        public void AddModel(_2DGameModel model, string filePath = "")
        {
            ClosableTabItem tabItem = new ClosableTabItem();
            tabItem.Header = model.Name;
            tabItem.Style = App.Current.Resources["ClosableTabItemStyle"] as Style;

            MainWindowTabItemContext context = new MainWindowTabItemContext();
            context.GameModel = model;
            context.FilePath = filePath;
            this.ContextMapping.Add(tabItem, context);


            ModelEditor modelEditor = new ModelEditor();
            modelEditor.GameModel = model;
            tabItem.Content = modelEditor;
            this.MainTabControl.Items.Add(tabItem);
            this.MainTabControl.SelectedItem = tabItem;
        }

        private void CloseTab(object source, RoutedEventArgs args)
        {
            TabItem tabItem = args.Source as TabItem;
            if (tabItem != null)
            {
                var privousSelectedTabItem = this.SelectedTabItem;
                this.SelectedTabItem = tabItem;
                this.CloseFile();
                if (privousSelectedTabItem != tabItem)
                {
                    this.SelectedTabItem = privousSelectedTabItem;
                }
            }
        }

        public void SetCurrentTabEditted()
        {
            this.ContextMapping[this.SelectedTabItem].IsEditted = true;
            var header = (string)this.SelectedTabItem.Header;
            if (header == null)
            {
            	Trace.TraceError("Current Tab has a null header, cannot set it to editted.");
            	return;
            }
            var index = header.LastIndexOf(EDITTED_FLAG);
            if (index != (header.Length - 1))
            {
                this.SelectedTabItem.Header = header + EDITTED_FLAG;
            }
            this.CanSaveFile = true;
            this.CanSaveAll = true;
        }

        public void SetCurrentTabUnEditted()
        {
            var context = this.ContextMapping[this.SelectedTabItem];
            if (context.IsEditted == true)
            {
                context.IsEditted = false;
                var header = ((string)this.SelectedTabItem.Header);
                var index = header.LastIndexOf(EDITTED_FLAG);
                if (index == (header.Length - 1))
                {
                    this.SelectedTabItem.Header = header.Remove(index);
                }
            }
            this.CanSaveFile = false;
        }

        public void SetCloseAndCloseAllMenuItemAvailability()
        {
            if (this.MainTabControl.Items.Count > 0)
            {
                this.CanCloseAll = true;
            }
            else
            {
                this.CanCloseAll = false;
            }

            if (this.SelectedTabItem != null)
            {
                this.CanCloseFile = true;
            }
            else
            {
                this.CanCloseFile = false;
            }
        }

        public void SetModelPreviewAvailability()
        {
            if (this.SelectedTabItem != null)
            {
                this.CanModelPreview = true;
            }
            else
            {
                this.CanModelPreview = false;
            }
        }
        
        public void SetStatusMsg(string msg)
        {
        	this.Dispatcher.Invoke(
        		new Action(
        			()=>
        			{
        				this.statusTextBlock.Text = msg;
        			}));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
