using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Strawhat.Games._2DGame
{
    public class _2DGameBootstrapper
    {
        //public Queue<_I2DGameScene> QueuedScene { private set; get; }

        //public void AddScene(_I2DGameScene scene)
        //{
        //    bool bIsOtherSceneShowing = false;
        //    if (this.QueuedScene.Count > 0)
        //        bIsOtherSceneShowing = true;

        //    this.QueuedScene.Enqueue(scene);
        //        // There's other not shown scene
        //        scene.Ended += () =>
        //        {
        //            if (this.QueuedScene.Count > 0)
        //            {
        //                var nextScene = this.QueuedScene.Dequeue();

        //            }
        //        };
        //}

        public _I2DGameScene CurrentScene { private set; get; }
    }
}
