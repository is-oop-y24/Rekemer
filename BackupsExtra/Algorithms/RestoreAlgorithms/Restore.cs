using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace BackupsExtra.Algorithms.RestoreAlgorithms
{
    public abstract class Restore
    {
        public abstract void RestoreFiles(RestorePoint restorePoint);

        protected void DeleteFiles(List<string> files)
        {
            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }

        protected void ExtractZip(List<string> files)
        {
            foreach (var file in files)
            {
                try
                {
                    ZipFile.ExtractToDirectory(file, Path.GetDirectoryName(file));
                }
                catch
                {
                }
            }
        }

        protected List<string> GetFilesOfZip(List<string> files)
        {
            List<string> filesOfNewPoints = new List<string>();
            foreach (var file in files)
            {
                using (ZipArchive zip = ZipFile.Open(file, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        filesOfNewPoints.Add(Path.Combine(Path.GetDirectoryName(file), entry.Name));
                    }
                }
            }

            return filesOfNewPoints;
        }
    }
}