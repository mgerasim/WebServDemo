using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServDemo.Core
{
    public interface IRepository<T>
    {
        void Save(T entity);
        void Update(T entity);
        void Delete(T entity);
        T GetById(string UUID);
        IList<T> GetAll(string condition = "", string order = "");
    }
}
