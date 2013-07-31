using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Windows;

namespace Strawhat.Games._2DModelEditor.Controller
{
    [Export(typeof(BaseController))]
    public class CloseFileCommandController :BaseController
    {
        public override void OnControl()
        {
            var mainWindow = MainWindow.Instance;
            
            mainWindow.CloseFileCommandExecuted += () =>
            {
                var selectedTabItem = mainWindow.SelectedTabItem;
                if (selectedTabItem != null)
                {
                    var context = mainWindow.ContextMapping[mainWindow.SelectedTabItem];
                    MessageBoxResult result = default(MessageBoxResult);
                    if (context.IsEditted)
                    {
                        var modelName = context.GameModel.Name;
                        result = MessageBox.Show(
                            string.Format("The model {0} was not saved, do you want to save the changes before closing it?",
                                modelName),
                            "Closing unsaved model",
                            MessageBoxButton.YesNoCancel,
                            MessageBoxImage.Warning,
                            MessageBoxResult.Cancel);
                        if (result == MessageBoxResult.Yes)
                        {
                            mainWindow.SaveFile();
                        }
                    }

                    if (result != MessageBoxResult.Cancel)
                        mainWindow.MainTabControl.Items.Remove(mainWindow.SelectedTabItem);
                }

                mainWindow.SetCloseAndCloseAllMenuItemAvailability();
            };
        }
    }
}
