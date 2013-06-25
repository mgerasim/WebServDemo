using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServDemo.Core
{
    class Entity_Repository<T> : IRepository<T>
    {
        public virtual void Save(T entity)
        {

        }
        public virtual void Update(T entity)
        {
        }

        public virtual void Delete(T entity)
        {
        }

        public virtual T GetById(string UUID)
        {
            return default(T);
        }

        public virtual IList<T> GetAll(string condition = "", string order = "")
        {
            return null;
        }
    }
}
