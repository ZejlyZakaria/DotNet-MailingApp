using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mailing.Models
{
    public class User
    {
        private int id;
        private string firstName;
        private string lastName;
        private string email;
        private string password;
        private Service userService;
        private string grade;
        private bool isResponsable;
        private string createdIn;
        private string modifiedIn;

        public User()
        {
            UserService = new Service();
        }

        public int Id { get => id; set => id = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public Service UserService { get => userService; set => userService = value; }
        public string Grade { get => grade; set => grade = value; }
        public bool IsResponsable { get => isResponsable; set => isResponsable = value; }
        public string CreatedIn { get => createdIn; set => createdIn = value; }
        public string ModifiedIn { get => modifiedIn; set => modifiedIn = value; }
    }
}