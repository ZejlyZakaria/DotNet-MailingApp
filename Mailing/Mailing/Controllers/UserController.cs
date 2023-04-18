using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mailing.Models;
using Mailing.DAO;
using Mailing.Business;
using Mailing.DbAccess;
using System.Web.Security;
using System.IO;

namespace Mailing.Controllers
{
    public class UserController : Controller
    {
        private static DbConnection dbConnection = new DbConnection();
        private static IServiceDAO serviceDAO = new ServiceDAO(dbConnection);
        private static IUserDAO userDAO = new UserDAO(dbConnection);
        private static IMailDAO mailDAO = new MailDAO(dbConnection);
        private static ISenderDAO senderDAO = new SenderDAO(dbConnection);
        private static IMailingService service = new MailingService(userDAO, serviceDAO, mailDAO, senderDAO);
        public static Boolean isConnected = false;
        private static User currentUser;
        private static string lastView = "Index";
        private static List<User> listU = new List<User>();
        private static List<Mail> listM = new List<Mail>();
        private static List<Sender> listS = new List<Sender>();


        public ActionResult Index()
        {
            if (isConnected)
            {
                currentUser = service.GetUser(currentUser.Id);
                return View(currentUser);
            }
            else
            {
                return Redirect("~/User/Login");

            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection, User u)
        {
            try
            {
                User user = service.AuthenticateUser(u.Email, u.Password);
                if (user != null)
                {
                    currentUser = user;
                    isConnected = true;
                    FormsAuthentication.SetAuthCookie(u.Email, false);
                    return Redirect("~/User/" + lastView);
                }
                else
                {
                    ViewBag.msg = "Erreur d'authentification!";
                    return View();
                }
            }
            catch
            {
                ViewBag.msg = "Erreur d'authentification!";
                return View();
            }
        }

        public ActionResult Users()
        {
            if (isConnected)
            {
                currentUser = service.GetUser(currentUser.Id);
                if (currentUser.IsResponsable && (currentUser.UserService.Name == "Bureau d'ordre"))
                {
                    listU = service.GetUsers();
                    ViewData["listUsers"] = listU;
                    ViewData["listServices"] = service.GetServices();
                    return View(currentUser);
                }
                else if (currentUser.IsResponsable)
                {
                    List<Service> listServices = new List<Service>();
                    listServices.Add(currentUser.UserService);
                    listU = service.GetUsers(currentUser.UserService.Id + "");
                    ViewData["listUsers"] = listU;
                    ViewData["listServices"] = listServices;
                    return View(currentUser);
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                lastView = "Users";
                return Redirect("~/user/login");
            }

        }

        [HttpPost]
        public ActionResult Users(FormCollection collection, User u)
        {
            try
            {
                int result = service.AddUser(u);
                if (result > 0)
                {
                    return Redirect("~/User/Users");
                }
                else
                {
                    return Redirect("~/User/Users");

                }
            }
            catch
            {
                return Redirect("~/User/Users");
            }
        }

        [HttpPost]
        public ActionResult DeleteUser(FormCollection collection, int idUser)
        {
            try
            {
                int result = service.DeleteUser(idUser);
                if (result > 0)
                {
                    ViewBag.result = "Success";
                    return Redirect("~/User/Users");
                }
                else
                {
                    ViewBag.result = "Error";
                    return View("Users");
                }
            }
            catch
            {
                ViewBag.result = "Error";
                return View("Users");
            }
        }

        [HttpPost]
        public ActionResult DeleteMail(FormCollection collection, int idMail)
        {
            try
            {
                int result = service.DeleteMail(idMail);
                if (result > 0)
                {
                    ViewBag.result = "Success";
                    return Redirect("~/User/Mails");
                }
                else
                {
                    ViewBag.result = "Error";
                    return View("Users");
                }
            }
            catch
            {
                ViewBag.result = "Error";
                return View("Mails");
            }
        }

        [HttpPost]
        public ActionResult ModifyUser(FormCollection collection, User model)
        {
            try
            {
                model.Password = currentUser.Password;
                int result = service.UpdateUser(model.Id, model);
                if (result > 0)
                {
                    ViewBag.result = "Success";
                    return Redirect("~/User/Users");
                }
                else
                {
                    ViewBag.result = "Error";
                    return Redirect("~/User/Users");
                }
            }
            catch
            {
                ViewBag.result = "Error";
                return Redirect("~/User/Users");
            }
        }

        [HttpPost]
        public ActionResult ModifyMail(FormCollection collection, Mail model)
        {
            try
            {
                int result = service.UpdateMail(model.Id, model);
                if (result > 0)
                {
                    ViewBag.result = "Success";
                    return Redirect("~/User/Mails");
                }
                else
                {
                    ViewBag.result = "Error";
                    return Redirect("~/User/Mails");
                }
            }
            catch
            {
                ViewBag.result = "Error";
                return Redirect("~/User/Mails");
            }
        }


        [HttpPost]
        public ActionResult ChangePassword(FormCollection collection)
        {
            try
            {
                string newPassword = collection["Password"];
                User u = currentUser;
                u.Password = newPassword;
                int result = service.UpdateUser(currentUser.Id, u);
                if (result > 0)
                {
                    ViewBag.result = "Success";
                    return Redirect("~/User/Index");
                }
                else
                {
                    ViewBag.result = "Error";
                    return Redirect("~/User/Index");
                }
            }
            catch
            {
                ViewBag.result = "Error";
                return Redirect("~/User/Index");
            }
        }

        // MAILS ACTIONS

        [HttpPost]
        public ActionResult AddMail(FormCollection collection, Mail model)
        {
            try
            {
                HttpPostedFileBase file = Request.Files["Link"];
                string filename = Path.GetFileName(file.FileName);
                string filepath = Path.Combine(Server.MapPath("~/assets/files"), filename);
                file.SaveAs(filepath);
                model.Link = filename;
                int result = service.AddMail(model);
                return Redirect("~/User/Mails");
            }
            catch (Exception)
            {
                return Redirect("~/User/Mails");

            }


        }

        public ActionResult Mails()
        {
            if (isConnected)
            {
                currentUser = service.GetUser(currentUser.Id);
                if (currentUser.IsResponsable || (currentUser.UserService.Name == "Bureau d'ordre"))
                {
                    listM = service.GetMails();
                    ViewData["listMails"] = listM;
                    ViewData["listSenders"] = service.GetSenders();
                    ViewData["listUsers"] = service.GetUsers();
                    ViewData["listServices"] = service.GetServices();
                    ViewData["currentUser"] = currentUser;
                    return View(currentUser);
                }
                else if (currentUser.IsResponsable)
                {
                    //List<Service> listServices = new List<Service>();
                    //listServices.Add(currentUser.UserService);
                    //listM = service.GetUsers(currentUser.UserService.Id + "");
                    //ViewData["listUsers"] = listU;
                    //ViewData["listServices"] = listServices;
                    return View(currentUser);
                }
                else
                {
                    listM = service.GetMailsByUser(currentUser.Id);
                    ViewData["listMails"] = listM;
                    ViewData["listSenders"] = service.GetSenders();
                    ViewData["listUsers"] = service.GetUsers();
                    ViewData["listServices"] = service.GetServices();
                    return View(currentUser);
                }
            }
            else
            {
                lastView = "Mails";
                return Redirect("~/User/Login");
            }

        }


        public JsonResult GetUsers(string key)
        {
            List<User> listUsers = new List<User>();
            if (key.Length > 0)
            {
                foreach (User user in listU)
                {
                    string fullName = user.FirstName + user.LastName;
                    if (fullName.ToLower().Contains(key.ToLower()))
                    {
                        listUsers.Add(user);
                    }
                }
                return Json(listUsers, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(listU, JsonRequestBehavior.AllowGet);

            }

        }

        public JsonResult GetCurrentUser()
        {

            return Json(currentUser, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetUsers2(string key)
        {
            return Json(service.GetUsers(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateMail(FormCollection collection, Mail model) {
            int id = model.Id;
            int id2 = model.MailResponsable.Id;
            int result = service.Reaffecter(id, id2);
            if (result > 0)
            {
                return Redirect("~/User/Mails");
            }
            else
            {
                return Redirect("~/User/Mails");
            }

        }

        public JsonResult GetMails(string key)
        {

            List<Mail> listMails = new List<Mail>();
            if (key.Length > 0)
            {
                foreach (Mail mail in listM)
                {
                    string reference = mail.MailRef;
                    if (reference.ToLower().Contains(key.ToLower()))
                    {
                        listMails.Add(mail);
                    }
                }
                return Json(listMails, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(listM, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        public ActionResult AddFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string filepath = Path.Combine(Server.MapPath("~/assets/files"),filename);
                    file.SaveAs(filepath);
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Logout()
        {
            currentUser = null;
            isConnected = false;
            return Redirect("~/User/Index");
        }

    }
}
