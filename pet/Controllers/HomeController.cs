using pet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace pet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return RedirectPermanent("/index.html");
            //return View();
        }

        /// <summary>
        /// 用來接金流後 Post 回來的方法
        /// 並且導向訂單後台頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult RedirectMemberBackstage()
        {
            return RedirectPermanent("/index.html#/MemberBackstage");
        }

        private Model1 db = new Model1();
        public ActionResult TestView()
        {
            ViewBag.Title = "Home Page";
            var a = db.Member.FirstOrDefault();
            Utility.log("TestView :", "on");
            //return RedirectPermanent("http://pettrip.rocket-coding.com/index.html#/MemberBackstage");
            return View(a);
        }
    }
}
