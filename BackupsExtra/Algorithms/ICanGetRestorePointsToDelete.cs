using System.Collections.Generic;

namespace BackupsExtra.Algorithms
{
    public interface ICanGetRestorePointsToDelete
    {
        public List<RestorePoint> GetRestorePointsToDelete(List<RestorePoint> restorePoints);
    }
}