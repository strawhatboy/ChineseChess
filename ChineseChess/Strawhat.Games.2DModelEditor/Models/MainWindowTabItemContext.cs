using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Strawhat.Games._2DGame;

namespace Strawhat.Games._2DModelEditor.Models
{
    public class MainWindowTabItemContext
    {
        public bool IsEditted { set; get; }
        public _2DGameModel GameModel { set; get; }
        public string FilePath { set; get; }
    }
}
