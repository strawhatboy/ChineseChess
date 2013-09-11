using Strawhat.Games.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Strawhat.Games._2DModelEditor.Models
{
    [Singleton]
    [Serializable]
    public class EditorOptions
    {
        private static readonly string _CONFIG_FILE_NAME_ = "Config.conf";
        private static EditorOptions _Instance;
        private static object _obj = new object();
        static GameConfigFile<EditorOptions> optionsFormatter = new GameConfigFile<EditorOptions>();
        static string dir = string.Format("{0}{1}{2}{1}",
                                GameResourceFile.CURRENT_DIR,
                                Path.DirectorySeparatorChar,
                                Strawhat.Games.Utilities.StringResources._FOLDERS_CONFIGS_);
        static string fileName = string.Format("{0}{1}",
                dir,
                _CONFIG_FILE_NAME_);

        public static EditorOptions Instance
        {
            get
            {
                lock (_obj)
                {
                    if (_Instance == null)
                    {
                        //string dir = string.Format("{0}{1}{2}{1}",
                        //        GameResourceFile.CURRENT_DIR,
                        //        Path.DirectorySeparatorChar,
                        //        Strawhat.Games.Utilities.StringResources._FOLDERS_CONFIGS_);
                        //string fileName = string.Format("{0}{1}",
                        //        dir,
                        //        _CONFIG_FILE_NAME_);
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
                                EditorOptions options = new EditorOptions();
                                optionsFormatter.SaveToFile(fileName);
                                _Instance = options;

                                return _Instance;
                            }
                            else
                            {
                                _Instance = optionsFormatter.ReadFromFile(fileName);
                            }
                        }
                        catch (Exception e)
                        {
                            Trace.TraceError("Failed to load EditorOptions from file {0}, Details: {1}", fileName, e.ToString());
                        }
                    }
                    return _Instance;
                }
            }
        }

        public void Save()
        {
            optionsFormatter.ConfigEntity = this;
            optionsFormatter.SaveToFile(fileName);
        }
    }
}
