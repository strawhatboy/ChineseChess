using Strawhat.Games.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;

namespace Strawhat.Games._2DGame
{
    [Singleton]
    public static class _2DGameController
    {
        static _2DGameController()
        {
            Application.Current.MainWindow.KeyDown += MainWindow_KeyDown;
            Application.Current.MainWindow.KeyUp += MainWindow_KeyUp;
        }

        static void MainWindow_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {   
        }

        static void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }
    }

}
