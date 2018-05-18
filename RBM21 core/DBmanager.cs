using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Diagnostics;

namespace RBM21_core
{
    class DBmanager
    {
        SQLiteConnection dbConnection;
        SQLiteCommand command;
        string path;

        public DBmanager(string path)
        {
            this.path = path;
            //SQLite memorize the database in a single file. If it doesn't exist, it must be created.
            if (!File.Exists(path))
                CreateDatabase(path);
            else
            {
                dbConnection = new SQLiteConnection("Data Source=" + path + ";Version=3;");
                dbConnection.Open();
            }                                       
        }

        public DateTime GetLastEntrance(string key)
        {
            Debug.WriteLine("GetEntrances " + key);
            //items are ordered  by time, descending order (example 2018-03-07 01:54:00):
            string sql = "select * from entrances where key = '" + key + "'  order by time desc;";

            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
                return new DateTime(1980, 1, 1);

            reader.Read();
            string data = (string)(reader["time"]);
            return DateTime.Parse(data);
        }

        public List<string> GetEntrances(string key)
        {            
            Debug.WriteLine("GetEntrances "+ key);
            //items are ordered  by time, descending order (example 2018-03-07 01:54:00):
            string sql = "select * from entrances where key = '" + key + "'  order by time desc;";                      

            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
                return null;

            List<string> result = new List<string>();
            while (reader.Read())
                result.Add((string)(reader["time"]));
            return result;
        }
   
        public int AddEntrance(string key, DateTime date)
        {
            string cmd = string.Format("insert into entrances (key, time) values (\"{0}\", \"{1}\")", key, date.ToString("yyyy-MM-dd HH:mm:ss"));
            command = new SQLiteCommand(cmd, dbConnection);
            int rows = command.ExecuteNonQuery();
            Tools.LogMessageToFile(String.Format("DBmanager - AddEntrance \"{0}\" at {1}. {2} rows affected (database: {3}).", key, date.ToString(), rows, path));

            return rows;
        }

        public void CreateDatabase(string path)
        {
            Tools.LogMessageToFile(String.Format("DBmanager - CreateDatabase {0} .", path));
            string cmd;
            Debug.WriteLine("Creating SQLite database file: {0}", path);
            SQLiteConnection.CreateFile(path);
            dbConnection = new SQLiteConnection("Data Source=" + path + ";Version=3;");
            dbConnection.Open();
                   
            cmd = "CREATE TABLE entrances (key TEXT(110), time TEXT(25));";
            command = new SQLiteCommand(cmd, dbConnection);
            command.ExecuteNonQuery();
            //dbConnection.Close();
        }
        
        public void Close()
        {
            dbConnection.Close();
        }
    }
}
