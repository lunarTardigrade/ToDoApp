using System;
using System.Collections.Generic;
using System.IO;
using SQLite;

namespace ToDoApp
{
    public class DataManager<T>(Type dataType) where T : class, IIdentifiable, new() 
    {
        public Type DataType { get; set;} = dataType; 

        public static DataManager<T> Create()
        {
            return new DataManager<T>(typeof(T));
        }

        public SQLiteConnection Get_db_connection()
        {
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "tododatabase.db");
            var _db = new SQLiteConnection(databasePath);
            _db.CreateTable<T>();
            return _db;
        }

        public List<T> Get_table()
        {
            var db = Get_db_connection();
            return [.. db.Table<T>()];
                    
        }

        public void Insert_item(T item)
        {
            var db = Get_db_connection();
            db.Insert(item);
        }

        public void Delete_item(string identifier)
        {
            var db = Get_db_connection();
            var itemToDelete = db.Table<T>().FirstOrDefault(item => item.Identifier == identifier);
            if (itemToDelete != null)
            {
                db.Delete(itemToDelete);
            }
        }

        public void Update_item(T item)
        {
            var db = Get_db_connection();
            db.Update(item);
        }
    }
}
