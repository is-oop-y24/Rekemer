using ServerApplication.BusinessLayer.Controllers;
using ServerApplication.Common.Models;
using ServerApplication.DataLayer.Repository;

namespace ServerApplication.InterfaceLayer
{
    public class Manager
    {
        public TasksController TasksController { get; }
        public WorkersController WorkersController { get; }

        public Manager(DataContext dataContext)
        {
            TasksController = new TasksController(dataContext);
            WorkersController = new WorkersController(dataContext);
        }

        public void Save()
        {
            TasksController.Save();
            WorkersController.Save();
        }

        public void Load()
        {
            TasksController.Load();
            WorkersController.Load();
        }
    }
}