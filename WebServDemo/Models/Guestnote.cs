using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServDemo.Core;
using WebServDemo.Factories;

namespace WebServDemo.Models
{
    public class Guestnote : Entity<Guestnote>
    {
        private void Init()
        {
            _repository = null;
            this.UUID = System.Guid.NewGuid().ToString();
            this.Created_at = DateTime.Now;
        }

        public Guestnote()
        {
            Init();
            GuestnoteFactory _factory = new GuestnoteFactory(Settings.XmlFileStorage, Settings.SqliteStorage);
            _repository = _factory.createRepository();
        }
        public string User { get; set; }
        public string Message { get; set; }
        public DateTime Created_at { get; set; }
        public string UUID { get; set; }
        
    }
}
