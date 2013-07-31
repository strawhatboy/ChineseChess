using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Strawhat.Games._2DModelEditor.Controller
{
    public abstract class BaseController
    {
        public BaseController()
        {
            this.OnControl();
        }

        public abstract void OnControl();
    }
}
