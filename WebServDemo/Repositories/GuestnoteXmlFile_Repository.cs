using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServDemo.Core;
using WebServDemo.Models;
using System.Xml.Serialization;
using System.IO;

namespace WebServDemo.Repositories
{
    class GuestnoteXmlFile_Repository : Entity_Repository<Guestnote>
    {   
        public GuestnoteXmlFile_Repository()
        {
        }

        override public void Save(Guestnote entity)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Guestnote));
            TextWriter writer = new StreamWriter(entity.UUID+".xml");
            serializer.Serialize(writer, entity);
            writer.Close();
        }

        override public void Update(Guestnote entity)
        {
            Save(entity);
        }

        override public void Delete(Guestnote entity)
        {
            
        }

        override public Guestnote GetById(string UUID)
        {
            string filename = UUID + ".xml";
            XmlSerializer serializer = new XmlSerializer(typeof(Guestnote));
            FileStream fs = new FileStream(filename, FileMode.Open);
            Guestnote theGuestnote;
            theGuestnote = (Guestnote)serializer.Deserialize(fs);
            fs.Close();
            return theGuestnote;
        }

        override public IList<Guestnote> GetAll(string condition = "", string order = "")
        {
            IList<Guestnote> theList = new List<Guestnote>();
            DirectoryInfo dirInfo = new DirectoryInfo(Directory.GetCurrentDirectory());

            foreach (FileInfo file in dirInfo.GetFiles("*.xml"))
            {                
                Guestnote theGuestnote = new Guestnote();
                theGuestnote = theGuestnote.GetById(file.Name.Split('.')[0]);
                theList.Add(theGuestnote);
            }
            return theList;
        }
    }
}
