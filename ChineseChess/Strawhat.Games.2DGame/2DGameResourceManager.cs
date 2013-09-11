using Strawhat.Games.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Timers;
using System.Windows.Media;

namespace Strawhat.Games._2DGame
{
    [Singleton]
    public class _2DGameResourceManager
    {
        private static _2DGameResourceManager _Instance;
        private static object obj = new object();
        public static _2DGameResourceManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (obj)
                    {
                        if (_Instance == null)
                            _Instance = new _2DGameResourceManager();
                    }
                }
                return _Instance;
            }
        }

        private Dictionary<string, _2DGameModel> models = new Dictionary<string, _2DGameModel>();
        private Dictionary<string, MediaPlayer> sounds = new Dictionary<string, MediaPlayer>();

        public void LoadModel(string modelKey, string modelFileName)
        {
            if (!models.ContainsKey(modelKey))
            {
                _2DGameModel model = _2DGameModel.LoadFromFile(string.Format("{0}{1}{2}{1}{3}",
                        GameResourceFile.CURRENT_DIR,
                        System.IO.Path.DirectorySeparatorChar,
                        StringResources._FOLDERS_RESOURCES_MODELS_,
                        modelFileName)
                        );
                models.Add(modelKey, model);
            }
            else
            {
                Trace.WriteLine(string.Format("The model key {0} has been used when adding model {1}.", modelKey, modelFileName));
            }
        }

        public void ReleaseModel(string modelKey)
        {
            if (models.ContainsKey(modelKey))
            {
                models.Remove(modelKey);
            }
            else
            {
                Trace.WriteLine(string.Format("Cannot remove model by key {0}", modelKey));
            }
        }

        public _2DGameModel GetModelByKey(string modelKey)
        {
            if (models.ContainsKey(modelKey))
            {
                return models[modelKey];
            }
            else
            {
                Trace.WriteLine(string.Format("Cannot get model by key {0}", modelKey));
                return null;
            }
        }

        public void PlaySound(string soundKey, double volume, TimeSpan duration = default(TimeSpan))
        {
            if (sounds.ContainsKey(soundKey))
            {
                InitSound(soundKey);
                sounds[soundKey].Volume = volume;
                if (duration != default(TimeSpan) && duration.TotalMilliseconds <= sounds[soundKey].NaturalDuration.TimeSpan.TotalMilliseconds)
                {
                    Timer t = new Timer(duration.TotalMilliseconds);
                    t.Enabled = true;
                    t.Start();
                    t.Elapsed += (sender, e) =>
                    {
                        InitSound(soundKey);
                    };
                }
                sounds[soundKey].Play();
            }
            else
            {
                Trace.WriteLine(string.Format("No such sound {0} to play", soundKey));
            }
        }

        public void StopSound(string soundKey)
        {
            if (sounds.ContainsKey(soundKey))
            {
                InitSound(soundKey);
            }
            else
            {
                Trace.WriteLine(string.Format("No such sound {0} to stop", soundKey));
            }
        }

        public void ReleaseSound(string soundKey)
        {
            if (sounds.ContainsKey(soundKey))
            {
                FieldInfo fi = typeof(MediaPlayer).GetField("MediaEnded");
                object obj = fi.GetValue(sounds[soundKey]);
                PropertyInfo pi = obj.GetType().GetProperty("Events");
                EventHandlerList ehl = (EventHandlerList)pi.GetValue(obj, null);
                ehl.RemoveHandler(obj, ehl[obj]);

                sounds[soundKey].Close();
                sounds.Remove(soundKey);
            }
            else
            {
                Trace.WriteLine(string.Format("No such sound {0} to Release", soundKey));
            }
        }

        public void LoadSound(string soundKey, string soundFileName)
        {
            if (!sounds.ContainsKey(soundKey))
            {
                MediaPlayer p = new MediaPlayer();
                p.Open(new Uri(
                    string.Format("{0}{1}{2}{1}{3}",
                    GameResourceFile.CURRENT_DIR,
                    System.IO.Path.DirectorySeparatorChar,
                    StringResources._FOLDERS_RESOURCES_SOUNDS_,
                    soundFileName)
                    ));
                sounds.Add(soundKey, p);
                InitSound(soundKey);
                sounds[soundKey].MediaEnded += (sender, e) =>
                {
                    InitSound(soundKey);
                };
            }
            else
            {
                Trace.WriteLine(string.Format("The sound key {0} has been used when adding sound {1}.", soundKey, soundFileName));
            }
        }

        private void InitSound(string soundKey)
        {
            if (sounds.ContainsKey(soundKey))
            {
                sounds[soundKey].Position = TimeSpan.FromMilliseconds(0);
                sounds[soundKey].Play();
                sounds[soundKey].Pause();
            }
            else
            {
                Trace.WriteLine(string.Format("No such sound {0} to initialize.", soundKey));
            }
        }
    }
}
