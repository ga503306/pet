using pet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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

        public class OrderState
        {
            public string orderseq { get; set; }
            public int state { get; set; }
        }
    }
}
