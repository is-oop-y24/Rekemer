using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Backups
{
    public interface IRepository
    {
        public DirectoryInfo DirectoryInfo { get; }
        string AddFilesToArchive(List<FileInfo> files);
        string CreateZipCopyOfFile(FileInfo file);
    }
}