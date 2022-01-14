using System.Collections.Generic;
using System.IO;

namespace Backups
{
    public interface IRepository
    {
        DirectoryInfo DirectoryInfo { get; }
        string AddFilesToArchive(List<FileInfo> files);
        string CreateZipCopyOfFile(FileInfo file);
    }
}