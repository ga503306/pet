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
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Web.Http;
using System.Web.Http.ValueProviders;

namespace pet.Controllers
{
    [RoutePrefix("api/Evaluation")]
    public class StarController : ApiController
    {
        private Model1 db = new Model1();
        // Post: api/Evaluation/Set //評價
        [JwtAuthFilter]
        [Route("Set")]
        [HttpPost]
        public IHttpActionResult Set(Evalution evalution)
        {
            //拿已登入的流水
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);

            string user = userseq.Substring(0, 1);

            if (user == "C")
            {
                return Ok(new
                {
                    result = "廠商無法評價"
                });
            }

            evalution.del_flag = "N";
            evalution.postday = DateTime.Now;
            ModelState.Clear();
            Validate(evalution);

            db.Evalution.Add(evalution);
            db.SaveChanges();
            return Ok(new
            {
                result = "評價完成"
            });

        }

        // Get: api/Evaluation/Get //評價
        [JwtAuthFilter]
        [Route("Get")]
        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            //拿已登入的流水
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);

            string user = userseq.Substring(0, 1);

            Order order = db.Order.Find(id);
            //Evalution evalution = db.Evalution.Where(x => x.orderseq == id).FirstOrDefault();
            Company company = db.Company.Find(order.companyseq);
            Member member = db.Member.Find(order.memberseq);
            if (user == "C")
            {
              
                return Ok(new
                {
                    company = new
                    {
                        company.companyname,
                        company.companybrand,
                        member.avatar,
                        company.bannerimg,
                        order.roomname,
                        order.orderseq,
                        order.amt,
                        member.membername,
                    },
                    evalution = new
                    {
                        star = order.Evalution.Select(x => x.star).FirstOrDefault(),
                        memo = order.Evalution.Select(x => x.memo).FirstOrDefault()
                    },
                });
            }
            else
            {
                return Ok(new
                {
                    company = new
                    {
                        company.companyname,
                        company.companybrand,
                        company.avatar,
                        company.bannerimg,
                        order.roomname,
                        order.orderseq,
                        order.amt
                    },
                    evalution = new
                    {
                        star = order.Evalution.Select(x => x.star).FirstOrDefault() == null ? 5 : order.Evalution.Select(x => x.star).FirstOrDefault(),
                        memo = order.Evalution.Select(x => x.memo).FirstOrDefault()
                    },
                    state = new
                    {
                        btn_Evalution_readonly = order.Evalution.Count == 0 ? false : true
                    }
                });
            }
        }
    }
}
