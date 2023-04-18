using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mailing.Models;
using Mailing.DAO;
using Mailing.Business;
using Mailing.DbAccess;


namespace Mailing.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return Redirect("~/User/Login");

        }

    }
}