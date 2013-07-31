using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Strawhat.Games._2DModelEditor.ExtensionMethods
{
    public static class TreeViewItemExtension
    {
        public static bool IsAngleTreeViewItem(this TreeViewItem treeViewItem)
        {
            try
            {
                GetAngleFromTreeViewItem(treeViewItem);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static double GetAngleFromTreeViewItem(this TreeViewItem treeViewItem)
        {
            var angleString = (string)treeViewItem.Header;
            angleString = angleString.Remove(angleString.Length - 1);
            return double.Parse(angleString);
        }
    }
}
