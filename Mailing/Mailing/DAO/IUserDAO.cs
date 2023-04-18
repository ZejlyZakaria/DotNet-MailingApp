using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mailing.Models;

namespace Mailing.DAO
{
    public interface IUserDAO
    {
        int Add(User u);
        int Delete(int id);
        int Update(int id, User u);
        User SelectAll(string email, string password);
        User SelectAll(int id);
        List<User> SelectAllUsers();
        List<User> SelectAllUsers(string keyword);
    }
}
