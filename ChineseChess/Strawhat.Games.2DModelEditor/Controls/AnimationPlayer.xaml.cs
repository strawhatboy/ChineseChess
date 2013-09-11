using Strawhat.Games._2DGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Strawhat.Games._2DModelEditor.Controls
{
    /// <summary>
    /// Interaction logic for AnimationPlayer.xaml
    /// </summary>
    public partial class AnimationPlayer : UserControl, INotifyPropertyChanged
    {
        private _2DSprite _GameSpirit;
        public _2DSprite GameSpirit
        {
            private set
            {
                if (_GameSpirit != value)
                {
                    _GameSpirit = value;
                    this.OnPropertyChanged("GameSpirit");
                    this.SetSpirit(value);
                }
                //ChangeSourceAnimation(value);
            }
            get
            {
                return _GameSpirit;
            }
        }

        private double _Angle;
        public double Angle
        {
            set
            {
                if (_Angle != value)
                {
                    _Angle = value;
                    this.OnPropertyChanged("Angle");
                    this.SetAngle(value);
                }
            }
            get
            {
                return _Angle;
            }
        }

        private void SetAngle(double value)
        {
            if (this.GameSpirit != null)
            {
                this.Animations = new ObservableCollection<string>((from p in
                    this.GameSpirit.GameObject.GameModel.GetAnimationsByAngle(value)
                                   select p.Name).Distinct());
            }
        }

        private bool _IsPaused;
        public bool IsPaused
        {
            set
            {
                if (_IsPaused != value)
                {
                    _IsPaused = value;
                    this.OnPropertyChanged("IsPaused");
                }
            }
            get
            {
                return _IsPaused;
            }
        }

        private ObservableCollection<string> _Animations;
        public ObservableCollection<string> Animations
        {
            set
            {
                if (_Animations != value)
                {
                    _Animations = value;
                    this.OnPropertyChanged("Animations");
                }
            }
            get
            {
                return _Animations;
            }
        }

        public bool IsRunning
        {
            get
            {
                return !IsPaused;
            }
        }

        private string _CurrentAnimation;
        public string CurrentAnimation
        {
            set
            {
                if (_CurrentAnimation != value)
                {
                    _CurrentAnimation = value;
                    this.OnPropertyChanged("CurrentAnimation");
                    this.SetCurrentAnimation(value);
                }
            }
            get 
            {
                return _CurrentAnimation;
            }
        }

        private double _Speed;
        public double Speed
        {
            set
            {
                if (_Speed != value)
                {
                    _Speed = value;
                    this.GameSpirit.GameObject.AnimationSpeed = _Speed;
                    this.OnPropertyChanged("Speed");
                }
            }
            get
            {
                return _Speed;
            }
        }
        
        private int _MaxFrameCount;
        public int MaxFrameCount
        {
        	set{
        		if (_MaxFrameCount != value)
        		{
        			_MaxFrameCount = value;
        			this.OnPropertyChanged("MaxFrameCount");
        		}
        	}
        	get
        	{
        		return _MaxFrameCount;	
        	}
        }
        
        private int _CurrentFrameIndex;
        public int CurrentFrameIndex
        {
        	set
        	{
        		if (_CurrentFrameIndex != value)
        		{
        			_CurrentFrameIndex = value;
        			this.OnPropertyChanged("CurrentFrameIndex");
        		}
        	}
        	get
        	{
        		return _CurrentFrameIndex;
        	}
        }

        private void SetCurrentAnimation(string value)
        {
            //throw new NotImplementedException();
            this.GameSpirit.GameObject.PlayAnimation(value);
            this.MaxFrameCount = this.GameSpirit.GameObject.CurrentFramesCount;
            this.IsPaused = this.GameSpirit.GameObject.IsAnimationPaused;
        }

        private void SetSpirit(_2DSprite value)
        {
            //throw new NotImplementedException();
            this.GameCanvas.Children.Add(value);
            this.Angle = 0d;
        }

        private bool _IsPeriodic;
        public bool IsPeriodic
        {
            set
            {
                _IsPeriodic = IsPeriodic;
            }
            get
            {
                return _IsPeriodic;
            }
        }

        public AnimationPlayer()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Loaded += AnimationPlayer_Loaded;
        }

        void AnimationPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.GameSpirit != null)
            {
                this.Angle = 1d;
                this.Angle = 0d;
                this.CurrentAnimation = _2DGameModelAnimation.STAND_BY;
            }
        }

        public AnimationPlayer(_2DSprite spirit) : this()
        {
            this.GameSpirit = spirit;
        }

        public AnimationPlayer(_2DGameObject _2dGameObject)
        {
            _2DSprite spirit = new _2DSprite();
            spirit.GameObject = _2dGameObject;
            this.GameSpirit = spirit;
        }

        public AnimationPlayer(_2DGameModel model) : this()
        {
            _2DSprite spirit = new _2DSprite();
            spirit.GameObject = new _2DGameObject(model);
            spirit.GameObject.FrameIndexChanged+= 
            	a => {
            	this.CurrentFrameIndex = a;
            };
            this.GameSpirit = spirit;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
