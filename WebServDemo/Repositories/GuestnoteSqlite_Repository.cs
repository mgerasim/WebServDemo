using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServDemo.Core;
using WebServDemo.Models;
using System.Xml.Serialization;
using System.IO;
using System.Data.SQLite;
using System.Data;

namespace WebServDemo.Repositories
{    
    class GuestnoteSqlite_Repository : Entity_Repository<Guestnote>
    {           
        protected readonly string db_filename = "geustnotes.db";

        public GuestnoteSqlite_Repository()
        {
            if (File.Exists(db_filename))
            {
                return;
            }
            SQLiteConnection.CreateFile(db_filename);
 
            SQLiteConnection con = new SQLiteConnection();
            SQLiteConnectionStringBuilder conString = new SQLiteConnectionStringBuilder();
            conString.DataSource = db_filename;
            con.ConnectionString = conString.ToString();
            using (con)
            {
                try
                {
                    con.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Can't connect to new data base. Reason: " + e.Message);
                    throw;
                }
 
                // Create data base structure
                SQLiteCommand createDataBase = con.CreateCommand();    // Useful method
                createDataBase.CommandText = ""+
                    "CREATE TABLE guestnotes( " +
                    "uuid TEXT, " +
                    "User TEXT, " +
                    "Message TEXT " +
                    ")" +
                                                           " ";
                createDataBase.ExecuteNonQuery();
            }
        }

        private SQLiteConnection Connect()
        {
            SQLiteConnection connection = new SQLiteConnection();
            SQLiteConnectionStringBuilder cs = new SQLiteConnectionStringBuilder();
            cs.DataSource = db_filename;
            connection.ConnectionString = cs.ToString();
            connection.Open();
            return connection;
        }

        private int ExecuteNonQuery(string sql)
        {
            SQLiteConnection cnn = Connect();
            SQLiteCommand mycommand = new SQLiteCommand(cnn);
            mycommand.CommandText = sql;
            int rowsUpdated = mycommand.ExecuteNonQuery();
            cnn.Close();
            return rowsUpdated;
        }
        

        override public void Save(Guestnote entity)
        {
            try
            {
                this.ExecuteNonQuery(String.Format("insert into guestnotes(uuid, user, message) values('{0}', '{1}', '{2}')", 
                    entity.UUID, entity.User, entity.Message));             
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        override public void Update(Guestnote entity)
        {
            try
            {
                this.ExecuteNonQuery(String.Format("update guestnotes set user='{0}', message='{1}' where uuid='{2}'",
                    entity.User, entity.Message, entity.UUID));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        override public void Delete(Guestnote entity)
        {
            try
            {
                this.ExecuteNonQuery(String.Format("delete from guestnotes where uuid='{0}'", entity.UUID));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        override public Guestnote GetById(string UUID)
        {
            Guestnote theGuestnote = new Guestnote();
            DataTable dt = new DataTable();
            try
            {
                SQLiteConnection cnn = Connect();
                SQLiteCommand mycommand = new SQLiteCommand(cnn);
                mycommand.CommandText = String.Format("SELECT * FROM guestnotes WHERE uuid = {0}", UUID);
                SQLiteDataReader reader = mycommand.ExecuteReader();
                dt.Load(reader);

                foreach (DataRow r in dt.Rows)
                {
                    theGuestnote.UUID = (r["UUID"].ToString());
                    theGuestnote.User = (r["User"].ToString());
                    theGuestnote.Message = (r["Message"].ToString());
                }

                reader.Close();
                cnn.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return theGuestnote;
        }

        override public IList<Guestnote> GetAll(string condition = "", string order = "")
        {
            List<Guestnote> theList = new List<Guestnote>();
            DataTable dt = new DataTable();
            try
            {
                SQLiteConnection cnn = Connect();
                SQLiteCommand mycommand = new SQLiteCommand(cnn);
                mycommand.CommandText = String.Format("SELECT * FROM guestnotes ");
                SQLiteDataReader reader = mycommand.ExecuteReader();
                dt.Load(reader);

                foreach (DataRow r in dt.Rows)
                {
                    Guestnote theGuestnote = new Guestnote();
                    theGuestnote.UUID = (r["UUID"].ToString());
                    theGuestnote.User = (r["User"].ToString());
                    theGuestnote.Message = (r["Message"].ToString());
                    theList.Add(theGuestnote);
                }

                reader.Close();
                cnn.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return theList;
        }
    }
}
