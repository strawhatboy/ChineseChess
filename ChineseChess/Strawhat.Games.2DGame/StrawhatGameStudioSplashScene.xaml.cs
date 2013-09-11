using Strawhat.Games.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Strawhat.Games._2DGame
{
    /// <summary>
    /// Interaction logic for StrawhatGameStudioSplashScene.xaml
    /// </summary>
    public partial class StrawhatGameStudioSplashScene : _2DGameScene
    {
        public StrawhatGameStudioSplashScene()
        {
            InitializeComponent();

            // Splash scene cannot be skipped.
            this.CanBeSkipped = false;
            this.Loaded += StrawhatGameStudioSplashScene_Loaded;
        }

        void StrawhatGameStudioSplashScene_Loaded(object sender, RoutedEventArgs e)
        {
            this.OnBeginStart();
            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += (o, eargs) =>
            {
                MediaPlayer player = new MediaPlayer();
                player.Open(new Uri(
                    string.Format("{0}{1}{2}{1}HEART.WAV",
                    GameResourceFile.CURRENT_DIR,
                    System.IO.Path.DirectorySeparatorChar,
                    StringResources._FOLDERS_RESOURCES_SOUNDS_)
                    ));
                player.Play();
                this.OnStarted();
                Thread.Sleep(1000);
                player.Position = TimeSpan.FromMilliseconds(0);
                player.Volume = 0.8;
                player.Play();
                Thread.Sleep(1000);
                player.Position = TimeSpan.FromMilliseconds(0);
                player.Volume = 0.6;
                player.Play();
                Thread.Sleep(1000);
                player.Position = TimeSpan.FromMilliseconds(0);
                player.Volume = 0.4;
                player.Play();
                Thread.Sleep(1000);
                player.Position = TimeSpan.FromMilliseconds(0);
                player.Volume = 0.2;
                player.Play();
                Thread.Sleep(1000);
                player.Close();
                this.OnBeginEnd();
                this.OnEnded();
            };
            bgw.RunWorkerAsync();
            //player.Open(new Uri(@"C:\Users\Andy\Downloads\Sound_HEART_wav_2\HEART\HEART.WAV"));
            //player.Play();

        }
    }
}
