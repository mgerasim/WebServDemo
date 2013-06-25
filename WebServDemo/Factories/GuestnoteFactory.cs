using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServDemo.Core;
using WebServDemo.Models;
using WebServDemo.Repositories;

namespace WebServDemo.Factories
{
    public class GuestnoteFactory : IFactory<IRepository<Guestnote>>
    {
        protected int XmlFileStorage;
        protected int SqliteStorage;
        public GuestnoteFactory(int XmlFileStorage = 1, int SqliteStorage = 0)
        {
            this.XmlFileStorage = XmlFileStorage;
            this.SqliteStorage = SqliteStorage;
        }

        public IRepository<Guestnote> createRepository()
        {
            GuestnoteMulti_Repository _repo = new GuestnoteMulti_Repository();
            if (XmlFileStorage == 1)
            {
                _repo.AddRepo(new GuestnoteXmlFile_Repository());
            }
            if (SqliteStorage == 1)
            {
                _repo.AddRepo(new GuestnoteSqlite_Repository());
            }
            return _repo;
        }

    }
}
