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

        // GET: api/Order/Getorder //後台 訂單列表
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
            List<Order> order = db.Order.Where(x => (temp == "C" ? x.companyseq == userseq : x.memberseq == userseq) && (state == 99 ? x.state != 0 : x.state == state)).OrderBy(x => x.orderseq).Skip((page - 1) * paged).Take(paged).ToList();
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
        // GET: api/Order/Getorder?id //後台 單一訂單
        [JwtAuthFilter]
        [Route("Getorder")]
        [HttpGet]
        public IHttpActionResult Getorder(string id)
        {
            //拿已登入的流水
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);

            string temp = userseq.Substring(0, 1);

            Order order = db.Order.Find(id);
            if (order == null)
            {
                return Ok(new
                {
                    result = "查無資料"
                });
            }

            bool 取消訂單鈕 = false;
            TimeSpan s = new TimeSpan(order.orderdates.Value.Ticks - DateTime.Now.Ticks);
            if (s.Days > 7)//訂單-今日 日期大於七天 可消單
                取消訂單鈕 = true;

            s = new TimeSpan(order.orderdatee.Value.Ticks - order.orderdates.Value.Ticks);
            int 寄宿總價 = 0;//寄宿總價
            寄宿總價 = 寄宿總價 + order.roomprice.Value * s.Days; //每間金額 * 天數

            var result = new
            {
                order = new
                {
                    order.orderseq,
                    //state = Enum.Parse(typeof(Orderstate), order.state.Value.ToString()).ToString(),
                    order.roomname,
                    order.orderdates,
                    order.orderdatee,
                    roomstate = 寄宿總價
                },
                detail = new
                {
                    order.name,
                    order.tel,
                    order.petamount,
                    medicine = new
                    {
                        order.medicine_infeed,
                        order.medicine_infeed_amt,
                        order.medicine_paste,
                        order.medicine_paste_amt,
                        order.medicine_pill,
                        order.medicine_pill_amt,
                    },
                    plus = new
                    {
                        order.bath,
                        order.bath_amt,
                        order.nails,
                        order.nails_amt,
                        order.hair,
                        order.hair_amt
                    },
                    orderprice = order.amt,//訂購總額
                    btn_cancel = 取消訂單鈕
                }
            };
            return Ok(result);
        }
    }
}
