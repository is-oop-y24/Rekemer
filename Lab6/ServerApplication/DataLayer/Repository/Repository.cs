using System.Collections.Generic;

namespace ServerApplication.DataLayer.Repository
{
    public abstract class Repository<T> where T : IHaveID
    {
        protected DataContext _dataContext;
        public List<T> Entities => _dataContext.Set<T>();

        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public T GetById(string id)
        {
            return Entities.Find(t => t.ID == id);
        }

        public void Save()
        {
            _dataContext.Save();
        }

        public abstract void Load();

        public void Delete(T obj)
        {
            Entities.Remove(obj);
        }

        public void Create(T obj)
        {
            if (Entities.Find(t => t.ID == obj.ID) == null)
            {
                Entities.Add(obj);
            }
        }
    }
}