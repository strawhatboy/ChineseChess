using Strawhat.Games.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Strawhat.Games._2DGame
{
    [Singleton]
    [Serializable]
    [XmlRoot]
    public class _2DGameSettings
    {
        [XmlElement]
        public Vector2D<int> Resolution { set; get; }

        [NonSerialized]
        private static readonly string _CONFIG_FILE_NAME_ = "2DGameConfig.conf";

        [NonSerialized]
        private static _2DGameSettings _Instance;

        [NonSerialized]
        private static object obj;

        private static GameConfigFile<_2DGameSettings> configFile = new GameConfigFile<_2DGameSettings>(false);

        static string dir = string.Format("{0}{1}{2}{1}",
                                GameResourceFile.CURRENT_DIR,
                                Path.DirectorySeparatorChar,
                                Strawhat.Games.Utilities.StringResources._FOLDERS_CONFIGS_);
        static string fileName = string.Format("{0}{1}",
                dir,
                _CONFIG_FILE_NAME_);

        public static _2DGameSettings Instance
        {
            get
            {
                try
                {
                    if (!Directory.Exists(dir) || !File.Exists(fileName))
                    {
                        Trace.WriteLine(string.Format("No directory {0} or config file {1} exist, will initialize one.",
                            dir, _CONFIG_FILE_NAME_));
                        try
                        {
                            Directory.CreateDirectory(dir);
                        }
                        catch { }
                        _2DGameSettings options = new _2DGameSettings();
                        configFile.SaveToFile(fileName);
                        _Instance = options;

                        return _Instance;
                    }
                    else
                    {
                        _Instance = configFile.ReadFromFile(fileName);
                    }
                }
                catch (Exception e)
                {
                    Trace.TraceError("Failed to load EditorOptions from file {0}, Details: {1}", fileName, e.ToString());
                }
                return _Instance;
            }
            set
            {
                _Instance = value;

            }
        }

        private _2DGameSettings()
        {
        }

        public void Save()
        {
            configFile.ConfigEntity = this;
            configFile.SaveToFile(fileName);
        }
    }
}
