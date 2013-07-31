using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Strawhat.Games._2DModelEditor.Models
{
    public class SelectedObject
    {
        public object Object { set; get; }
        public IEnumerable<object> Args { set; get; }

        public SelectedObject(object obj, IEnumerable<object> args)
        {
            this.Object = obj;
            this.Args = args;
        }
    }
}
