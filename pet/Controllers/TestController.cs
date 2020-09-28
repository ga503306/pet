using Microsoft.AspNet.SignalR;
using pet.Hubs;
using pet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace pet.Controllers
{
    [RoutePrefix("api/Test")]
    public class TestController : ApiController
    {
        private Model1 db = new Model1();

        // Get: api/Test/OrderState //修改訂單狀態
        [Route("OrderState")]
        [HttpPost]
        public IHttpActionResult OrderState_(OrderState orderstate)
        {
            if (orderstate.orderseq != null)
            {
                Order order = db.Order.Find(orderstate.orderseq);
                order.state = orderstate.state;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                List<Order> orders = db.Order.Where(x => x.state == (int)Orderstate.已取消).ToList();
                foreach (Order o in orders)
                {
                    o.state = orderstate.state;
                    db.Entry(o).State = EntityState.Modified;
                }

                db.SaveChanges();
            }
            return Ok(new
            {
                result = "修改成功"
            });
        }

        // Post: api/Test/Question //會員問 demo用
        [Route("Question")]
        [HttpPost]
        public IHttpActionResult Question_(Question question)
        {
            question.roomseq = "R202009250005";
            question.memberseq = "M202009250001";
            question.companyseq = "C202009250003";
            question.message = "測試問題";
            question.del_flag = "N";
            question.postday = DateTime.Now;
            db.Question.Add(question);
            db.SaveChanges();
            //signalr即時通知
            Utility.signalR_notice(question.memberseq, question.companyseq, question.queseq, "", Noticetype.問通知);
            var context = GlobalHost.ConnectionManager.GetHubContext<DefaultHub>();
            var connectid = db.Signalr.Where(x => x.whoseq == question.companyseq).Select(x => x.connectid).ToList();//需要通知的廠商signalr connectid
            foreach (var c in connectid)
            {
                context.Clients.Client(c).Get();
            }
            return Ok(new
            {
                result = "ok"
            });
        }

        // Post: api/Test/Order //客戶下單
        [Route("Order")]
        [HttpPost]
        public IHttpActionResult Order(Order order)
        {
            order.companyseq = "C202009250003";
            order.companyname = "火箭隊寵物旅館";
            order.roomseq = "R202009250016";
            order.roomname = "火箭寵物豪華房 火箭房";
            order.memberseq = "M202009250001"; //登入者 流水號
            order.country = "臺北市";
            order.area = "內湖區";
            order.address = "麗山街348巷25-1號";
            order.name = "黃子庭";
            order.tel = "0939113380";
            order.pettype = "米克斯";
            order.petsize = 1;
            order.petamount = 1;
            order.roomprice = 700;
            order.roomamount_amt = 300;
            order.medicine_infeed = true;
            order.medicine_infeed_amt = 30;
            order.medicine_paste = false;
            order.medicine_paste_amt = 40;
            order.medicine_pill = false;
            order.medicine_pill_amt = 60;
            order.bath = true;
            order.bath_amt = 30;
            order.hair = false;
            order.hair_amt = 0;
            order.nails = false;
            order.nails_amt = 0;
            order.state = 1; //已付款
            order.amt = 2160;
            order.orderdates = Convert.ToDateTime("2020-10-22");
            order.orderdatee = Convert.ToDateTime("2020-10-24");
            order.memo = "demo用測試單";
            order.postday = DateTime.Now;
            order.del_flag = "N";
            db.Order.Add(order);
            db.SaveChanges();
            //signalr即時通知
            Utility.signalR_notice(order.memberseq, order.companyseq, order.orderseq, "", Noticetype.下單通知);
            var context = GlobalHost.ConnectionManager.GetHubContext<DefaultHub>();
            var connectid = db.Signalr.Where(x => x.whoseq == order.companyseq).Select(x => x.connectid).ToList();//需要通知的廠商signalr connectid
            foreach (var c in connectid)
            {
                context.Clients.Client(c).Get();
            }
            return Ok(new
            {
                result = "ok"
            });
        }

        // Post: api/Test/Order_del //刪除測試單
        [Route("Order_del")]
        [HttpPost]
        public IHttpActionResult Order_del()
        {
            List<Order> order = db.Order.Where(x=>x.memo == "demo用測試單").ToList();
            db.Order.RemoveRange(order);
            db.SaveChanges();
            return Ok(new
            {
                result = "ok"
            });
        }

        public class OrderState
        {
            public string orderseq { get; set; }
            public int state { get; set; }
        }
    }
}
