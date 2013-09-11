using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Strawhat.Games._2DModelEditor.Controller
{
    [Export(typeof(OptionsCommandController))]
    public class OptionsCommandController : BaseController
    {
        public override void OnControl()
        {
            MainWindow.Instance.OptionsCommandExecuted += Instance_OptionsCommandExecuted;
        }

        void Instance_OptionsCommandExecuted()
        {
            
        }
    }
}
