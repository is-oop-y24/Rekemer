using System.Collections.Generic;
using BackupsExtra.Algorithms.TypeOfHybrids;

namespace BackupsExtra.Algorithms.DeleteAlgorithms
{
    public class HybridDelete : PointsMerger, IDeleteAlgorithm
    {
        private readonly Hybrid _typeOfHybrid;

        public HybridDelete(Hybrid typeOfHybrid)
        {
            this._typeOfHybrid = typeOfHybrid;
        }

        public void Delete(ref List<RestorePoint> restorePoints)
        {
            if (restorePoints == null || restorePoints.Count == 0)
            {
                return;
            }

            var restorePointsToDelete = _typeOfHybrid.GetRestorePointsToDelete(restorePoints);
            ProccessOldPoints(ref restorePoints, restorePointsToDelete);
        }
    }
}