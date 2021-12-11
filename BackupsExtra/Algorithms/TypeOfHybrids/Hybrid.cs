using System.Collections.Generic;
using System.Numerics;

namespace BackupsExtra.Algorithms.TypeOfHybrids
{
    public abstract class Hybrid : ICanGetRestorePointsToDelete
    {
        public readonly int AmountOfPoints;
        public readonly Vector3 ValidTime;

        public Hybrid(int amountOfPoints, Vector3 validTime)
        {
            this.AmountOfPoints = amountOfPoints;
            this.ValidTime = validTime;
        }

        public abstract List<RestorePoint> GetRestorePointsToDelete(List<RestorePoint> restorePoints);
    }
}