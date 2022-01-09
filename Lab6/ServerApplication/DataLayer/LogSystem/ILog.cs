namespace ServerApplication.DataLayer.LogSystem
{
    public interface ILog
    {
        public void Log(string message);
        string FindTimesOfParticularAction(string nameOfAction);
    }
}