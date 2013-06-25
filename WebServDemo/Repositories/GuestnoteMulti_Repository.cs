using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServDemo.Core;
using WebServDemo.Models;

namespace WebServDemo.Repositories
{
    class GuestnoteMulti_Repository : Entity_Repository<Guestnote>
    {   
        protected List<IRepository<Guestnote>> _repositories = new List<IRepository<Guestnote>>();
        public GuestnoteMulti_Repository()
        {

        }

        public void AddRepo(IRepository<Guestnote> _repo)
        {
            _repositories.Add(_repo);
        }

        override public void Save(Guestnote entity)
        {
            foreach (IRepository<Guestnote> _repo in _repositories)
            {
                _repo.Save(entity);
            }
        }

        override public void Update(Guestnote entity)
        {
            foreach (IRepository<Guestnote> _repo in _repositories)
            {
                _repo.Update(entity);
            }
        }

        override public void Delete(Guestnote entity)
        {
            foreach (IRepository<Guestnote> _repo in _repositories)
            {
                _repo.Delete(entity);
            }
        }

        override public Guestnote GetById(string UUID)
        {
            return _repositories[0].GetById(UUID);         
        }

        override public IList<Guestnote> GetAll(string condition = "", string order = "")
        {
            return _repositories[0].GetAll(condition, order);
        }
    }
}
