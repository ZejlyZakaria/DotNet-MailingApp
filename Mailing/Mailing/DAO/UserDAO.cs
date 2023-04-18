using Mailing.Models;
using System;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;

using Mailing.DbAccess;

namespace Mailing.DAO
{
    public class UserDAO : IUserDAO
    {
        private DbConnection db;
        private string tableName = "users";

        public UserDAO(DbConnection db)
        {
            this.db = db;
        }

        public int Add(User u)
        {
            string isResponsable;
            if (u.IsResponsable) isResponsable = "1";
            else isResponsable = "0";

            string randomPassword = GetRandomPassword(10);
            DateTime createdIn = DateTime.Now;

            u.Email = u.Email.ToLower();
            int result = db.Insert(tableName, u.FirstName, u.LastName, u.Email, randomPassword, u.UserService.Id, u.Grade, isResponsable, createdIn, "");
            if (result > 0)
            {
                string subject = "Bienvenue sur Mailing!";
                string content = "<div id=':ne' class='a3s aiL '>Salut, " + u.FirstName + "<br>" +
                "<br>Le responsable du bureau d'ordre a créé un compte pour vous dans Mailing, système de gestion des courriers.<br>" +
                "<br>Pour connecter à votre compte, suivez ce lien :<br><a href='https://localhost:44376/'>www.mailing.ac.ma</a><br>" +
                "En utilisant comme mot de passe : <b>" + randomPassword +
                "</b><br>Si vous avez besoin d'aide avec votre compte, vous pouvez demander au responsable à ashrafbetach@gmail.com.<br>" +
                "<br>Bienvenue dans Mailing, nous espérons que vous apprécierez l'utilisation de l'outil !<br><br>Cordialement,<br></div></div>";
                SendEmail(u.Email, content, subject);
            }
            return result;
        }

        public int Delete(int id)
        {
            return db.Delete(tableName, "id", id);
        }

        public int Update(int id, User u)
        {
            string isResponsable;
            if (u.IsResponsable) isResponsable = "1";
            else isResponsable = "0";

            string[] columns = { "firstName", "lastName", "email", "Password", "userService", "grade", "isResponsable", "modifiedIn" };
            string modifiedIn = DateTime.Now.ToString();
            int result = db.Update(tableName, "id", id, columns, u.FirstName, u.LastName, u.Email, u.Password, u.UserService.Id, u.Grade, isResponsable, modifiedIn);

            if (result > 0)
            {
                string content = "<div id=':ne' class='a3s aiL '>Salut, " + u.FirstName + "<br>" +
                "<br>Des modifications sont effectuées sur votre compte sur Mailing le " + modifiedIn + "<br>" +
                "Si vous n'avez rien fait, veuillez contacter le responsable de votre service <br><br>Cordialement,";
                string subject = "Vos informations sont modifiées";
                SendEmail(u.Email, content, subject);
            }

            return result;
        }

        public User SelectAll(string email, string password)
        {
            string[] columns = { "email", "password" };
            SqlDataReader reading = db.SelectAll(tableName, columns, email.ToLower(), password);
            User u = null;

            while (reading.Read())
            {
                u = new User();
                u.Id = reading.GetInt32(0);
                u.FirstName = reading.GetString(1);
                u.LastName = reading.GetString(2);
                u.Email = reading.GetString(3);
                u.Password = reading.GetString(4);
                u.UserService.Id = reading.GetInt32(5);
                u.Grade = reading.GetString(6);
                if (reading.GetInt32(7) == 0)
                {
                    u.IsResponsable = false;
                }
                else
                {
                    u.IsResponsable = true;
                }
                u.CreatedIn = reading.GetString(8);
                u.ModifiedIn = reading.GetString(9);

            }
            reading.Close();
            if (u != null)
            {
                string[] cols = { "id" };
                reading = db.SelectAll("services", cols, u.UserService.Id);
                while (reading.Read())
                {
                    if (reading.GetInt32(0) == u.UserService.Id)
                    {
                        u.UserService.Name = reading.GetString(1);
                    }

                }
                reading.Close();
            }
            return u;
        }

