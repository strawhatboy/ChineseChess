using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        //public static DispatcherTimer Timer;
        static _2DGameTimer()
        {
            //Timer = new DispatcherTimer();
            //Timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            //Timer.Start();
            bgw.DoWork += (o, ea) =>
            {
                _stopwatch.Stop();
                long alpha = _stopwatch.ElapsedMilliseconds - _fullMilliseconds;
                FPS = 1000d / (double)alpha;

                if (Tick != null)
                {
                    Tick((int)alpha);
                }
                _fullMilliseconds = _stopwatch.ElapsedMilliseconds;

                
                foreach (var @event in Actions.Keys)
                {
                    ActionsMilliseconds[@event] += (int)alpha;
                    long count = ActionsMilliseconds[@event] / Actions[@event].Interval;
                    if (count > ActionsMillisecondsCount[@event])
                    {
                        ActionsMilliseconds[@event] = count;
                        Actions[@event].Action();
                    }
                }

                _stopwatch.Start();
            };
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

        private static BackgroundWorker bgw = new BackgroundWorker();

        static void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            

            if (!bgw.IsBusy)
            {
                bgw.RunWorkerAsync();
            }
        }

        static Dictionary<string, _2DTimerEventArgs> Actions = new Dictionary<string, _2DTimerEventArgs>();
        static Dictionary<string, long> ActionsMilliseconds = new Dictionary<string, long>();
        static Dictionary<string, long> ActionsMillisecondsCount = new Dictionary<string, long>();

        /// <summary>
        /// Register an event. You must use Dispatcher.Invoke in your actions if there's UI changing code in it
        /// because the action will be invoked in another thread to improve the performance.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="milliseconds"></param>
        /// <param name="action"></param>
        public static void RegisterEvent(string eventName, int milliseconds, Action action)
        {
            if (!Actions.ContainsKey(eventName))
            {
                Actions.Add(eventName, new _2DTimerEventArgs { Interval = milliseconds, Action = action });
                ActionsMilliseconds.Add(eventName, 0);
                ActionsMillisecondsCount.Add(eventName, 0);
            }
            else
            {
                Actions[eventName].Interval = milliseconds;
                Actions[eventName].Action = action;
                ActionsMilliseconds[eventName] = 0;
                ActionsMillisecondsCount[eventName] = 0;
            }
        }

        /// <summary>
        /// Change an event's Interval, it will throw an exception if no such event.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="milliseconds"></param>
        /// <exception cref="InvalidOperationException" />
        public static void ChangeEventInterval(string eventName, int milliseconds)
        {
            if (!Actions.ContainsKey(eventName))
            {
                throw new InvalidOperationException(string.Format("Cannot change the non-exist event {0}", eventName));
            }
            else
            {
                Actions[eventName].Interval = milliseconds;
            }
        }

        /// <summary>
        /// Change an event's Action, it will throw an exception if no such event.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="milliseconds"></param>
        /// <exception cref="InvalidOperationException" />
        public static void ChangeEventInterval(string eventName, Action action)
        {
            if (!Actions.ContainsKey(eventName))
            {
                throw new InvalidOperationException(string.Format("Cannot change the non-exist event {0}", eventName));
            }
            else
            {
                Actions[eventName].Action = action;
            }
        }

        public static void UnloadEvent(string eventName)
        {
            if (!Actions.ContainsKey(eventName))
            {
                Trace.WriteLine(string.Format("No such event {0} to unload", eventName));
            }
            else
            {
                Actions.Remove(eventName);
                ActionsMilliseconds.Remove(eventName);
                ActionsMillisecondsCount.Remove(eventName);
            }
        }

    }

    public class _2DTimerEventArgs : EventArgs
    {
        public int Interval { set; get; }
        public Action Action { set; get; }
    }
}
