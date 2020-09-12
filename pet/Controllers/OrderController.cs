using pet.Filter;
using pet.Models;
using pet.Security;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace pet.Controllers
{
    [RoutePrefix("api/Order")]
    public class OrderController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/Order/Getorder //廠商後台 訂單列表
        [JwtAuthFilter]
        [Route("Getorder")]
        [HttpGet]
        public IHttpActionResult Getorder(int state = 99, int page = 1, int paged = 6)
        {
            Pagination pagination = new Pagination();
            //拿已登入的流水
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);

            string temp = userseq.Substring(0, 1);
           
            //廠商
            //if (temp == "C")
           // {
                List<Order> order = db.Order.Where(x => (temp == "C" ?  x.companyseq == userseq  : x.memberseq == userseq) && (state == 99 ? x.state != 0 : x.state == state)).OrderBy(x => x.orderseq).Skip((page - 1) * paged).Take(paged).ToList();
                if (order == null)
                {
                    return Ok(new
                    {
                        result = "查無資料"
                    });
                }

                pagination.total = db.Order.Where(x => (temp == "C" ? x.companyseq == userseq : x.memberseq == userseq) && (state == 99 ? x.state != 0 : x.state == state)).Count();
                pagination.count = order.Count;
                pagination.per_page = paged;
                pagination.current_page = page;
                pagination.total_page = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(pagination.total) / Convert.ToDouble(pagination.per_page)));

                var result = new
                {
                    order = order.Select(x => new
                    {
                        x.orderseq,
                        orderdates = Convert.ToDateTime(x.orderdates).ToString("yyyy-MM-dd"),
                        orderdatee = Convert.ToDateTime(x.orderdatee).ToString("yyyy-MM-dd"),
                        x.roomname,
                        x.state
                    }),
                    meta = pagination
                };
                return Ok(result);
            //}
            //會員
            //else
            //{
            //    List<Order> order = db.Order.Where(x => x.memberseq == userseq && x.state == state).OrderBy(x => x.orderseq).Skip((page - 1) * paged).Take(paged).ToList();
            //    if (order == null)
            //    {
            //        return Ok(new
            //        {
            //            result = "查無資料"
            //        });
            //    }

            //    pagination.total = db.Order.Where(x => x.memberseq == userseq && x.state == state).Count();
            //    pagination.count = order.Count;
            //    pagination.per_page = paged;
            //    pagination.current_page = page;
            //    pagination.total_page = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(pagination.total) / Convert.ToDouble(pagination.per_page)));

            //   var result = new
            //    {
            //        order = order.Select(x => new
            //        {
            //            x.orderseq,
            //            orderdates = Convert.ToDateTime(x.orderdates).ToString("yyyy-MM-dd"),
            //            orderdatee = Convert.ToDateTime(x.orderdatee).ToString("yyyy-MM-dd"),
            //            x.roomname,
            //            x.state
            //        }),
            //        meta = pagination
            //    };
            //    return Ok(result);
            //}

           
        }
    }
}
