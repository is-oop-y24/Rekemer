using ServerApplication.Common.Models;

namespace ServerApplication.DataLayer.Repository
{
    public class Workers : Repository<Worker>
    {
        public Workers(DataContext dataContext) : base(dataContext)
        {
        }

        public override void Load()
        {
            _dataContext.LoadWorkers();
        }
    }
}