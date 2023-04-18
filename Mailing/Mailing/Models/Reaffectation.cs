using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mailing.Models
{
    public class Reaffectation
    {
        private int id;
        private string date;
        private string motif;
        private User reaffectationAgent;
        private User reaffectationFollowedBy;
        private Mail reaffectationMail;

        public Reaffectation()
        {

        }

        public int Id { get => id; set => id = value; }
        public string Date { get => date; set => date = value; }
        public string Motif { get => motif; set => motif = value; }
        public User ReaffectationAgent { get => reaffectationAgent; set => reaffectationAgent = value; }
        public User ReaffectationFollowedBy { get => reaffectationFollowedBy; set => reaffectationFollowedBy = value; }
        public Mail ReaffectationMail { get => reaffectationMail; set => reaffectationMail = value; }
    }
}