using pet.Filter;
using pet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace pet
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public System.ComponentModel.IContainer components = null;
        public System.Timers.Timer timer1;
        private Model1 db = new Model1();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            InitializeComponent();
        }
        protected void Application_BeginRequest()
        {
            if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")
            {
                Response.End();
            }
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Timers.Timer();
            timer1.Interval = 28800000;// 每10分鐘執行一次
            timer1.Enabled = true;
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);
        }
        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Backup));
            thread.Start();
        }

        void Backup()
        {
            List<Order> order = db.Order.Where(x => x.orderdatee < DateTime.Now && x.state == 1).ToList();
            foreach (Order o in order)
            {
                o.state = (int)Orderstate.已完成;
                db.Entry(o).State = EntityState.Modified;
            }
            if (order != null)
            {
                db.SaveChanges();
            }
        }

    }
}
