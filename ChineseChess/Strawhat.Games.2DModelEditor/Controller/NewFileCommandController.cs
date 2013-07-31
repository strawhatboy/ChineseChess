using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Strawhat.Games._2DGame;
using System.ComponentModel.Composition;
using System.Collections.ObjectModel;

namespace Strawhat.Games._2DModelEditor.Controller
{
    [Export(typeof(BaseController))]
    public class NewFileCommandController : BaseController
    {
        public override void OnControl()
        {
            MainWindow.Instance.NewFileCommandExecuted += () =>
            {
                BackgroundWorker bgw = new BackgroundWorker();
                bgw.DoWork += (obj, args) =>
                {
                    _2DGameModel gameModel = new _2DGameModel();
                    gameModel.Name = "New Game Model";
                    gameModel.Details.Add(0, new ObservableCollection<_2DGameModelAnimation>());
                    var animation = new _2DGameModelAnimation();
                    animation.Name = _2DGameModelAnimation.STAND_BY;
                    gameModel.Details[0].Add(animation);
                    MainWindow.Instance.Dispatcher.Invoke(new Action(() =>
                    {
                        MainWindow.Instance.AddModel(gameModel);
                        MainWindow.Instance.SetCurrentTabEditted(); 
                        MainWindow.Instance.SetCloseAndCloseAllMenuItemAvailability();
                        MainWindow.Instance.SetModelPreviewAvailability();
                    }));
                };
                bgw.RunWorkerAsync();
            };
        }
    }
}
