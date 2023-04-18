using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mailing.Models
{
    public class Sender
    {
        private int id;
        private string name;
        private string address;
        private User user;

        public Sender()
        {

        }
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Address { get => address; set => address = value; }
        public User User { get => user; set => user = value; }
    }
}