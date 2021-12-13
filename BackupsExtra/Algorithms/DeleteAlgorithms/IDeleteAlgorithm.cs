using System.Collections.Generic;

namespace BackupsExtra.Algorithms.DeleteAlgorithms
{
    public interface IDeleteAlgorithm
    {
        public void Delete(ref List<RestorePoint> restorePoints);
    }
}