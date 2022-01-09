using System.Collections.Generic;

namespace ServerApplication.DataLayer.Repository
{
    public abstract class Repository<T> where T : IHaveID
    {
        protected DataContext.DataContext _dataContext;
        public List<T> Entities => _dataContext.Set<T>();

        public Repository(DataContext.DataContext dataContext)
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

        public bool Delete(string obj)
        {
            foreach (var entity in Entities)
            {
                if (entity.ID == obj)
                {
                    Entities.Remove(entity);
                    return true;
                }
                
            }

            return false;
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