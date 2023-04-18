using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mailing.Models;


namespace Mailing.DAO
{
   public interface IMailDAO
    {
        List<Mail> SelectAllMails();
        List<Mail> SelectAllMailsWhere(int id);
        int Add(Mail m);
        int Delete(int id);
        int Update(int id, Mail m);
        int Reaffecter(int idMail, int id);
    }
}
