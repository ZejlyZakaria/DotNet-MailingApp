using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using Mailing.DbAccess;
using Mailing.Models;


namespace Mailing.DAO
{
    public class ServiceDAO : IServiceDAO
    {
        private DbConnection db;
        private string tableName = "services";

        public ServiceDAO(DbConnection db)
        {
            this.db = db;
        }

        public int Add(Service s)
        {
            return db.Insert(tableName, s.Name);
        }

        public int Delete(int id)
        {
            return db.Delete(tableName, "id", id); 
        }

        public List<Service> SelectAllServices()
        {
            string[] columns = { };
            SqlDataReader reading = db.SelectAll(tableName, columns);
            List<Service> listServices = new List<Service>();
            while (reading.Read())
            {
                Service s = new Service();
                s.Id = reading.GetInt32(0);
                s.Name = reading.GetString(1);
                listServices.Add(s);
            }
            reading.Close();
            return listServices;
        }
    }
}