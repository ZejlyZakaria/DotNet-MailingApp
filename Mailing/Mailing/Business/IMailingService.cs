using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mailing.Models;

namespace Mailing.Business
{
    interface IMailingService
    {
        // User Methods
        int AddUser(User u);
        int DeleteUser(int id);
        int UpdateUser(int id, User u);
        User AuthenticateUser(string email, string password);
        int Reaffecter(int idMail, int id);
        List<User> GetUsers();
        List<User> GetUsers(string keyword);
        User GetUser(int id);
        List<Service> GetServices();

         // Mail Methods
        List<Mail> GetMails();
        List<Mail> GetMailsByUser(int id);
        int AddMail(Mail m);
        int DeleteMail(int id);
        int UpdateMail(int id, Mail m);

        // Sender Methods
        int AddSender(Sender s);
        List<Sender> GetSenders();
    }
}
