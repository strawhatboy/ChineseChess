using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Strawhat.Games;

namespace Strawhat.Games._2DGame
{
    public class _2DGameObject : GameObject, IDisposable
    {
        private Vector2D _Location;
        public Vector2D Location
        {
            get { return _Location; }
            set { if (_Location != value) { _Location = value; this.OnPropertyChanged("Location"); this.OnLocationChanged(value); } }
        }
        private BitmapImage _Texture;
        public BitmapImage Texture
        {
            get { return _Texture; }
            set { if (_Texture != value) { _Texture = value; this.OnPropertyChanged("Texture"); this.OnTextureChanged(value); } }
        }
        
        public int CurrentFramesCount
        {
        	get {return this.currentAnimation != null ? this.currentAnimation.Frames.Count : 0;}
        }
        
        public event Action<BitmapImage> TextureChanged;
        public event Action<int> FrameIndexChanged;
        public event Action<Vector2D> LocationChanged;
        public _2DGameModel GameModel { set; get; }
        public double AnimationSpeed { set; get; }

        public double FacingAngle { set; get; }

        private int currentAnimationIndex = 0;
        private int currentTicks = 0;
        private _2DGameModelAnimation currentAnimation;
        private string currentAnimationName;
        public bool IsAnimationPeriodic { set; get; }
        public bool IsAnimationPaused { set; get; }

        public _2DGameObject()
        {
            AnimationSpeed = 1.0;
            Location = new Vector2D(0, 0);
            //_2DGameTimer.Timer.Tick += timer_Tick;
            _2DGameTimer.Tick += _2DGameTimer_Tick;
        }

        void _2DGameTimer_Tick(int interval)
        {
            if (this.IsAnimationPaused)
                return;

            if (this.currentAnimation == null)
                return;

            //var timer = sender as DispatcherTimer;
            var framesCount = this.currentAnimation.Frames.Count;
            if (framesCount == 0)
            {
                Trace.TraceWarning("Empty frames in this animation {0} for object {1}",
                    this.currentAnimation.Name,
                    this.Name);
                this.PlayAnimation(_2DGameModelAnimation.STAND_BY);
                return;
            }

            this.currentTicks += interval;
            var frame = this.currentAnimation.Frames[this.currentAnimationIndex];
            var changePointCount = frame.OccupiedFrameCount * this.currentAnimation.Interval / this.AnimationSpeed;
            if (this.currentTicks >= changePointCount)
            {
                this.Texture = frame.Image;
                this.currentAnimationIndex++;
                OnFrameIndexChanged(this.currentAnimationIndex);
                this.currentTicks = 0;
            }

            if (this.currentAnimationIndex == framesCount)
            {
                if (this.IsAnimationPeriodic)
                {
                    this.currentAnimationIndex = 0;
                }
                else
                {
                    this.PlayAnimation(_2DGameModelAnimation.STAND_BY);
                }
            }
        }

        public _2DGameObject(_2DGameModel gameModel) : this()
        {
            this.GameModel = gameModel;
        }

        public virtual void PlayAnimation(string animation)
        {
            if (this.GameModel != null)
            {
                currentAnimation = this.GameModel.GetAnimationsByAngle(this.FacingAngle).FirstOrDefault(a => string.Compare(a.Name, animation, true) == 0);
                this.currentAnimationIndex = 0;
                this.currentTicks = 0;
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (this.IsAnimationPaused)
                return;

            if (this.currentAnimation == null)
                return;

            //var timer = sender as DispatcherTimer;
            var framesCount = this.currentAnimation.Frames.Count;
            if (framesCount == 0)
            {
                Trace.TraceWarning("Empty frames in this animation {0} for object {1}",
                    this.currentAnimation.Name,
                    this.Name);
                this.PlayAnimation(_2DGameModelAnimation.STAND_BY);
                return;
            }

            this.currentTicks++;
            var frame = this.currentAnimation.Frames[this.currentAnimationIndex];
            var changePointCount = frame.OccupiedFrameCount * this.currentAnimation.Interval / this.AnimationSpeed;
            if (this.currentTicks >= changePointCount)
            {
                this.Texture = frame.Image;
                this.currentAnimationIndex++;
                OnFrameIndexChanged(this.currentAnimationIndex);
                this.currentTicks = 0;
            }

            if (this.currentAnimationIndex == framesCount)
            {
                if (this.IsAnimationPeriodic)
                {
                    this.currentAnimationIndex = 0;
                }
                else
                {
                    this.PlayAnimation(_2DGameModelAnimation.STAND_BY);
                }
            }
        }

        //public void PlayAnimation(string animationName)
        //{
        //    _2DGameModelAnimation animation = this.Animations.FirstOrDefault(
        //        a => string.Compare(a.Name, animationName, true) >= 0);
        //    if (animation == null)
        //        Trace.TraceWarning("Object {0} has no animation {1}", this.Name, animationName);

        //    PlayAnimation(animation);
        //}
        public void OnFrameIndexChanged(int newIndex)
        {
            //this.Texture = newTexture;
            if (this.FrameIndexChanged != null)
                this.FrameIndexChanged(newIndex);
        }

        public void OnTextureChanged(BitmapImage newTexture)
        {
            //this.Texture = newTexture;
            if (this.TextureChanged != null)
                this.TextureChanged(newTexture);
        }

        public void OnLocationChanged(Vector2D location)
        {
            if (this.LocationChanged != null)
                this.LocationChanged(location);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
            //_2DGameTimer.Timer.Tick -= timer_Tick;
            _2DGameTimer.Tick -= _2DGameTimer_Tick;
        }
    }
}
