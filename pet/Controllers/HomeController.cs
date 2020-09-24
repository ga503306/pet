using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return RedirectPermanent("http://pettrip.rocket-coding.com/index.html#/MemberBackstage");
            //return View();
        }
        public ActionResult TestView()
        {
            ViewBag.Title = "Home Page";
            //return RedirectPermanent("http://pettrip.rocket-coding.com/index.html#/MemberBackstage");
            return View();
        }
    }
}
