using System.Collections.Generic;

namespace BackupsExtra.Algorithms.SaveAlgorithms
{
    
    public interface IAlgorithm
    {
        List<string> Operation(List<string> files,  IRepository repository);
        public string NameOfAlgorithm();
    }
    
}