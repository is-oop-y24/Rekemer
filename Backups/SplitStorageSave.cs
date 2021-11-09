using System.Collections.Generic;
using System.IO;

namespace Backups
{
    public class SplitStorageSave : IAlgorithm
    {
        public List<FileInfo> Operation(List<FileInfo> files, IRepository repository)
        {
            List<FileInfo> names = new List<FileInfo>();
            foreach (var fileInfo in files)
            {
                names.Add(new FileInfo(repository.CreateZipCopyOfFile(fileInfo)));
            }

            return names;
        }
    }
}