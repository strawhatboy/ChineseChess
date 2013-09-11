using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Strawhat.Games._2DGame
{
    public interface _I2DGameScene
    {
        bool CanBeSkipped { get; }

        /// <summary>
        /// Useless if 'bCanBeSkipped' is false.
        /// </summary>
        Key SkipKey { get; }

        /// <summary>
        /// Useless if 'bCanBeSkipped' is false. Represent the Xbox360 controller button.
        /// </summary>
        short SkipButton { get; }

        event Action BeginEnd;
        event Action Ended;
        event Action BeginStart;
        event Action Started;
    }


    
}
