using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Strawhat.Games.Utilities
{
    public class GameConfigFile
    {
        private bool _IsBinarySerialization;

        public object ConfigEntity { set; get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isBinarySerialization">Set to false to use XML serialization</param>
        public GameConfigFile(bool isBinarySerialization)
        {
            this._IsBinarySerialization = isBinarySerialization;
        }

        public GameConfigFile() : this(true) { }

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
                var dirPath = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                using (var stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    if (!this._IsBinarySerialization)
                    {
                        // Xml Serialization
                        XmlSerializer serializer = new XmlSerializer(ConfigEntity.GetType());
                        serializer.Serialize(stream, this.ConfigEntity);
                    }
                    else
                    {
                        // Binary Serialization
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(stream, this.ConfigEntity);
                    }
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
                
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    if (!this._IsBinarySerialization)
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        this.ConfigEntity = serializer.Deserialize(stream);
                    }
                    else
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        this.ConfigEntity = formatter.Deserialize(stream);
                    }
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

        private bool _IsBinarySerialization;

        private GameConfigFile configFile;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isBinarySerialization">Set to false to use XML serialization</param>
        public GameConfigFile(bool isBinarySerialization)
        {
            this._IsBinarySerialization = isBinarySerialization;
            this.configFile = new GameConfigFile(this._IsBinarySerialization);
        }

        /// <summary>
        /// Use binary serialization by default. Use another override to use XML serialization.
        /// </summary>
        public GameConfigFile() : this(true) { }

        public void SaveToFile(string filePath)
        {
            configFile.ConfigEntity = this.ConfigEntity;
            configFile.SaveToFile(filePath);
        }

        public T ReadFromFile(string filePath)
        {
            this.ConfigEntity = configFile.ReadFromFile<T>(filePath);
            return this.ConfigEntity;
        }
    }
}
