using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mailing.Models
{
    public class Response
    {
        private int id;
        private string mailRef;
        private string text;
        private User followedBy;

        public Response()
        {

        }

        public int Id { get => id; set => id = value; }
        public string MailRef { get => mailRef; set => mailRef = value; }
        public string Text { get => text; set => text = value; }
        public User FollowedBy { get => followedBy; set => followedBy = value; }
    }
}