using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mailing.Models
{
    public class Notification
    {
        private User to;
        private string title;
        private string content;
        private string date;

        public Notification()
        {
            To = new User();
        }

        public User To { get => to; set => to = value; }
        public string Title { get => title; set => title = value; }
        public string Content { get => content; set => content = value; }
        public string Date { get => date; set => date = value; }
    }
}