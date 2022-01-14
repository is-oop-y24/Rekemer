using System.Collections.Generic;
using System.Numerics;

namespace BackupsExtra.Algorithms.TypeOfHybrids
{
    public abstract class Hybrid : ICanGetRestorePointsToDelete
    {
        private readonly int _amountOfPoints;
        private readonly Vector3 _validTime;
        public Hybrid(int amountOfPoints, Vector3 validTime)
        {
            this._amountOfPoints = amountOfPoints;
            this._validTime = validTime;
        }

        public int AmountOfPoints { get => _amountOfPoints; }
        public Vector3 ValidTime { get => _validTime; }
        public abstract List<RestorePoint> GetRestorePointsToDelete(List<RestorePoint> restorePoints);
    }
}