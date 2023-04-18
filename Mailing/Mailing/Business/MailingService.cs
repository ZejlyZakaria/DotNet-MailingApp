using Mailing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mailing.DAO;

namespace Mailing.Business
{
    public class MailingService : IMailingService
    {
        private IUserDAO userDAO;
        private IServiceDAO serviceDAO;
        private IMailDAO mailDAO;
        private ISenderDAO senderDAO;

        public MailingService()
        {}

        public MailingService(IUserDAO userDAO, IServiceDAO serviceDAO, IMailDAO mailDAO, ISenderDAO senderDAO)
        {
            this.userDAO = userDAO;
            this.serviceDAO = serviceDAO;
            this.mailDAO = mailDAO;
            this.senderDAO = senderDAO;
        }

        public int AddMail(Mail m)
        {
            return mailDAO.Add(m);
        }

        public int AddSender(Sender s)
        {
            return senderDAO.Add(s);
        }

        public int AddUser(User u)
        {
            return userDAO.Add(u);
        }

        public User AuthenticateUser(string email, string password)
        {
            return userDAO.SelectAll(email, password);
        }

        public int DeleteMail(int id)
        {
            return mailDAO.Delete(id);
        }

        public int DeleteUser(int id)
        {
            return userDAO.Delete(id);
        }

        public List<Mail> GetMails()
        {
            return mailDAO.SelectAllMails();
        }

        public List<Mail> GetMailsByUser(int id)
        {
            return mailDAO.SelectAllMailsWhere(id);
        }

        public List<Sender> GetSenders()
        {
            return senderDAO.SelectAllSenders();
        }

        public List<Service> GetServices()
        {
            return serviceDAO.SelectAllServices();
        }

        public User GetUser(int id)
        {
            return userDAO.SelectAll(id);
        }

        public List<User> GetUsers()
        {
            return userDAO.SelectAllUsers();
        }

        public List<User> GetUsers(string keyword)
        {
            return userDAO.SelectAllUsers(keyword);
        }

        public int Reaffecter(int idMail, int id)
        {
            return mailDAO.Reaffecter(idMail, id);
        }

        public int UpdateMail(int id, Mail m)
        {
            return mailDAO.Update(id, m);
        }

        public int UpdateUser(int id, User u)
        {
            return userDAO.Update(id, u);
        }
    }
}