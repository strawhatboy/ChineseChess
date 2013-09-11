using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.IO.Compression;
using System.Diagnostics;

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

        public static List<string> TempPaths = new List<string>();

        public static string CURRENT_DIR = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

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
            return GetTempPath(false);
        }


        /// <summary>
        /// Get a tempory folder. You'd better call "CleanAllTempFiles" to clean all temp folders at the end of the game or 
        /// the resources have been loaded to the memory.
        /// </summary>
        /// <param name="isSystemTemp">Whether in a system temp folder or under the game path.</param>
        /// <returns></returns>
        public static string GetTempPath(bool isSystemTemp)
        {
            string path;
            if (isSystemTemp)
            {
                path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            }
            else
            {
                string currentDir = CURRENT_DIR;
                path = string.Format("{0}{1}{2}{1}{3}", currentDir, Path.DirectorySeparatorChar, StringResources._FOLDERS_TEMP_, Path.GetRandomFileName());
            }

            TempPaths.Add(path);
            return path;
        }

        public static bool CleanAllTempFiles()
        {
            Trace.WriteLine("Removing all temp files...");
            bool result = true;
            while (TempPaths.Count > 0)
            {
                string path = TempPaths.First();
                if (Directory.Exists(path))
                {
                    try
                    {
                        Trace.WriteLine(string.Format("Removing directory {0}", path));
                        Directory.Delete(path, true);
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError("Failed to remove directory {0}, Details: {1}", path, e);
                        if (result)
                            result = false;
                    }
                }

                TempPaths.Remove(path);
            }

            return result;
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
