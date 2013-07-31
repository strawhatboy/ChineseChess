using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Strawhat.Games.Utilities
{
    public class GameConfigFile
    {
        public object ConfigEntity { set; get; }

        /// <summary>
        /// Save the ConfigEntity to a xml file.
        /// </summary>
        /// <param name="filePath">The destination file path.</param>
        public void SaveToFile(string filePath)
        {
            if (ConfigEntity == null)
            {
                Trace.WriteLine("ConfigEntity is null, will ignore the save operation.");
                //throw new NullReferenceException("The config entity is null.");
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(ConfigEntity.GetType());
                var dirPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                using (var stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    serializer.Serialize(stream, this.ConfigEntity);
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(string.Format("Error occurs when save 'ConfigEntity' to {0}. Inner Exception{1}{2}",
                    filePath, Environment.NewLine, e.ToString()));
                //throw e;
            }
        }

        /// <summary>
        /// Read ConfigEntity from a xml file.
        /// </summary>
        /// <typeparam name="T">Type of ConfigEntity</typeparam>
        /// <param name="filePath">The source file path.</param>
        /// <returns>ConfigEntity</returns>
        public T ReadFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Trace.WriteLine("The file doesn't exist");
                this.ConfigEntity = null;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    this.ConfigEntity = serializer.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(string.Format("Error occurs when read 'ConfigEntity' from file {0}. Inner Exception{1}{2}",
                    filePath, Environment.NewLine, e.ToString()));
                //throw e;
            }
            return (T)this.ConfigEntity;
        }
    }

    public class GameConfigFile<T>
    {
        public T ConfigEntity { set; get; }

        private GameConfigFile configFile = new GameConfigFile();

        public void SaveToFile(string filePath)
        {
            configFile.ConfigEntity = this.ConfigEntity;
            configFile.SaveToFile(filePath);
        }

        public T ReadFromFile(string filePath)
        {
            return configFile.ReadFromFile<T>(filePath);
        }
    }
}
