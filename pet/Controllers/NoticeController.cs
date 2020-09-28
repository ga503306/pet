using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using pet.Fillter;
using pet.Filter;
using pet.Hubs;
using pet.Models;
using pet.Security;
using WebApplication1.Models;


namespace pet.Controllers
{
    [RoutePrefix("api/Notice")]
    public class NoticeController : ApiController
    {
        private Model1 db = new Model1();

        IHubContext hub = GlobalHost.ConnectionManager.GetHubContext<DefaultHub>();

        // GET: api/Notice/GetNotice
        [JwtAuthFilter]
        [Route("GetNotice")]
        [HttpGet]
        public IHttpActionResult GetNotice()
        {
            //拿已登入的流水
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);
            string user = userseq.Substring(0, 1);

            var unread = db.Notice.Where(x => x.toseq == userseq).ToList();
            unread = unread.Where(x => x.state == Convert.ToBoolean(Noticestate.未讀)).ToList();

            List<Notice> notices = db.Notice.Where(x => x.toseq == userseq).OrderBy(x => x.state).ThenByDescending(x => x.postday).Take(10).ToList();
            var result = new
            {
                unread = unread.Count(),
                notices = notices.Select(
                   x => new
                   {
                       x.noticeseq,
                       x.fromseq,
                       x.toseq,
                       state = Enum.Parse(typeof(Noticestate), x.state.GetHashCode().ToString()).ToString(),
                       x.text,
                       type = Enum.Parse(typeof(Noticetype), x.type.ToString()).ToString(),
                       time = Convert.ToDateTime(x.postday).ToString("yyyy-MM-dd HH:mm")
                   })
            };

            return Ok(result);
        }

        // GET: api/Notice/Readall
        [JwtAuthFilter]
        [Route("Readall")]
        [HttpGet]
        public IHttpActionResult Readall ()
        {
            //拿已登入的流水
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);
            string user = userseq.Substring(0, 1);
            var notices = db.Notice.Where(x => x.toseq == userseq).ToList();
            notices = notices.Where(x => x.state == Convert.ToBoolean(Noticestate.未讀)).ToList();

            foreach (var n in notices)
            {
                n.state = Convert.ToBoolean(Noticestate.已讀);
                db.Entry(n).State = EntityState.Modified;
            }
            db.SaveChanges();

            return Ok(new
            {
                result = "已讀"
            });
        }

        // Post: api/Notice/Readone
        [JwtAuthFilter]
        [Route("Readone")]
        [HttpPost]
        public IHttpActionResult Readone(Notice notice)
        {
            Notice notice_ = db.Notice.Find(notice.noticeseq);
            notice_.state = Convert.ToBoolean(Noticestate.已讀);
            db.Entry(notice_).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(new
            {
                result = "已讀"
            });
        }

        // Post: api/Notice/Sendid
        [JwtAuthFilter]
        [Route("Sendid")]
        [HttpPost]
        public IHttpActionResult Sendid(Signalr signalr)
        {
            //拿已登入的流水
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);
            string user = userseq.Substring(0, 1);

            

            signalr.whoseq = userseq;
            signalr.postday = DateTime.Now;

            db.Signalr.Add(signalr);
            db.SaveChanges();
            return Ok(new
            {
                result = "連線加入成功"
            });
        }
    }
}
