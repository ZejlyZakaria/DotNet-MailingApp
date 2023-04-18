using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mailing.Models
{
    public class Service
    {
        private int id;
        private string name;
        
        public Service()
        {

        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
    }
}