        public List<User> SelectAllUsers()
        {
            string[] columns = {};
            SqlDataReader reading = db.SelectAll(tableName, columns);
            List <User> listUsers = new List<User>();
            while (reading.Read())
            {
                User u = new User();
                u.Id = reading.GetInt32(0);
                u.FirstName = reading.GetString(1);
                u.LastName = reading.GetString(2);
                u.Email = reading.GetString(3);
                u.Password = reading.GetString(4);
                u.UserService.Id = reading.GetInt32(5);
                u.Grade = reading.GetString(6);
                if (reading.GetInt32(7) == 0)
                {
                    u.IsResponsable = false;
                }
                else
                {
                    u.IsResponsable = true;
                }
                u.CreatedIn = reading.GetString(8);
                u.ModifiedIn = reading.GetString(9);
                listUsers.Add(u);
            }
            reading.Close();

            foreach (User user in listUsers)
            {
                string[] columns2 = { "id" };
                reading = db.SelectAll("services", columns2, user.UserService.Id );
                while (reading.Read())
                {
                    if (reading.GetInt32(0) == user.UserService.Id)
                    {
                        user.UserService.Name = reading.GetString(1);
                    }
                }
                reading.Close();
            }
            return listUsers;
        }


        private void SendEmail(string to, string content, string subject)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("no-reply@ac.ma");
                message.To.Add(new MailAddress(to));
                message.Subject = subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = content;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("esisalibrary@gmail.com", "Esisa1999");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception) { }
        }

        private string GetRandomPassword(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public User SelectAll(int id)
        {
            string[] columns = { "id"};
            SqlDataReader reading = db.SelectAll(tableName, columns, id);
            User u = new User();

            while (reading.Read())
            {
                if (reading.GetInt32(0) == id)
                {
                    u.Id = reading.GetInt32(0);
                    u.FirstName = reading.GetString(1);
                    u.LastName = reading.GetString(2);
                    u.Email = reading.GetString(3);
                    u.Password = reading.GetString(4);
                    u.UserService.Id = reading.GetInt32(5);
                    u.Grade = reading.GetString(6);
                    if (reading.GetInt32(7) == 0)
                    {
                        u.IsResponsable = false;
                    }
                    else
                    {
                        u.IsResponsable = true;
                    }
                    u.CreatedIn = reading.GetString(8);
                    u.ModifiedIn = reading.GetString(9);
                }
            }
            reading.Close();
            if (u != null)
            {
                string[] cols = { "id" };
                reading = db.SelectAll("services", cols, u.UserService.Id);
                while (reading.Read())
                {
                    if (reading.GetInt32(0) == u.UserService.Id)
                    {
                        u.UserService.Name = reading.GetString(1);
                    }

                }
                reading.Close();
            }
            return u;
        }

        public List<User> SelectAllUsers(string keyword)
        {
            string[] columns = { "userService" };
            SqlDataReader reading = db.SelectAll(tableName, columns, keyword);
            List<User> listUsers = new List<User>();
            while (reading.Read())
            {
                User u = new User();
                u.Id = reading.GetInt32(0);
                u.FirstName = reading.GetString(1);
                u.LastName = reading.GetString(2);
                u.Email = reading.GetString(3);
                u.Password = reading.GetString(4);
                u.UserService.Id = reading.GetInt32(5);
                u.Grade = reading.GetString(6);
                if (reading.GetInt32(7) == 0)
                {
                    u.IsResponsable = false;
                }
                else
                {
                    u.IsResponsable = true;
                }
                u.CreatedIn = reading.GetString(8);
                u.ModifiedIn = reading.GetString(9);
                listUsers.Add(u);
            }
            reading.Close();

            foreach (User user in listUsers)
            {
                string[] columns2 = { "id" };
                reading = db.SelectAll("services", columns2, user.UserService.Id);
                while (reading.Read())
                {
                    if (reading.GetInt32(0) == user.UserService.Id)
                    {
                        user.UserService.Name = reading.GetString(1);
                    }
                }
                reading.Close();
            }
            return listUsers;
        }
    }
}