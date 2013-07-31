using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Strawhat.Games.Utilities;
using System.IO;
using System.Collections.Generic;

namespace Strawhat.Games.Test.Strawhat_Games_Utilities
{
    [TestClass]
    public class _GameResourceFile
    {
        string m_ResFileDirectory = @"E:\bbb\";
        string m_ResFileName = "blabla";
        string m_SampleFileToBeCompressed = @"E:\Background.png";
        string m_ExtractDirectory = @"E:\cd";
        FileInfo m_ResFile { set; get; }
        FileInfo m_SampleFile { set; get; }

        [TestInitialize]
        public void PreAction()
        {
            m_ResFile = new FileInfo(string.Format("{0}{1}{2}", m_ResFileDirectory, m_ResFileName, GameResourceFile.EXTENSION));
            m_SampleFile = new FileInfo(m_SampleFileToBeCompressed);
        }

        [TestMethod]
        public void GetTempPath()
        {
            Console.WriteLine(GameResourceFile.GetTempPath());
        }

        [TestMethod]
        public void CreateEmptyResourceFile()
        {
            GameResourceFile.CreateEmptyResourceFile(m_ResFileDirectory, m_ResFileName);
        }

        [TestMethod]
        public void AddFilesToResourceFile()
        {
            GameResourceFile.AddFilesToResourceFile(
                new List<FileInfo>() { new FileInfo(m_SampleFileToBeCompressed) },
                m_ResFile.FullName);
        }

        [TestMethod]
        public void AddFileToResourceFile()
        {
            GameResourceFile.AddFileToResourceFile(m_SampleFile, m_ResFile.FullName, "/xxx");
        }

        [TestMethod]
        public void ExtractResourceFile()
        {
            Console.WriteLine(GameResourceFile.ExtractResourceFile(m_ResFile.FullName, m_ExtractDirectory));
        }
    }
}
