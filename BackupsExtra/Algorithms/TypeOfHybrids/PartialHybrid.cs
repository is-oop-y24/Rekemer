using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using BackupsExtra.Algorithms.DeleteAlgorithms;

namespace BackupsExtra.Algorithms.TypeOfHybrids
{
    public class PartialHybrid : Hybrid
    {
        public PartialHybrid(int amountOfPoints, Vector3 validTime)
            : base(amountOfPoints, validTime)
        {
        }

        public override List<RestorePoint> GetRestorePointsToDelete(List<RestorePoint> restorePoints)
        {
            ICanGetRestorePointsToDelete timeDelete = new ValidTimeDelete(ValidTime);
            ICanGetRestorePointsToDelete amountDelete = new AmountOfPointsDelete(AmountOfPoints);
            var pointsTime = timeDelete.GetRestorePointsToDelete(restorePoints);
            var pointsAmount = amountDelete.GetRestorePointsToDelete(restorePoints);
            var intersectedPoints = pointsTime.Union(pointsAmount).ToList();
            return intersectedPoints;
        }
    }
}