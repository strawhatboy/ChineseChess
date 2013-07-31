using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Strawhat.Games._2DModelEditor.Controls;

namespace Strawhat.Games._2DModelEditor.Controller
{
    [Export(typeof(BaseController))]
    public class ModelPreViewCommandController : BaseController
    {
        public override void OnControl()
        {
            //throw new NotImplementedException();
            var mainWindow = MainWindow.Instance;
            mainWindow.ModelPreviewCommandExecuted += () =>
            {
                if (mainWindow.SelectedTabItem != null)
                {
                    var currentModel = mainWindow.ContextMapping[mainWindow.SelectedTabItem].GameModel;
                    AnimationPlayer player = new AnimationPlayer(currentModel);
                    MyDialog dialog = new MyDialog(player, string.Format("Model - {0}", currentModel.Name), MyDialogButton.None);
                    dialog.Show();
                }
            };
        }
    }
}
