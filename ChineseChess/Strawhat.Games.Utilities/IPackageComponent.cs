using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Strawhat.Games.Utilities
{
    public interface IPackageComponent
    {
        /// <summary>
        /// Extract Packaged File to a destination folder.
        /// </summary>
        /// <param name="pkgFilePath">Resource file path need to be extracted.</param>
        /// <param name="destinationPath">Destination folder path.</param>
        void ExtractPackage(string pkgFilePath, string destinationPath);

        /// <summary>
        /// Add several files to a package.
        /// </summary>
        /// <param name="files">Files need to be added into the package.</param>
        /// <param name="pkgFilePath">Package file path.</param>
        void AddFilesToPackage(IEnumerable<FileInfo> files, string pkgFilePath);

        /// <summary>
        /// Add a single file to a package.
        /// </summary>
        /// <param name="file">The file need to be added into the package.</param>
        /// <param name="pkgFilePath">Package file path.</param>
        /// <param name="entryPath">Entry path of the file in the package.</param>
        void AddFileToPackage(FileInfo file, string pkgFilePath, string entryPath);

        /// <summary>
        /// Create an empty package file.
        /// </summary>
        /// <param name="pkgFilePath">The empty package file path.</param>
        void CreateEmptyPackage(string pkgFilePath);
    }
}
