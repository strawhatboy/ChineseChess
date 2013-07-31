using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Strawhat.Games._2DGame
{
    public static class _2DGameTimer
    {
        public static DispatcherTimer Timer;
        static _2DGameTimer()
        {
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            Timer.Start();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            _stopwatch.Start();
        }

        /// <summary>
        /// Tick by milliseconds
        /// </summary>
        public static event Action<int> Tick;

        public static double FPS { get; private set; }

        private static Stopwatch _stopwatch = new Stopwatch();

        private static long _fullMilliseconds = 0;

        static void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            _stopwatch.Stop();
            long alpha = _stopwatch.ElapsedMilliseconds - _fullMilliseconds;
            FPS = 0.001d / (double)alpha;

            if (Tick != null)
            {
                Tick((int)alpha);
            }

            _fullMilliseconds = _stopwatch.ElapsedMilliseconds;
            _stopwatch.Start();
        }
    }
}
