using Mailing.DbAccess;
using Mailing.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Mailing.DAO
{
    public class MailDAO : IMailDAO
    {
        private DbConnection db;
        private string tableName = "mails";

        public MailDAO(DbConnection db)
        {
            this.db = db;
        }

        public int Add(Mail m)
        {
            if (m.MailSender.Name != null && m.MailSender.Address != null)
            {
                string[] senderCoulumns = { "name", "address" };
                db.Insert("senders", senderCoulumns, m.MailSender.Name, m.MailSender.Address);
                SqlDataReader reading = db.SelectLastId("senders");
                while (reading.Read())
                {
                    m.MailSender.Id = reading.GetInt32(0);
                }
                reading.Close();
                string[] columns = { "mailRef", "folderRef", "mailSender", "type", "nature", "demande", "purpose", "comment", "mailService", "mailFollowedBy", "mailResponsable", "writtenIn", "arrivedIn", "createdIn", "link" };
                return db.Insert(tableName, columns, m.MailRef, m.FolderRef, m.MailSender.Id, m.Type, m.Nature, m.Demande, m.Purpose, m.Comment, m.MailService.Id, m.MailFollowedBy.Id, m.MailResponsable.Id, m.WrittenIn, m.ArrivedIn, m.CreatedIn, m.Link);
            }
            else if (m.MailSender.Id > 0 || m.MailSender.User.Id > 0)
            {
                string[] columns = { "mailRef", "folderRef", "mailSender", "type", "nature", "demande", "purpose", "comment", "mailService", "mailFollowedBy", "mailResponsable", "writtenIn", "arrivedIn", "createdIn", "link" };
                return db.Insert(tableName, columns, m.MailRef, m.FolderRef, m.MailSender.Id, m.Type, m.Nature, m.Demande, m.Purpose, m.Comment, m.MailService.Id, m.MailFollowedBy.Id, m.MailResponsable.Id, m.WrittenIn, m.ArrivedIn, m.CreatedIn, m.Link);
            }
            else return -1;

        }

        public int Delete(int id)
        {
            return db.Delete(tableName, "id", id);
        }

        public int Reaffecter(int idMail, int id)
        {
            string[] columns = { "mailResponsable" };
            int result = db.Update(tableName, "id", id, columns, id);
            return result;
        }

        public List<Mail> SelectAllMails()
        {
            string[] columns = { };
            SqlDataReader reading = db.SelectAll(tableName, columns);
            List<Mail> listMails = new List<Mail>();
            while (reading.Read())
            {
                Mail m = new Mail();
                m.Id = reading.GetInt32(0);
                m.MailRef = reading.GetString(1);
                m.FolderRef = reading.GetString(2);
                m.MailSender.Id = reading.GetInt32(3);
                m.Type = reading.GetString(4);
                m.Nature = reading.GetString(5);
                m.WrittenIn = reading.GetString(6);
                m.ArrivedIn = reading.GetString(7);
                m.CreatedIn = reading.GetString(8);
                m.MailService.Id = reading.GetInt32(9);
                m.MailFollowedBy.Id = reading.GetInt32(10);
                m.MailResponsable.Id = reading.GetInt32(11);
                m.Demande = reading.GetString(12);
                m.Purpose = reading.GetString(13);
                m.Comment = reading.GetString(14);
                //m.MailResponse.Id = reading.GetInt32(15);
                m.Link = reading.GetString(16);
                listMails.Add(m);
            }
            reading.Close();

            foreach (Mail mail in listMails)
            {
                string[] columns2 = { "id" };
                reading = db.SelectAll("senders", columns2, mail.MailSender.Id);
                while (reading.Read())
                {
                    if (reading.GetInt32(0) == mail.MailSender.Id)
                    {
                        if (reading.GetString(1).Length > 0)
                        {
                            mail.MailSender.Name = reading.GetString(1);
                            mail.MailSender.Address = reading.GetString(2);

                        }
                        else
                        {
                            User user = new User();
                            user.Id = reading.GetInt32(3);
                            mail.MailSender.Id = user.Id;
                        }
                    }
                }
                reading.Close();
            }

            foreach (Mail mail in listMails)
            {
                string[] columns3 = { "id" };
                reading = db.SelectAll("services", columns3, mail.MailService.Id);
                while (reading.Read())
                {
                    if (reading.GetInt32(0) == mail.MailService.Id)
                    {
                        mail.MailService.Name = reading.GetString(1);
                    }
                }
                reading.Close();
            }

            foreach (Mail mail in listMails)
            {
                string[] columns4 = { "id" };
                reading = db.SelectAll("users", columns4, mail.MailFollowedBy.Id);
                while (reading.Read())
                {
                    if (reading.GetInt32(0) == mail.MailFollowedBy.Id)
                    {
                        mail.MailFollowedBy.FirstName = reading.GetString(1);
                        mail.MailFollowedBy.LastName = reading.GetString(2);
                    }
                }
                reading.Close();
            }

            foreach (Mail mail in listMails)
            {
                string[] columns5 = { "id" };
                reading = db.SelectAll("users", columns5, mail.MailResponsable.Id);
                while (reading.Read())
                {
                    if (reading.GetInt32(0) == mail.MailResponsable.Id)
                    {
                        mail.MailResponsable.FirstName = reading.GetString(1);
                        mail.MailResponsable.LastName = reading.GetString(2);
                    }
                }
                reading.Close();
            }

            //foreach (Mail mail in listMails)
            //{
            //    string[] columns6 = { "id" };
            //    reading = db.SelectAll("responses", columns6, mail.MailResponse.Id);
            //    while (reading.Read())
            //    {
            //        if (reading.GetInt32(0) == mail.MailResponsable.Id)
            //        {
            //            mail.MailResponse.Text = reading.GetString(2);
            //        }
            //    }
            //    reading.Close();
            //}
            return listMails;
        }

        public List<Mail> SelectAllMailsWhere(int id)
        {
            List<Mail> listMails = new List<Mail>();
            string[] columnsSenders = {};
            SqlDataReader readingSenders = db.SelectAll("senders", columnsSenders);
            List<Sender> listSenders = new List<Sender>();
            while (readingSenders.Read())
            {
                if (readingSenders.GetString(1).Length == 0)
                {
                    Sender s = new Sender();
                    s.Id = readingSenders.GetInt32(0);
                    s.Name = readingSenders.GetString(1);
                    s.Address = readingSenders.GetString(2);
                    s.User.Id = readingSenders.GetInt32(3);
                    listSenders.Add(s);
                }

            }
            readingSenders.Close();
            foreach (Sender s in listSenders)
            {
                if (s.User.Id == id)
                {
                    string[] columns = { "mailSender", "mailResponsable" };
                    SqlDataReader reading = db.SelectAllOr(tableName, columns, s.Id, id);
                   
                    while (reading.Read())
                    {
                        Mail m = new Mail();
                        m.Id = reading.GetInt32(0);
                        m.MailRef = reading.GetString(1);
                        m.FolderRef = reading.GetString(2);
                        m.MailSender.Id = s.Id;
                        m.Type = reading.GetString(4);
                        m.Nature = reading.GetString(5);
                        m.WrittenIn = reading.GetString(6);
                        m.ArrivedIn = reading.GetString(7);
                        m.CreatedIn = reading.GetString(8);
                        m.MailService.Id = reading.GetInt32(9);
                        m.MailFollowedBy.Id = reading.GetInt32(10);
                        m.MailResponsable.Id = id;
                        m.Demande = reading.GetString(12);
                        m.Purpose = reading.GetString(13);
                        m.Comment = reading.GetString(14);
                        //m.MailResponse.Id = reading.GetInt32(15);
                        m.Link = reading.GetString(16);
                        listMails.Add(m);
                    }
                    reading.Close();

                  
                    
                    foreach (Mail mail in listMails)
                    {
                        string[] columns3 = { "id" };
                        reading = db.SelectAll("services", columns3, mail.MailService.Id);
                        while (reading.Read())
                        {
                            if (reading.GetInt32(0) == mail.MailService.Id)
                            {
                                mail.MailService.Name = reading.GetString(1);
                            }
                        }
                        reading.Close();
                    }

                    foreach (Mail mail in listMails)
                    {
                        string[] columns4 = { "id" };
                        reading = db.SelectAll("users", columns4, mail.MailFollowedBy.Id);
                        while (reading.Read())
                        {
                            if (reading.GetInt32(0) == mail.MailFollowedBy.Id)
                            {
                                mail.MailFollowedBy.FirstName = reading.GetString(1);
                                mail.MailFollowedBy.LastName = reading.GetString(2);
                            }
                        }
                        reading.Close();
                    }

                    foreach (Mail mail in listMails)
                    {
                        string[] columns5 = { "id" };
                        reading = db.SelectAll("users", columns5, mail.MailResponsable.Id);
                        while (reading.Read())
                        {
                            if (reading.GetInt32(0) == mail.MailResponsable.Id)
                            {
                                mail.MailResponsable.FirstName = reading.GetString(1);
                                mail.MailResponsable.LastName = reading.GetString(2);
                            }
                        }
                        reading.Close();
                    }
                }
            }


            string[] columnsS = { "mailResponsable" };
            SqlDataReader reading3 = db.SelectAllOr(tableName, columnsS, id);

            while (reading3.Read())
            {
                Mail m = new Mail();
                m.Id = reading3.GetInt32(0);
                m.MailRef = reading3.GetString(1);
                m.FolderRef = reading3.GetString(2);
                m.MailSender.Id = reading3.GetInt32(3);
                m.Type = reading3.GetString(4);
                m.Nature = reading3.GetString(5);
                m.WrittenIn = reading3.GetString(6);
                m.ArrivedIn = reading3.GetString(7);
                m.CreatedIn = reading3.GetString(8);
                m.MailService.Id = reading3.GetInt32(9);
                m.MailFollowedBy.Id = reading3.GetInt32(10);
                m.MailResponsable.Id = id;
                m.Demande = reading3.GetString(12);
                m.Purpose = reading3.GetString(13);
                m.Comment = reading3.GetString(14);
                //m.MailResponse.Id = reading.GetInt32(15);
                m.Link = reading3.GetString(16);
                listMails.Add(m);
            }
            reading3.Close();



            foreach (Mail mail in listMails)
            {
                string[] columns3 = { "id" };
                reading3 = db.SelectAll("services", columns3, mail.MailService.Id);
                while (reading3.Read())
                {
                    if (reading3.GetInt32(0) == mail.MailService.Id)
                    {
                        mail.MailService.Name = reading3.GetString(1);
                    }
                }
                reading3.Close();
            }

            foreach (Mail mail in listMails)
            {
                string[] columns4 = { "id" };
                reading3 = db.SelectAll("users", columns4, mail.MailFollowedBy.Id);
                while (reading3.Read())
                {
                    if (reading3.GetInt32(0) == mail.MailFollowedBy.Id)
                    {
                        mail.MailFollowedBy.FirstName = reading3.GetString(1);
                        mail.MailFollowedBy.LastName = reading3.GetString(2);
                    }
                }
                reading3.Close();
            }

            foreach (Mail mail in listMails)
            {
                string[] columns5 = { "id" };
                reading3 = db.SelectAll("users", columns5, mail.MailResponsable.Id);
                while (reading3.Read())
                {
                    if (reading3.GetInt32(0) == mail.MailResponsable.Id)
                    {
                        mail.MailResponsable.FirstName = reading3.GetString(1);
                        mail.MailResponsable.LastName = reading3.GetString(2);
                    }
                }
                reading3.Close();
            }

            foreach (Mail mail in listMails)
            {
                string[] columns2 = { "id" };
                reading3 = db.SelectAll("senders", columns2, mail.MailSender.Id);
                while (reading3.Read())
                {
                    if (reading3.GetInt32(0) == mail.MailSender.Id)
                    {
                        if (reading3.GetString(1).Length > 0)
                        {
                            mail.MailSender.Name = reading3.GetString(1);
                            mail.MailSender.Address = reading3.GetString(2);

                        }
                        else
                        {
                            User user = new User();
                            user.Id = reading3.GetInt32(3);
                            mail.MailSender.Id = user.Id;
                        }
                    }
                }
                reading3.Close();
            }

            return listMails;
        }

        public int Update(int id, Mail m)
        {
            string[] columns = { "mailRef", "folderRef", "mailSender", "type", "nature", "writtenIn", "arrivedIn", "createdIn", "mailService", "mailFollowedBy", "mailResponsable", "demande", "purpose", "comment" };
            int result = db.Update(tableName, "id", id, columns, m.MailRef, m.FolderRef, m.MailSender.Id, m.Type, m.Nature, m.WrittenIn, m.ArrivedIn, m.CreatedIn, m.MailService.Id, m.MailFollowedBy.Id, m.MailResponsable.Id, m.Demande, m.Purpose, m.Comment);
            return result;
        }
    }
}