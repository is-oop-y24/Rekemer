using ServerApplication.Common.Models;

namespace ServerApplication.DataLayer.Repository
{
    public class Tasks: Repository<Task>
    {
        public Tasks(DataContext.DataContext dataContext) : base(dataContext)
        {
        }

        public override void Load()
        {
            {
                _dataContext.LoadTasks();
            }
        }
    }
}