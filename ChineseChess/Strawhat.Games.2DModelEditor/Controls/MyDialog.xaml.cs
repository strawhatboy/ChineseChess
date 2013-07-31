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
using System.Windows.Shapes;
using System.ComponentModel;
using Strawhat.Games._2DGame.Infrastructure;

namespace Strawhat.Games._2DModelEditor.Controls
{
    public enum MyDialogButton
    {
        None,
        OK,
        OKCancel,
        YesNo,
        YesNoCancel,
    }

    public enum MyDialogResult
    {
        None,
        OK,
        Yes,
        No,
        Cancel,
    }


    /// <summary>
    /// Interaction logic for MyDialog.xaml
    /// </summary>
    public partial class MyDialog : Window, INotifyPropertyChanged
    {
        private bool _CanOK;
        public bool CanOK { set { if (_CanOK != value) { _CanOK = value; this.OnPropertyChanged("CanOK"); } } get { return _CanOK; } }

        private bool _CanYes;
        public bool CanYes { set { if (_CanYes != value) { _CanYes = value; this.OnPropertyChanged("CanYes"); } } get { return _CanYes; } }

        private bool _CanNo;
        public bool CanNo { set { if (_CanNo != value) { _CanNo = value; this.OnPropertyChanged("CanNo"); } } get { return _CanNo; } }

        private bool _CanCanel;
        public bool CanCanel { set { if (_CanCanel != value) { _CanCanel = value; this.OnPropertyChanged("CanCanel"); } } get { return _CanCanel; } }

        public RelayCommand OKCommand { get { return new RelayCommand(a => OK(), b => CanOK); } }

        public RelayCommand YesCommand { get { return new RelayCommand(a => Yes(), b => CanYes); } }

        public RelayCommand NoCommand { get { return new RelayCommand(a => No(), b => CanNo); } }

        public RelayCommand CanelCommand { get { return new RelayCommand(a => Cancel(), b => CanCanel); } }

        public void OK()
        {
        }

        public void Yes()
        {
        }

        public void No()
        {
        }

        public void Cancel()
        {
        }

        public MyDialog() : this("Dialog Message", "Dialog Caption", MyDialogButton.OK)
        {
        }

        public MyDialog(string message, string caption, MyDialogButton buttons)
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public MyDialog(FrameworkElement innerElement, string caption, MyDialogButton buttons)
        {
            InitializeComponent();
            this.DataContext = this;
            this.mainContainer.Children.Add(innerElement);
        }

        private void SetButtons(MyDialogButton buttons)
        {
            switch (buttons)
            {
                case MyDialogButton.None:
                    this.CanOK = false;
                    this.CanYes = false;
                    this.CanNo = false;
                    this.CanCanel  =false;
                    break;
                case MyDialogButton.OK:
                    this.CanOK = true;
                    this.CanYes = false;
                    this.CanNo = false;
                    this.CanCanel  =false;
                    break;
                case MyDialogButton.OKCancel:
                    this.CanOK = true;
                    this.CanYes = false;
                    this.CanNo = false;
                    this.CanCanel = true;
                    break;
                case MyDialogButton.YesNo:
                    this.CanOK = false;
                    this.CanYes = true;
                    this.CanNo = true;
                    this.CanCanel = false;
                    break;
                case MyDialogButton.YesNoCancel:
                    this.CanOK = false;
                    this.CanYes = true;
                    this.CanNo = true;
                    this.CanCanel = true;
                    break;
                default:
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
