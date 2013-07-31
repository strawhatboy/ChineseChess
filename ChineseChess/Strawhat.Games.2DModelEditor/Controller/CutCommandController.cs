using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Strawhat.Games._2DModelEditor.Models;
using Strawhat.Games._2DModelEditor.ExtensionMethods;
using System.Windows.Controls;
using Strawhat.Games._2DGame;
using Strawhat.Games._2DModelEditor.Controls;

namespace Strawhat.Games._2DModelEditor.Controller
{
    [Export(typeof(BaseController))]
    public class CutCommandController : BaseController
    {
        public override void OnControl()
        {
            MainWindow.Instance.CutCommandExecuted += () =>
            {
                HandleAngleTreeViewItem();
                HandleFrames();
            };
                
        }

        void HandleAngleTreeViewItem()
        {
            var mainWindow = MainWindow.Instance;
            var selectedObject = mainWindow.SelectedObject;
            if (!SelectedObjectChangedController.IsSelectedObjectTreeViewItem(selectedObject))
                return;

            var treeViewItem = selectedObject.Object as TreeViewItem;
            var globalSession = GlobalSession.Instance;

            if (!treeViewItem.IsAngleTreeViewItem())
                return;

            var angle = treeViewItem.GetAngleFromTreeViewItem();

            var gameModel = selectedObject.Args.ElementAt(0) as _2DGameModel;
            var animation = gameModel.Details[angle];

            //Remove angle and animation from game model and the treeview.
            gameModel.Details.Remove(angle);
            var framesTreeViewItem = treeViewItem.Parent as TreeViewItem;
            framesTreeViewItem.Items.Remove(treeViewItem);

            globalSession.GameModelAnimationClipboard.Data1 = angle;
            globalSession.GameModelAnimationClipboard.Data2 = animation;
        }

        void HandleFrames()
        {
            var mainWindow = MainWindow.Instance;
            var selectedObject = mainWindow.SelectedObject;
            if (!SelectedObjectChangedController.IsSelectedObjectFrames(selectedObject))
                return;

            var frames = selectedObject.Object as IEnumerable<_2DGameModelFrame>;
            var container = selectedObject.Args.First() as FramesContainer;

            var globalSession = GlobalSession.Instance;
            globalSession.GameModelFramesClipboard.Data1 = frames;
            globalSession.GameModelFramesClipboard.Data2 = container;

            foreach (var frame in frames)
            {
                container.GameModelAnimation.Frames.Remove(frame);
            }
        }
    }
}
