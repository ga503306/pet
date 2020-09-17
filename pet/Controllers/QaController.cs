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
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using pet.Fillter;
using pet.Filter;
using pet.Models;
using pet.Security;
using WebApplication1.Models;


namespace pet.Controllers
{
    [RoutePrefix("api/Qa")]
    public class QaController : ApiController
    {
        private Model1 db = new Model1();

        // Post: api/Qa/PostQuestion
        [JwtAuthFilter]
        [Route("PostQuestion")]
        [HttpPost]
        public IHttpActionResult PostQuestion(Question question)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);

            string user = userseq.Substring(0, 1);
            if (user == "C")
                return Ok(new
                {
                    result = "廠商無法發問"
                });

            question.memberseq = userseq;
            question.del_flag = "N";
            question.postday = DateTime.Now;
            db.Question.Add(question);
            db.SaveChanges();

            return Ok(new
            {
                result = "發問成功"
            });
        }

        // Post: api/Qa/PostAnswer 廠商後台 回覆 要傳queseq
        [JwtAuthFilter]
        [Route("PostAnswer")]
        [HttpPost]
        public IHttpActionResult PostAnswer(QuestionAnswer questionAnswer)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);

            string user = userseq.Substring(0, 1);
            if (user == "M")
                return Ok(new
                {
                    result = "會員無法回答"
                });
            questionAnswer.companyseq = userseq;
            questionAnswer.del_flag = "N";
            questionAnswer.postday = DateTime.Now;
            db.QuestionAnswer.Add(questionAnswer);
            db.SaveChanges();

            return Ok(new
            {
                result = "回覆成功"
            });
        }

        // Get: api/Qa/GetQuestion 廠商後台 拿問與答清單 state:
        [JwtAuthFilter]
        [Route("GetQuestion")]
        [HttpGet]
        public IHttpActionResult GetQuestion(string state = null)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);

            string user = userseq.Substring(0, 1);
            if (user == "M")
                return Ok(new
                {
                    result = "會員無法查看"
                });

            List<Question> questions = db.Question.Where(x => x.companyseq == userseq).ToList();
            if (state == "未回覆")
                questions = questions.Where(x => !x.QuestionAnswer.Any()).ToList();

            return Ok(new
            {
                question = questions.Select(x => new
                {
                    x.queseq,
                    membername = GetMemberName(x.memberseq),
                    x.message,
                    x.postday
                })
            });
        }

        public string GetMemberName(string seq)
        {
            string name = db.Member.Find(seq).membername;
            return name;
        }
    }
}
