using System.Collections.Generic;

namespace BackupsExtra.Algorithms.SaveAlgorithms
{
   
    public class SingleStorageSave : IAlgorithm
    {
        public List<string> Operation(List<string> files, IRepository repository)
        {
            string directory = repository.AddFilesToArchive(files);
            List<string> file = new List<string>();
            if (directory != null)
            {
                file.Add(directory);
                return file;
            }

            return file;
        }

        public string NameOfAlgorithm()
        {
            return "SingleStorageSave";
        }
    }
}