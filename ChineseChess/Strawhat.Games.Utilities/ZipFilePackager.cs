using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Packaging;

namespace Strawhat.Games.Utilities
{
    [Singleton]
    public class ZipFilePackager : IPackageComponent
    {
        public const char NORMAL_DIRECTORY_SEPERATOR = '\\';
        public const char ZIP_DIRECTORY_SEPERATOR = '/';
        public const char SPACE = ' ';
        public const char UNDERLINE = '_';

        private const string METADATA_FILE_EXTENSION = ".psmdcp";
        private const string RELS_FILE_EXTENSION = ".rels";

        private static ZipFilePackager _Instance;
        private static object objLocker = new object();
        private ZipFilePackager()
        {
        }

        public static ZipFilePackager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (objLocker)
                    {
                        _Instance = new ZipFilePackager();
                    }
                }
                return _Instance;
            }
        }

        public void ExtractPackage(string pkgFilePath, string destinationPath)
        {
            //throw new NotImplementedException();
            using (var zip = ZipPackage.Open(pkgFilePath, FileMode.Open))
            {
                foreach (var pkgPart in zip.GetParts())
                {
                    // Get the path without '/'
                    var fileName = pkgPart.Uri.OriginalString.Substring(1);

                    if (IsZipPackageReservedFile(fileName))
                        continue;

                    var stream = pkgPart.GetStream();
                    var bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);

                    fileName = fileName.Replace(UNDERLINE, SPACE);
                    var filePath = Path.Combine(destinationPath, fileName);
                    var directoryName = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directoryName))
                        Directory.CreateDirectory(directoryName);

                    File.WriteAllBytes(filePath, bytes);
                }
            }
        }

        private bool IsZipPackageReservedFile(string fileName)
        {
            if (fileName.EndsWith(METADATA_FILE_EXTENSION, StringComparison.CurrentCultureIgnoreCase))
                return true;

            if (fileName.EndsWith(RELS_FILE_EXTENSION, StringComparison.CurrentCultureIgnoreCase))
                return true;

            return false;
        }

        public void AddFilesToPackage(IEnumerable<System.IO.FileInfo> files, string pkgFilePath)
        {
            //throw new NotImplementedException();
            using (var zip = ZipPackage.Open(pkgFilePath, FileMode.Open))
            {
                foreach (var file in files)
                {
                    AddFileToArchive(file, zip);
                }
            }
        }

        public void AddFileToPackage(System.IO.FileInfo file, string pkgFilePath, string entryPath = "")
        {
            //throw new NotImplementedException();
            using (var zip = ZipPackage.Open(pkgFilePath, FileMode.Open))
            {
                AddFileToArchive(file, zip, entryPath);
            }
        }

        private void AddFileToArchive(System.IO.FileInfo file, Package archive, string entryPath = "")
        {
            if (!string.IsNullOrEmpty(entryPath))
            {
                if (!(entryPath[0] == ZIP_DIRECTORY_SEPERATOR))
                    entryPath = string.Concat(ZIP_DIRECTORY_SEPERATOR, entryPath);

                if (!(entryPath[entryPath.Length - 1] == ZIP_DIRECTORY_SEPERATOR))
                    entryPath = string.Concat(entryPath, ZIP_DIRECTORY_SEPERATOR);

                entryPath = string.Concat(entryPath, file.Name);
            }
            else
            {
                entryPath = string.Concat(ZIP_DIRECTORY_SEPERATOR, file.Name);
            }
            entryPath = entryPath.Replace(SPACE, UNDERLINE);

            var partUri = new Uri(entryPath, UriKind.Relative);
            var contentType = System.Net.Mime.MediaTypeNames.Application.Zip;

            var pkgPart = archive.CreatePart(partUri, contentType, CompressionOption.Normal);
            var bytes = File.ReadAllBytes(file.FullName);

            pkgPart.GetStream().Write(bytes, 0, bytes.Length);
        }

        public void CreateEmptyPackage(string pkgFilePath)
        {
            if (File.Exists(pkgFilePath))
                File.Delete(pkgFilePath);

            using (var zip = ZipPackage.Open(pkgFilePath, FileMode.CreateNew))
            {
                //Nothing to do.
            }
        }
    }
}
