using System.Collections.Generic;

namespace BackupsExtra.Algorithms.SaveAlgorithms
{
    
    public class SplitStorageSave : IAlgorithm
    {
        public List<string> Operation(List<string> files, IRepository repository)
        {
            List<string> names = new List<string>();
            foreach (var fileInfo in files)
            {
                names.Add(repository.CreateZipCopyOfFile(fileInfo));
            }

            return names;
        }
        public string NameOfAlgorithm()
        {
            return "SplitStorageSave";
        }
    }
}