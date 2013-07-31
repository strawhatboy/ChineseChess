using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Strawhat.Games._2DModelEditor.Models
{
    public class MyClipboard<T>
    {
        public T Data
        {
            get;
            set;
        }

        public bool HasData
        {
            get
            {
                return Data != null;
            }
        }
    }

    public class MyClipboard<T1, T2>
    {
        public T1 Data1 { set; get; }
        public T2 Data2 { set; get; }

        public bool HasData
        {
            get
            {
                return Data1 != null && Data2 != null;
            }
        }
    }
}
