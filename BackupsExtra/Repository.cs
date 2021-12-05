
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;


namespace BackupsExtra
{
    
    [DataContract]
    public class Repository : IRepository
    {
        [DataMember]
        private string _directoryInfo;
       
        public string DirectoryInfo
        {
            get { return _directoryInfo; }
        }

        public Repository()
        {
            
        }
        public Repository(string directory)
        {
            
                _directoryInfo =directory;
            
        }

        public string CreateZipCopyOfFile(string file)
        {
            var name = Path.GetFileNameWithoutExtension(file);
            var zipFile = Path.Combine(_directoryInfo, name + ".zip");
            int i = 1;
            while (File.Exists(zipFile))
            {
                //string path = @"photo\myFolder\image.jpg";
                string newFileName = $@"{name}_" + i.ToString();

                string dir = Path.GetDirectoryName(zipFile);
                string ext = Path.GetExtension(zipFile);
                zipFile = Path.Combine(dir, newFileName + ext); // @"photo\myFolder\image-resize.jpg"
                i++;
            }

            using var archive = ZipFile.Open($@"{zipFile}", ZipArchiveMode.Create);
            archive.CreateEntryFromFile(file, Path.GetFileName(file));
            return zipFile;
        }

        public string AddFilesToArchive(List<string> files)
        {
            var zipFile = Path.Combine(_directoryInfo, "myzip.zip");
            int i = 1;
            while (File.Exists(zipFile))
            {
                //string path = @"photo\myFolder\image.jpg";
                string newFileName = @"myzip_" + i.ToString();

                string dir = Path.GetDirectoryName(zipFile);
                string ext = Path.GetExtension(zipFile);
                zipFile = Path.Combine(dir, newFileName + ext); // @"photo\myFolder\image-resize.jpg"
                i++;
            }

            using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
            {
                foreach (var fPath in files)
                {
                    archive.CreateEntryFromFile(fPath, Path.GetFileName(fPath));
                }
            }

            return zipFile;
        }
    }
}