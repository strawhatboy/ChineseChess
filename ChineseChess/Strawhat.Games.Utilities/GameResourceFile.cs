using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.IO.Compression;

namespace Strawhat.Games.Utilities
{
    public class GameResourceFile
    {
#if DEBUG
        public static string EXTENSION = ".SGR";
        public static Encoding TEXT_FILE_ENCODING = Encoding.UTF8;
#else
        public const string EXTENSION = ".SGR";
        public const Encoding TEXT_FILE_ENCODING = Encoding.UTF8;
#endif

        public static IPackageComponent ResFilePackager = ZipFilePackager.Instance;


        /// <summary>
        /// Extract Resource File to a destination folder.
        /// </summary>
        /// <param name="resFilePath">Resource file path need to be extracted.</param>
        /// <param name="destinationPath">Destination folder path.</param>
        /// <returns>Destination folder path.</returns>
        public static string ExtractResourceFile(string resFilePath, string destinationPath)
        {
            DirectoryInfo directory = null;
            if (Directory.Exists(destinationPath))
            {
                directory = new DirectoryInfo(destinationPath);
            }
            else
            {
                directory = Directory.CreateDirectory(destinationPath);
            }

            ResFilePackager.ExtractPackage(resFilePath, destinationPath);

            //string destinationFileName = string.Empty;
            //string destinationDirectory = string.Empty;

            //using (ZipArchive archive = ZipFile.OpenRead(resFilePath))
            //{
            //    foreach (var entry in archive.Entries)
            //    {
            //        destinationFileName = Path.Combine(directory.FullName, entry.FullName);
            //        destinationDirectory = Path.GetDirectoryName(destinationFileName);
            //        if (!Directory.Exists(destinationDirectory))
            //        {
            //            Directory.CreateDirectory(destinationDirectory);
            //        }
            //        entry.ExtractToFile(destinationFileName, true);
            //    }
            //    //archive.ExtractToDirectory(destinationPath);
            //}
            return destinationPath;
        }

        /// <summary>
        /// Extract Resource File to a temp folder.
        /// </summary>
        /// <param name="resFilePath">Resource file path need to be extracted.</param>
        /// <returns>Temp folder path.</returns>
        public static string ExtractResourceFile(string resFilePath)
        {
            return ExtractResourceFile(resFilePath, GetTempPath());
        }

        public static string GetTempPath()
        {
            return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        }

        public static void AddFilesToResourceFile(IEnumerable<FileInfo> files, string resFilePath)
        {
            //using (ZipArchive archive = ZipFile.Open(resFilePath, ZipArchiveMode.Update))
            //{
            //    foreach (var file in files)
            //    {
            //        archive.CreateEntryFromFile(file.FullName, file.Name);
            //    }
            //}
            ResFilePackager.AddFilesToPackage(files, resFilePath);
        }

        /// <summary>
        /// Add a file to resource file
        /// </summary>
        /// <param name="file">The fileinfo need to be added.</param>
        /// <param name="resFilePath">Resource file path</param>
        /// <param name="entryPath">Entry path of the file added in the resource file.</param>
        public static void AddFileToResourceFile(FileInfo file, string resFilePath, string entryPath)
        {
            //using (ZipArchive archive = ZipFile.Open(resFilePath, ZipArchiveMode.Update))
            //{
            //    archive.CreateEntryFromFile(file.FullName, Path.Combine(entryPath, file.Name));
            //}
            ResFilePackager.AddFileToPackage(file, resFilePath, entryPath);
        }

        /// <summary>
        /// Create an empty resource file.
        /// </summary>
        /// <param name="resFileDirectory">The directory of the resource file located.</param>
        /// <param name="resFileName">The resrouce file name.</param>
        public static void CreateEmptyResourceFile(string resFileDirectory, string resFileName)
        {
            if (!Directory.Exists(resFileDirectory))
                Directory.CreateDirectory(resFileDirectory);
            DirectoryInfo directory = new DirectoryInfo(resFileDirectory);
            //if (!resFileName.EndsWith(EXTENSION))
            //    resFileName += EXTENSION;
            var resFileFullName = Path.Combine(directory.FullName, resFileName);
            ResFilePackager.CreateEmptyPackage(resFileFullName);
        }

        public static void CreateEmptyResourceFile(string resFilePath)
        {
            var dirPath = Path.GetDirectoryName(resFilePath);
            var fileName = Path.GetFileName(resFilePath);
            CreateEmptyResourceFile(dirPath, fileName);
        }
    }
}
