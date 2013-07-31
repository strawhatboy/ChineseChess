using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using Strawhat.Games;
using System.Windows.Media.Imaging;
using System.IO;
using Strawhat.Games.Utilities;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace Strawhat.Games._2DGame
{
    public enum _2DGameModelType
    {
        Normal,
        SingleDirection,
        HorizontalMirrored,
    }


    [XmlRoot]
    public class _2DGameModel : GameObject
    {
#if DEBUG
        public static readonly string EXTENSION = ".SGR_2DM";
        public static readonly string PNG_FILE_FILTER = "*.PNG";
        public static readonly string META_DATA_FILENAME = "METADATA.XML";
        public static readonly string META_DATA_NAME = "Name";
        public static readonly string META_DATA = "MetaData";
        public static readonly string META_DATA_DESCRIPTION = "Description";
        public static readonly string META_DATA_MODEL_TYPE = "ModelType";
        public static readonly char PNG_FILE_SEPERATOR = '_';
        public static readonly char PNG_FILE_SEPERATOR_EXTRACTED = ' ';
#else
        public const string EXTENSION = ".SGR_2DM";
        public const string PNG_FILE_FILTER = "*.PNG";
        public const string META_DATA_FILENAME = "METADATA.XML";
        public const string META_DATA_NAME = "Name";
        public const string META_DATA = "MetaData";
        public const string META_DATA_DESCRIPTION = "Description";
        public const string META_DATA_IS_SINGLE_DIRECT_MODEL = "IsSingleDirectModel";
        public const char PNG_FILE_SEPERATOR = '_';
        public const char PNG_FILE_SEPERATOR_EXTRACTED = ' ';
#endif

        /// <summary>
        /// Represent that this model is single direction or horizontal mirrored or just with all available angles.
        /// </summary>
        [XmlElement]
        public _2DGameModelType ModelType { set; get; }

        /// <summary>
        /// Angle, Animations with different names
        /// </summary>
        [XmlIgnore]
        public Dictionary<double, ObservableCollection<_2DGameModelAnimation>> Details { private set; get; }

        /// <summary>
        /// fileName, Interval
        /// Represent for the animation's interval by its file name.
        /// </summary>
        [XmlIgnore]
        private Dictionary<string, double> Intervals { set; get; }

        [XmlElement]
        public List<string> Intervals_Keys { set; get; }
        [XmlElement]
        public List<double> Intervals_Values { set; get; }

        /// <summary>
        /// fileName, attachment points
        /// Represent for each frames' attachement points by its file name and index.
        /// </summary>
        [XmlIgnore]
        private Dictionary<string, ObservableCollection<_2DGameModelAttachmentPoint>> AttachmentPoints { set; get; }

        [XmlElement]
        public List<string> AttachmentPoints_Keys { set; get; }
        [XmlElement]
        public List<int> AttachmentPoints_EveryKeyValuesCount { set; get; }
        [XmlElement]
        public List<_2DGameModelAttachmentPoint> AttachmentPoints_Values { set; get; }

        [XmlElement]
        public List<string> OccupiedFramesCountOfFrames_Keys { set; get; }
        [XmlElement]
        public List<int> OccupiedFramesCountOfFrames_Values { set; get; }

        [XmlIgnore]
        public Dictionary<string, int> OccupiedFramesCountOfFrames { set; get; }

        /// <summary>
        /// Metadata file utility.
        /// </summary>
        private static GameConfigFile<_2DGameModel> config = new GameConfigFile<_2DGameModel>();

        public _2DGameModel()
        {
            //IsSingleDirectModel = true;
            Details = new Dictionary<double, ObservableCollection<_2DGameModelAnimation>>();
            Intervals = new Dictionary<string, double>();
            AttachmentPoints = new Dictionary<string, ObservableCollection<_2DGameModelAttachmentPoint>>();
            OccupiedFramesCountOfFrames = new Dictionary<string, int>();

            ////A model should have at least one zero direction animation.
            //Details.Add(0, new List<_2DGameObjectAnimation>());
            Intervals_Keys = new List<string>();
            Intervals_Values = new List<double>();
            AttachmentPoints_Keys = new List<string>();
            AttachmentPoints_EveryKeyValuesCount = new List<int>();
            AttachmentPoints_Values = new List<_2DGameModelAttachmentPoint>();

            OccupiedFramesCountOfFrames_Keys = new List<string>();
            OccupiedFramesCountOfFrames_Values = new List<int>();
        }

        public void SaveToFile(string modelFilePath)
        {
            //Clear elements in "Intervals" and "AttachmentPoints" for reconstruct them.
            this.Intervals.Clear();
            this.AttachmentPoints.Clear();
            this.OccupiedFramesCountOfFrames.Clear();

            this.Intervals_Keys.Clear();
            this.Intervals_Values.Clear();
            this.AttachmentPoints_Keys.Clear();
            this.AttachmentPoints_EveryKeyValuesCount.Clear();
            this.AttachmentPoints_Values.Clear();

            this.OccupiedFramesCountOfFrames_Keys.Clear();
            this.OccupiedFramesCountOfFrames_Values.Clear();

            string tmpDir = GameResourceFile.GetTempPath();
            List<FileInfo> generatedFiles = new List<FileInfo>();
            List<FileInfo> generatedPictureFiles = new List<FileInfo>();
            foreach (var key in Details.Keys)
            {
                var animations = Details[key];
                int animationCount = animations.Count;
                for (int i = 0; i < animationCount; i++)
                {
                    var animation = animations[i];

                    int frameCount = animation.Frames.Count;
                    for (int j = 0; j < frameCount; j++)
                    {
                        string fileName = string.Format("{0}_{1}_{2}.PNG", key.ToString(), animation.Name, j.ToString());

                        PngBitmapEncoder encoder = new PngBitmapEncoder();
                        var frame = animation.Frames[j];
                        encoder.Frames.Add(BitmapFrame.Create(frame.Image));

                        //Fill the attachment points
                        this.AttachmentPoints.Add(
                            string.Format("{0}_{1}", fileName, j.ToString()), frame.AttachmentPoints);

                        //File the occupied frames count
                        this.OccupiedFramesCountOfFrames.Add(fileName, frame.OccupiedFrameCount);

                        string filePath = Path.Combine(tmpDir, fileName);

                        var directory = Path.GetDirectoryName(filePath);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        using (var stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            encoder.Save(stream);
                        }
                        generatedPictureFiles.Add(new FileInfo(filePath));
                    }

                    //Fill the intervals
                    var intervalKey = string.Format("{0}_{1}", key.ToString(), animation.Name);
                    this.Intervals.Add(intervalKey, animation.Interval);
                }
            }

            foreach (var key in this.Intervals.Keys)
            {
                this.Intervals_Keys.Add(key);
                this.Intervals_Values.Add(this.Intervals[key]);
            }

            foreach (var key in this.AttachmentPoints.Keys)
            {
                this.AttachmentPoints_Keys.Add(key);
                this.AttachmentPoints_EveryKeyValuesCount.Add(this.AttachmentPoints[key].Count);
                this.AttachmentPoints_Values.AddRange(this.AttachmentPoints[key]);
            }

            foreach (var key in this.OccupiedFramesCountOfFrames.Keys)
            {
                this.OccupiedFramesCountOfFrames_Keys.Add(key);
                this.OccupiedFramesCountOfFrames_Values.Add(this.OccupiedFramesCountOfFrames[key]);
            }


            string metaDataFileName = Path.Combine(tmpDir, META_DATA_FILENAME);
            //File.WriteAllText(metaDataFileName, GetMetaDataXml(), GameResourceFile.TEXT_FILE_ENCODING);
            config.ConfigEntity = this;
            config.SaveToFile(metaDataFileName);

            generatedFiles.Add(new FileInfo(metaDataFileName));
            generatedFiles.AddRange(generatedPictureFiles);

            GameResourceFile.CreateEmptyResourceFile(modelFilePath);
            GameResourceFile.AddFilesToResourceFile(generatedFiles, modelFilePath);
        }

        public string GetMetaDataXml()
        {
            XElement root = new XElement(META_DATA);
            root.Add(new XElement(META_DATA_NAME, this.Name));
            root.Add(new XElement(META_DATA_DESCRIPTION, this.Description));
            root.Add(new XElement(META_DATA_MODEL_TYPE, this.ModelType));
            return root.ToString(SaveOptions.None);
        }

        public static _2DGameModel LoadFromFile(string filePath)
        {
            _2DGameModel result = new _2DGameModel();
            string tempDirectoryPath = GameResourceFile.ExtractResourceFile(filePath);
            DirectoryInfo tempDirectory = new DirectoryInfo(tempDirectoryPath);
            FileInfo metaDataFile = tempDirectory.GetFiles(META_DATA_FILENAME, SearchOption.AllDirectories).FirstOrDefault();
            if (metaDataFile == null)
                throw new FileNotFoundException("Meta data not found in the model file.");

            var modelMirror = config.ReadFromFile(metaDataFile.FullName);
            modelMirror.Intervals = new Dictionary<string, double>();
            for (int i = 0; i < modelMirror.Intervals_Keys.Count; i++)
            {
                modelMirror.Intervals.Add(modelMirror.Intervals_Keys[i], modelMirror.Intervals_Values[i]);
            }
            modelMirror.AttachmentPoints = new Dictionary<string, ObservableCollection<_2DGameModelAttachmentPoint>>();

            int cursorAttachmentPoint = 0;

            for (int i = 0; i < modelMirror.AttachmentPoints_Keys.Count; i++)
            {
                List<_2DGameModelAttachmentPoint> attachmentPoints = new List<_2DGameModelAttachmentPoint>();
                for (int j = 0; j < modelMirror.AttachmentPoints_EveryKeyValuesCount[i]; j++)
                {
                    attachmentPoints.Add(modelMirror.AttachmentPoints_Values[cursorAttachmentPoint++]);
                }
                modelMirror.AttachmentPoints.Add(modelMirror.AttachmentPoints_Keys[i], new ObservableCollection<_2DGameModelAttachmentPoint>(attachmentPoints));
            }

            for (int i = 0; i < modelMirror.OccupiedFramesCountOfFrames_Keys.Count; i++)
            {
                modelMirror.OccupiedFramesCountOfFrames.Add(modelMirror.OccupiedFramesCountOfFrames_Keys[i], modelMirror.OccupiedFramesCountOfFrames_Values[i]);
            }
            result.Name = modelMirror.Name;
            result.Description = modelMirror.Description;
            result.AttachmentPoints = modelMirror.AttachmentPoints;
            result.Intervals = modelMirror.Intervals;
            result.ModelType = modelMirror.ModelType;
            result.OccupiedFramesCountOfFrames = modelMirror.OccupiedFramesCountOfFrames;

            FileInfo[] pngFiles = tempDirectory.GetFiles(PNG_FILE_FILTER, SearchOption.AllDirectories);
            Dictionary<double, ObservableCollection<_2DGameModelAnimation>> details = new Dictionary<double, ObservableCollection<_2DGameModelAnimation>>();

            #region xxx
            //foreach (var pngFile in pngFiles)
            //{
            //    string fileName = pngFile.Name;
            //    int seperatorIndex = fileName.IndexOf(PNG_FILE_SEPERATOR_EXTRACTED);
            //    string anglePart = pngFile.Name.Substring(0, seperatorIndex);
            //    double angle;
            //    if (!double.TryParse(anglePart, out angle))
            //    {
            //        Trace.WriteLine(string.Format("Name of PNG file '{0}' is not valid, will ignore it.",
            //            fileName));
            //        continue;
            //    }

            //    string leftPart = pngFile.Name.Substring(seperatorIndex + 1);
            //    seperatorIndex = leftPart.IndexOf(PNG_FILE_SEPERATOR_EXTRACTED);
            //    string animationNamePart = leftPart.Substring(0, seperatorIndex);
            //    var framePart = leftPart.Substring(seperatorIndex + 1).Remove(leftPart.ToUpper().IndexOf(".PNG"));

            //    ObservableCollection<_2DGameModelFrame> frames = new ObservableCollection<_2DGameModelFrame>();
            //    using (var stream = pngFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        var decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.Default);
            //        int frameCount = decoder.Frames.Count;
            //        for (int i = 0; i < frameCount; i++)
            //        {
            //            var frame = decoder.Frames[i];
            //            _2DGameModelFrame newFrame = new _2DGameModelFrame();
            //            newFrame.Image = BitmapFrame2BitmapImage(frame);
            //            string attachmentPointKey = string.Format("{0}_{1}", pngFile.Name.Replace(PNG_FILE_SEPERATOR_EXTRACTED, PNG_FILE_SEPERATOR), i.ToString());
            //            newFrame.AttachmentPoints = result.AttachmentPoints[attachmentPointKey];
            //            frames.Add(newFrame);
            //        }
            //    }

            //    if (!details.ContainsKey(angle))
            //    {
            //        details.Add(angle, new ObservableCollection<_2DGameModelAnimation>());
            //    }

            //    _2DGameModelAnimation animation = new _2DGameModelAnimation();
            //    animation.Name = animationNamePart;
            //    animation.Frames = frames;
            //    details[angle].Add(animation);
            //}
            #endregion

            foreach (var prefix in result.Intervals.Keys)
            {
                int seperatorIndex = prefix.IndexOf(PNG_FILE_SEPERATOR);
                string anglePart = prefix.Substring(0, seperatorIndex);
                double angle;
                if (!double.TryParse(anglePart, out angle))
                {
                    Trace.WriteLine(string.Format("Name of prefix '{0}' is not valid, will ignore it.",
                        prefix));
                    continue;
                }

                string animationPart = prefix.Substring(seperatorIndex + 1);

                if (!details.ContainsKey(angle))
                {
                    details.Add(angle, new ObservableCollection<_2DGameModelAnimation>());
                }

                var animation = new _2DGameModelAnimation();
                animation.Name = animationPart;
                animation.Interval = result.Intervals[prefix];

                var _prefix = prefix.Replace(PNG_FILE_SEPERATOR, PNG_FILE_SEPERATOR_EXTRACTED);
                var framePngFiles = pngFiles.Where(a => a.Name.StartsWith(_prefix));
                int counter = 0;
                int length = framePngFiles.Count();
                while (counter < length)
                {
                    string fileName = string.Format("{0} {1}.PNG", _prefix, counter);
                    var pngfile = framePngFiles.FirstOrDefault(a => a.Name == fileName);
                    if (pngfile != null)
                    {
                        _2DGameModelFrame frame = new _2DGameModelFrame();

                        //Add attachmentPoints first
                        var orginalFileName = fileName.Replace(PNG_FILE_SEPERATOR_EXTRACTED, PNG_FILE_SEPERATOR);
                        var attachmentPointsKey = string.Format("{0}_{1}", orginalFileName, counter);
                        frame.AttachmentPoints = result.AttachmentPoints[attachmentPointsKey];
                        frame.Image = _2DGameTextureHelper.LoadBitmapImageToMemory(pngfile.FullName);

                        if (result.OccupiedFramesCountOfFrames.ContainsKey(orginalFileName))
                        {
                            frame.OccupiedFrameCount = result.OccupiedFramesCountOfFrames[orginalFileName];
                        }
                        animation.Frames.Add(frame);
                    }
                    counter++;
                }

                details[angle].Add(animation);
            }

            result.Details = details;

            return result;
        }

        private static BitmapImage BitmapFrame2BitmapImage(BitmapFrame frame)
        {
            BitmapImage image = new BitmapImage();

            using (var stream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(frame);
                encoder.Save(stream);
                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();
            }

            return image;
        }

        public IEnumerable<_2DGameModelAnimation> GetAnimationsByAngle(double angle)
        {
            while (angle < 0)
            {
                angle = angle + 360;
            }

            angle = angle % 360;
            var actualAngle = 0d;
            switch (this.ModelType)
            {
                case _2DGameModelType.Normal:
                    {
                        actualAngle = GetNearestAngle(angle, this.Details.Keys);
                    }
                    break;
                case _2DGameModelType.HorizontalMirrored:
                    {
                        actualAngle = GetHorizontalMirroredAngle(angle);
                        actualAngle = GetNearestAngle(angle, this.Details.Keys);
                    }
                    break;
                case _2DGameModelType.SingleDirection:
                default:
                    break;
            }
            return this.Details[actualAngle];
        }

        private double GetNearestAngle(double angle, IEnumerable<double> angles)
        {
            double neareastAngle = 0d;
            double alpha = 512d;
            foreach (var a in angles)
            {
                var _alpha = Math.Abs(angle - a);
                if (_alpha < alpha)
                {
                    neareastAngle = a;
                    alpha = _alpha;
                }
            }
            return neareastAngle;
        }

        private double GetHorizontalMirroredAngle(double angle)
        {
            if (angle > 90d && angle <= 180d)
            {
                return 180d - angle;
            }

            if (angle > 180d && angle <= 270)
            {
                return 540d - angle;
            }

            return angle;
        }
    	
//		public System.Xml.Schema.XmlSchema GetSchema()
//		{
//			return null;
//		}
//    	
//		public void ReadXml(XmlReader reader)
//		{
//			// TODO:
//			
//		}
//    	
//		public void WriteXml(XmlWriter writer)
//		{
//			// TODO:
//		}
    }
}
