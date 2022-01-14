using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Backups
{
    public class Repository : IRepository
    {
        private DirectoryInfo _directoryInfo;
        public Repository(string directory)
        {
            if (Directory.Exists(directory))
            {
                _directoryInfo = new DirectoryInfo(directory);
            }
            else
            {
                _directoryInfo = Directory.CreateDirectory(directory);
            }
        }

        public DirectoryInfo DirectoryInfo
        {
            get { return new DirectoryInfo(_directoryInfo.FullName); }
        }

        public string CreateZipCopyOfFile(FileInfo file)
        {
            var name = Path.GetFileNameWithoutExtension(file.Name);
            var zipFile = Path.Combine(_directoryInfo.FullName, name + ".zip");
            int i = 1;
            while (File.Exists(zipFile))
            {
                string newFileName = $@"{name}_" + i.ToString();

                string dir = Path.GetDirectoryName(zipFile);
                string ext = Path.GetExtension(zipFile);
                zipFile = Path.Combine(dir, newFileName + ext);
                i++;
            }

            using var archive = ZipFile.Open($@"{zipFile}", ZipArchiveMode.Create);
            archive.CreateEntryFromFile(file.FullName, Path.GetFileName(file.FullName));
            return zipFile;
        }

        public string AddFilesToArchive(List<FileInfo> files)
        {
            var zipFile = Path.Combine(_directoryInfo.FullName, "myzip.zip");
            int i = 1;
            while (File.Exists(zipFile))
            {
                string newFileName = @"myzip_" + i.ToString();

                string dir = Path.GetDirectoryName(zipFile);
                string ext = Path.GetExtension(zipFile);
                zipFile = Path.Combine(dir, newFileName + ext);
                i++;
            }

            using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
            {
                foreach (var fPath in files)
                {
                    archive.CreateEntryFromFile(fPath.FullName, Path.GetFileName(fPath.FullName));
                }
            }

            return zipFile;
        }
    }
}