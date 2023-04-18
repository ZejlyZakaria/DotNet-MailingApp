using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Mailing.DbAccess;
using Mailing.Models;


namespace Mailing.DAO
{
    public class SenderDAO : ISenderDAO
    {
        private DbConnection db;
        private string tableName = "senders";

        public SenderDAO(DbConnection db)
        {
            this.db = db;
        }

        public int Add(Sender s)
        {
            string[] columns = { "name", "address"};
            return db.Insert(tableName, columns, s.Name, s.Address);
        }

        public List<Sender> SelectAllSenders()
        {
            string[] columns = { };
            SqlDataReader reading = db.SelectAll(tableName, columns);
            List<Sender> listSenders = new List<Sender>();
            while (reading.Read())
            {
                Sender s = new Sender();
                s.Id = reading.GetInt32(0);
                if (reading.GetString(1).Length > 0)
                {
                    s.Name = reading.GetString(1);
                    s.Address = reading.GetString(2);
                }
                else
                {
                    s.User.FirstName = reading.GetString(3);
                }
                listSenders.Add(s);
            }
            reading.Close();

            foreach (Sender sender in listSenders)
            {
                if (sender.User != null)
                {
                    string[] columns2 = { "id" };
                    reading = db.SelectAll("users", columns2, sender.User.Id);
                    while (reading.Read())
                    {
                        if (reading.GetInt32(0) == sender.User.Id)
                        {
                            sender.User.Id = reading.GetInt32(0);
                            sender.User.FirstName = reading.GetString(1);
                            sender.User.LastName = reading.GetString(2);
                            sender.User.Email = reading.GetString(3);
                            sender.User.Password = reading.GetString(4);
                            sender.User.UserService.Id = reading.GetInt32(5);
                            sender.User.Grade = reading.GetString(6);
                            if (reading.GetInt32(7) == 0)
                            {
                                sender.User.IsResponsable = false;
                            }
                            else
                            {
                                sender.User.IsResponsable = true;
                            }
                            sender.User.CreatedIn = reading.GetString(8);
                            sender.User.ModifiedIn = reading.GetString(9);
                        }
                    }
                    reading.Close();
                }

            }
            return listSenders;
        }
    }
}