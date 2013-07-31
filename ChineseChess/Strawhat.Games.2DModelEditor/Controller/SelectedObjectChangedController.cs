using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Strawhat.Games._2DModelEditor.Models;
using System.Windows.Controls;
using Strawhat.Games._2DModelEditor.ExtensionMethods;
using Strawhat.Games._2DModelEditor.Views;
using Strawhat.Games._2DGame;
using Strawhat.Games._2DModelEditor.Controls;

namespace Strawhat.Games._2DModelEditor.Controller
{
    [Export(typeof(BaseController))]
    public class SelectedObjectChangedController : BaseController
    {
        public override void OnControl()
        {
            MainWindow.Instance.SelectedObjectChanged += (o) =>
            {
                HandleTreeViewItem(o);
                HandleFrameItem(o);
            };
        }

        public static bool IsSelectedObjectTreeViewItem(SelectedObject selectedObject)
        {
            if (selectedObject.Object.GetType() != typeof(TreeViewItem))
                return false;

            var treeViewItem = selectedObject.Object as TreeViewItem;
            if (treeViewItem == null)
                return false;
            return true;
        }

        private void HandleTreeViewItem(SelectedObject selectedObject)
        {
            if (!IsSelectedObjectTreeViewItem(selectedObject))
            {
                return;
            }
            var treeViewItem = selectedObject.Object as TreeViewItem;

            var mainWindow = MainWindow.Instance;
            var globalSession = GlobalSession.Instance;

            if (treeViewItem.IsAngleTreeViewItem())
            {
                //Enable Cut/Copy/Paste/Delete
                mainWindow.CanCut = true;
                mainWindow.CanCopy = true;
                mainWindow.CanPaste = globalSession.GameModelAnimationClipboard.HasData;
                mainWindow.CanDelete = true;

                //Add these menu item to context menu.
                var framesTreeViewItem = treeViewItem.Parent as TreeViewItem;
                if (framesTreeViewItem != null && framesTreeViewItem.ContextMenu != null)
                {
                    var contextMenu = framesTreeViewItem.ContextMenu;

                    if (contextMenu.Items.Count == 1)
                    {
                        contextMenu.Items.Add(new Separator());
                        contextMenu.Items.Add(mainWindow.MENUITEM_EDIT_CUT.Clone());
                        contextMenu.Items.Add(mainWindow.MENUITEM_EDIT_COPY.Clone());
                        contextMenu.Items.Add(mainWindow.MENUITEM_EDIT_PASTE.Clone());
                        contextMenu.Items.Add(mainWindow.MENUITEM_EDIT_DELETE.Clone());
                    }
                }
            }
            else if ((string)treeViewItem.Header == ModelHierarchy.FRAMES_STRING)
            {
                mainWindow.CanCut = false;
                mainWindow.CanCopy = false;
                mainWindow.CanPaste = false;
                mainWindow.CanDelete = false;

                var contextMenu = treeViewItem.ContextMenu;
                if (contextMenu != null)
                {
                    while (contextMenu.Items.Count > 1)
                    {
                        contextMenu.Items.RemoveAt(1);
                    }

                    if (globalSession.GameModelAnimationClipboard.HasData)
                    {
                        mainWindow.CanPaste = true;
                        contextMenu.Items.Add(new Separator());
                        contextMenu.Items.Add(mainWindow.MENUITEM_EDIT_PASTE.Clone());
                    }
                }
            }
        }

        public static bool IsSelectedObjectFrames(SelectedObject selectedObject)
        {
            if (!(selectedObject.Object is IEnumerable<_2DGameModelFrame>))
                return false;

            var frames = selectedObject.Object as IEnumerable<_2DGameModelFrame>;
            if (frames == null)
                return false;

            //found its frames container:
            if (selectedObject.Args == null)
                return false;

            if (selectedObject.Args.Count() != 1)
                return false;

            var container = selectedObject.Args.First() as FramesContainer;
            if (container == null)
                return false;

            return true;
        }

        private void HandleFrameItem(SelectedObject selectedObject)
        {
            if (!IsSelectedObjectFrames(selectedObject))
                return;
            var container = selectedObject.Args.First() as FramesContainer;
            var mainWindow = MainWindow.Instance;
            var globalSession = GlobalSession.Instance;

            mainWindow.CanCut = true;
            mainWindow.CanCopy = true;
            mainWindow.CanPaste = globalSession.GameModelFramesClipboard.HasData;
            mainWindow.CanDelete = true;
            //mainWindow.CanSelectAll = true;

            var contextMenu = container.LISTBOX_FRAMES.ContextMenu;
            if (contextMenu != null && contextMenu.Items.Count == 1)
            {
                if (container.LISTBOX_FRAMES.Items.Count > 0)
                {
                    contextMenu.Items.Add(new Separator());
                    contextMenu.Items.Add(mainWindow.MENUITEM_EDIT_CUT.Clone());
                    contextMenu.Items.Add(mainWindow.MENUITEM_EDIT_COPY.Clone());
                    contextMenu.Items.Add(mainWindow.MENUITEM_EDIT_PASTE.Clone());
                    contextMenu.Items.Add(mainWindow.MENUITEM_EDIT_DELETE.Clone());
                    //contextMenu.Items.Add(new Separator());
                    //contextMenu.Items.Add(mainWindow.MENUITEM_EDIT_SELECTALL.Clone());
                }
                else
                {
                    while (contextMenu.Items.Count > 1)
                    {
                        contextMenu.Items.RemoveAt(1);
                    }
                }
            }
        }
    }
}
