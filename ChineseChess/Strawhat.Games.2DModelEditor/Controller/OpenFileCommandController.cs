using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.ComponentModel.Composition;
using Strawhat.Games._2DModelEditor.Models;

namespace Strawhat.Games._2DModelEditor.Controller
{
    [Export(typeof(BaseController))]
    public class OpenFileCommandController : BaseController
    {
        public override void OnControl()
        {
            MainWindow.Instance.OpenFileCommandExecuted += () =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = StringResources._2D_MODEL_FILE_FILTER_;
                openFileDialog.Multiselect = true;
                if (openFileDialog.ShowDialog() == true)
                {
                    MainWindow.Instance.LoadFiles(openFileDialog.FileNames);
                }

                MainWindow.Instance.SetCloseAndCloseAllMenuItemAvailability();
                MainWindow.Instance.SetModelPreviewAvailability();
            };
        }
    }
}
