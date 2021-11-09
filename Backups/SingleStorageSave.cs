using System.Collections.Generic;
using System.IO;

namespace Backups
{
    public class SingleStorageSave : IAlgorithm
    {
        public List<FileInfo> Operation(List<FileInfo> files, IRepository repository)
        {
            string directory = repository.AddFilesToArchive(files);
            List<FileInfo> file = new List<FileInfo>();
            if (directory != null)
            {
                file.Add(new FileInfo(directory));
                return file;
            }

            return file;
        }
    }
}