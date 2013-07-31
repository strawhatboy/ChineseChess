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
    public class SaveFileCommandController : BaseController
    {
        public override void OnControl()
        {
            //throw new NotImplementedException();
            MainWindow mainWindow = MainWindow.Instance;
            mainWindow.SaveFileCommandExecuted += () =>
            {
                var tabItem = mainWindow.SelectedTabItem;
                if (tabItem != null)
                {
                    var context = mainWindow.ContextMapping[tabItem];
                    if (context.IsEditted)
                    {
                        if (string.IsNullOrEmpty(context.FilePath))
                        {
                            SaveFileDialog saveFileDialog = new SaveFileDialog();
                            saveFileDialog.Filter = StringResources._2D_MODEL_FILE_FILTER_;
                            if (saveFileDialog.ShowDialog() == true)
                            {
                                mainWindow.SaveModel(context.GameModel, saveFileDialog.FileName);
                                context.FilePath = saveFileDialog.FileName;
                                mainWindow.SetCurrentTabUnEditted();
                            }
                        }
                        else
                        {
                            mainWindow.SaveModel(context.GameModel, context.FilePath);
                            mainWindow.SetCurrentTabUnEditted();
                        }
                    }
                }
            };
        }
    }
}
