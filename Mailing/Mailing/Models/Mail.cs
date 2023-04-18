using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mailing.Models
{
    public class Mail
    {
        private int id;
        private string mailRef;
        private string folderRef;
        private Sender mailSender;
        private string type;
        private string nature;
        private string writtenIn;
        private string arrivedIn;
        private string createdIn;
        private Service mailService;
        private User mailFollowedBy;
        private User mailResponsable;
        private string demande;
        private string purpose;
        private string comment;
        private Response mailResponse;
        private string link;

        public Mail()
        {
            mailSender = new Sender();
            mailService = new Service();
            mailFollowedBy = new User();
            mailResponsable = new User();
            mailResponse = new Response();
        }

        public int Id { get => id; set => id = value; }
        public string MailRef { get => mailRef; set => mailRef = value; }
        public string FolderRef { get => folderRef; set => folderRef = value; }
        public Sender MailSender { get => mailSender; set => mailSender = value; }
        public string Type { get => type; set => type = value; }
        public string Nature { get => nature; set => nature = value; }
        public string WrittenIn { get => writtenIn; set => writtenIn = value; }
        public string ArrivedIn { get => arrivedIn; set => arrivedIn = value; }
        public string CreatedIn { get => createdIn; set => createdIn = value; }
        public Service MailService { get => mailService; set => mailService = value; }
        public User MailFollowedBy { get => mailFollowedBy; set => mailFollowedBy = value; }
        public User MailResponsable { get => mailResponsable; set => mailResponsable = value; }
        public string Demande { get => demande; set => demande = value; }
        public string Purpose { get => purpose; set => purpose = value; }
        public string Comment { get => comment; set => comment = value; }
        public Response MailResponse { get => mailResponse; set => mailResponse = value; }
        public string Link { get => link; set => link = value; }
    }
}