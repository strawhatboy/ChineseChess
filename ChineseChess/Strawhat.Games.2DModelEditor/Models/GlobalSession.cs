using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;

using Strawhat.Games._2DGame;
using Strawhat.Games._2DModelEditor.Controls;
using Strawhat.Games.Utilities;

namespace Strawhat.Games._2DModelEditor.Models
{
    public class GlobalSession
    {
        private static GlobalSession _Instance = new GlobalSession();
        public static GlobalSession Instance
        {
            get
            {
//                if (_Instance == null)
//                    _Instance = new GlobalSession();
                return _Instance;
            }
        }

        public Timer Timer { set; get; }
        
        private FileLogger logger = new FileLogger();

        private GlobalSession()
        {
            GameModelAnimationClipboard = new MyClipboard<double?, ObservableCollection<_2DGameModelAnimation>>();
            GameModelFramesClipboard = new MyClipboard<IEnumerable<_2DGameModelFrame>, FramesContainer>();
            this.Timer = new Timer(1);
            this.Timer.Start();
            Trace.Listeners.Add(this.logger);
            Trace.Listeners.Add(new EventLogTraceListener(new EventLog("Application", ".", "Strawhat.Games.2DModelEditor")));
            //Trace.WriteLine("Global Session Initialized.");
        }

        public MyClipboard<double?, ObservableCollection<_2DGameModelAnimation>> GameModelAnimationClipboard { set; get; }

        public MyClipboard<IEnumerable<_2DGameModelFrame>, FramesContainer> GameModelFramesClipboard { set; get; }
    }
}
