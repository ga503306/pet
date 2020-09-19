using pet.Filter;
using pet.Models;
using pet.Security;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
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
            if (temp == "C")
            {

                var linq = db.Order.Where(x => x.companyseq == userseq && (state == 99 ? x.state != 0 : x.state == state));
                if (state == 2)
                    linq = linq.OrderByDescending(x => x.updateday).Skip((page - 1) * paged).Take(paged);
                else
                    linq = linq.OrderByDescending(x => x.postday).Skip((page - 1) * paged).Take(paged);

                List<Order> order = linq.ToList();
                if (order == null)
                {
                    return Ok(new
                    {
                        result = "查無資料"
                    });
                }

                pagination.total = db.Order.Where(x => x.companyseq == userseq && (state == 99 ? x.state != 0 : x.state == state)).Count();
                pagination.count = order.Count;
                pagination.per_page = paged;
                pagination.current_page = page;
                pagination.total_page = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(pagination.total) / Convert.ToDouble(pagination.per_page)));

                var result = new
                {
                    order = order.Select(x => new
                    {
                        x.companyseq,
                        x.roomseq,
                        x.orderseq,
                        orderdates = Convert.ToDateTime(x.orderdates).ToString("yyyy-MM-dd"),
                        orderdatee = Convert.ToDateTime(x.orderdatee).ToString("yyyy-MM-dd"),
                        x.roomname,
                        setdate = Convert.ToDateTime(x.postday).ToString("yyyy-MM-dd HH:mm"),
                        canceldate = Convert.ToDateTime(x.updateday).ToString("yyyy-MM-dd HH:mm"),
                        x.state,
                        btn_Evalution = x.Evalution.Count != 0 && x.state == (int)Orderstate.已完成 ? true : false
                    }),
                    meta = pagination
                };
                return Ok(result);
            }
            //會員
            else
            {
                var linq = db.Order.Where(x => x.memberseq == userseq && (state == 99 ? x.state != 0 : x.state == state));
                if (state == 2)
                    linq = linq.OrderByDescending(x => x.updateday).Skip((page - 1) * paged).Take(paged);
                else
                    linq = linq.OrderByDescending(x => x.postday).Skip((page - 1) * paged).Take(paged);

                List<Order> order = linq.ToList();
                if (order == null)
                {
                    return Ok(new
                    {
                        result = "查無資料"
                    });
                }

                pagination.total = db.Order.Where(x => x.memberseq == userseq && (state == 99 ? x.state != 0 : x.state == state)).Count();
                pagination.count = order.Count;
                pagination.per_page = paged;
                pagination.current_page = page;
                pagination.total_page = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(pagination.total) / Convert.ToDouble(pagination.per_page)));

                var result = new
                {
                    order = order.Select(x => new
                    {
                        x.companyseq,
                        x.roomseq,
                        x.orderseq,
                        orderdates = Convert.ToDateTime(x.orderdates).ToString("yyyy-MM-dd"),
                        orderdatee = Convert.ToDateTime(x.orderdatee).ToString("yyyy-MM-dd"),
                        x.roomname,
                        setdate = Convert.ToDateTime(x.postday).ToString("yyyy-MM-dd HH:mm"),
                        canceldate = Convert.ToDateTime(x.updateday).ToString("yyyy-MM-dd HH:mm"),
                        x.state,
                        btn_Evalution = x.state == (int)Orderstate.已完成 ? true : false,
                        btn_Evalution_readonly = x.Evalution.Count == 0 && x.state == (int)Orderstate.已完成 ? true : false
                    }),
                    meta = pagination
                };
                return Ok(result);
            }
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


            if (order.state == 1 && s.Days > 7)//如果 狀態不是進行中 就不能消單 //訂單-今日 日期大於七天 可消單
                取消訂單鈕 = true;

            s = new TimeSpan(order.orderdatee.Value.Ticks - order.orderdates.Value.Ticks);
            int 寄宿總價 = 0;//寄宿總價
            寄宿總價 += (order.roomprice.Value + order.roomamount_amt.Value * (order.petamount.Value - 1)) * (s.Days + 1); //每間金額 * 天數

            var result = new
            {
                order = new
                {
                    order.orderseq,
                    //state = Enum.Parse(typeof(Orderstate), order.state.Value.ToString()).ToString(),
                    order.roomname,
                    order.companyname,
                    order.country,
                    order.area,
                    order.address,
                    orderdates = Convert.ToDateTime(order.orderdates).ToString("yyyy-MM-dd"),
                    orderdatee = Convert.ToDateTime(order.orderdatee).ToString("yyyy-MM-dd"),
                    roomamt = 寄宿總價,
                    order.state,
                    setdate = Convert.ToDateTime(order.postday).ToString("yyyy-MM-dd HH:mm"),
                    canceldate = Convert.ToDateTime(order.updateday).ToString("yyyy-MM-dd HH:mm"),
                },
                detail = new
                {
                    order.name,
                    order.tel,
                    order.petamount,
                    order.pettype,
                    order.petsize,
                    order.memo,
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
                },
                cancel = new
                {
                    reason = order.OrderCancel.Select(x => x.reason).FirstOrDefault(),
                    memo = order.OrderCancel.Select(x => x.memo).FirstOrDefault()
                }
            };
            return Ok(result);
        }
        // GET: api/Order/Cancelorder 取消訂單
        [JwtAuthFilter]
        [Route("Cancelorder")]
        [HttpPost]
        public IHttpActionResult Cancelorder(OrderCancel orderCancel)
        {

            //拿已登入的流水
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);

            string user = userseq.Substring(0, 1); //C 廠商 M會員


            Order order = db.Order.Find(orderCancel.orderseq);
            TimeSpan s = new TimeSpan(order.orderdates.Value.Ticks - DateTime.Now.Ticks);
            if (s.Days <= 7)//訂單-今日 日期大於七天 不可消單
            {
                return Ok(new
                {
                    result = "七天內無法取消訂單"
                });
            }

            if (order.state == (int)Orderstate.已取消)
            {
                return Ok(new
                {
                    result = "已取消"
                });
            }

            orderCancel.del_flag = "N";
            db.OrderCancel.Add(orderCancel);


            order.state = (int)Orderstate.已取消;
            order.updateday = DateTime.Now; //取消訂單時間
            order.updateseq = userseq;      //取消訂單人
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(new
            {
                result = "取消成功"
            });
        }
    }

}
