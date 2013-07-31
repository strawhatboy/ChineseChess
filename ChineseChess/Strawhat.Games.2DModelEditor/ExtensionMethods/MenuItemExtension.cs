using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Strawhat.Games._2DModelEditor.ExtensionMethods
{
    public static class MenuItemExtension
    {
        public static MenuItem Clone(this MenuItem menuItem)
        {
            var newMenuItem = new MenuItem();
            newMenuItem.Header = menuItem.Header;
            newMenuItem.Name = menuItem.Name;
            newMenuItem.Command = menuItem.Command;
            //newMenuItem.CommandBindings = menuItem.CommandBindings;
            newMenuItem.CommandParameter = menuItem.CommandParameter;
            newMenuItem.CommandTarget = menuItem.CommandTarget;
            newMenuItem.InputGestureText = menuItem.InputGestureText;
            return newMenuItem;
        }
    }
}
