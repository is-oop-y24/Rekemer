using System;
using System.Collections.Generic;
using System.Numerics;


namespace BackupsExtra.Algorithms.DeleteAlgorithms
{
    public class ValidTimeDelete : PointsMerger, IDeleteAlgorithm, ICanGetRestorePointsToDelete
    {
        private readonly Vector3 _validTime;


        public ValidTimeDelete(Vector3 validTime)
        {
            _validTime = validTime;
        }


        public void Delete(ref List<RestorePoint> restorePoints)
        {
            if (restorePoints == null || restorePoints.Count == 0)
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
            foreach (RestorePoint point in restorePoints)
            {
                string dateString = point._time;
                DateTime dateTime = Convert.ToDateTime(dateString);
                var timeDiff = Time.Instance.CurrentTime - dateTime;
                if (timeDiff.TotalDays > (_validTime.Z + _validTime.Y * 29 + _validTime.X * 365))
                {
                    points.Add(queue.Dequeue());
                }
            }

            return points;
        }
    }
}