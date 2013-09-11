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

namespace Strawhat.Games._2DGame
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class _2DGameScene : UserControl, _I2DGameScene
    {
        public _2DGameScene()
        {
        }

        public bool CanBeSkipped
        {
            get;
            internal set;
        }

        public Key SkipKey
        {
            get;
            internal set;
        }

        public short SkipButton
        {
            get;
            internal set;
        }

        public event Action BeginEnd;

        public event Action Ended;

        public event Action BeginStart;

        public event Action Started;

        public void OnBeginStart()
        {
            if (this.BeginStart != null)
            {
                this.Dispatcher.Invoke(this.BeginStart);
            }
        }

        public void OnStarted()
        {
            if (this.Started != null)
            {
                this.Dispatcher.Invoke(this.Started);
            }
        }

        public void OnBeginEnd()
        {
            if (this.BeginEnd != null)
            {
                this.Dispatcher.Invoke(this.BeginEnd);
            }
        }

        public void OnEnded()
        {
            if (this.Ended != null)
            {
                this.Dispatcher.Invoke(this.Ended);
            }
        }
    }
}
