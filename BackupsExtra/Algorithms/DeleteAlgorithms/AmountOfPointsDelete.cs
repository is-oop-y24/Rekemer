using System.Collections.Generic;

namespace BackupsExtra.Algorithms.DeleteAlgorithms
{
    public class AmountOfPointsDelete : PointsMerger, IDeleteAlgorithm, ICanGetRestorePointsToDelete
    {
        private readonly int _validPoints;

        public AmountOfPointsDelete(int validPoints)
        {
            _validPoints = validPoints;
        }

        public void Delete(ref List<RestorePoint> restorePoints)
        {
            if (restorePoints == null || restorePoints.Count == 0 || restorePoints.Count < _validPoints)
            {
                return;
            }

            var restorePointsToDelete = GetRestorePointsToDelete(restorePoints);
            ProccessOldPoints(ref restorePoints, restorePointsToDelete);
        }

        public List<RestorePoint> GetRestorePointsToDelete(List<RestorePoint> restorePoints)
        {
            List<RestorePoint> points = new List<RestorePoint>();
            var queue = new Queue<RestorePoint>(restorePoints);
            while (queue.Count > _validPoints)
            {
                points.Add(queue.Dequeue());
            }

            return points;
        }
    }
}