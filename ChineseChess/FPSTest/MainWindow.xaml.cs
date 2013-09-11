using Strawhat.Games._2DGame;
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

namespace FPSTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //_2DGameTimer.Tick += _2DGameTimer_Tick;
            _2DGameTimer.RegisterEvent("FPSEvent", 50, () =>
                {
                    this.Dispatcher.Invoke(new Action(() =>
                        {
                            this.FPS.Text = string.Format("FPS: {0}", _2DGameTimer.FPS.ToString("f2"));
                            this.UpdateSeconds.Text = string.Format("Update Millisecods: {0}", 200.ToString());
                        }));
                });
        }

        void _2DGameTimer_Tick(int obj)
        {
            this.Title = _2DGameTimer.FPS.ToString();
            this.FPS.Text = string.Format("FPS: {0}", _2DGameTimer.FPS.ToString("f2"));
            this.UpdateSeconds.Text = string.Format("Update Millisecods: {0}", obj.ToString());
        }

    }
}
