using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
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
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
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

            //signalr即時通知
            Utility.signalR_notice(question.memberseq, question.companyseq, question.queseq, "", Noticetype.問通知);
            var context = GlobalHost.ConnectionManager.GetHubContext<DefaultHub>();
            var connectid = db.Signalr.Where(x => x.whoseq == question.companyseq).Select(x => x.connectid).ToList();//需要通知的廠商signalr connectid
            foreach (var c in connectid)
            {
                context.Clients.Client(c).Get();
            }
            //context.Clients.All.Get();

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

            //var questisonAnswer_ = db.QuestionAnswer.Where(x => x.Question.queseq == questionAnswer.queseq).FirstOrDefault();
            var question = db.Question.Where(x => x.queseq == questionAnswer.queseq).ToList();
            QuestionAnswer questionAnswer_ = db.QuestionAnswer.Find(questionAnswer.ansseq);

            //signalr即時通知
            Utility.signalR_notice(questionAnswer_.companyseq, questionAnswer_.Question.memberseq, questionAnswer_.ansseq, "", Noticetype.答通知);
            var context = GlobalHost.ConnectionManager.GetHubContext<DefaultHub>();
            var connectid = db.Signalr.Where(x => x.whoseq == questionAnswer_.Question.memberseq).Select(x => x.connectid).ToList();//需要通知的會員signalr connectid
            foreach (var c in connectid)
            {
                context.Clients.Client(c).Get();
            }
            return Ok(new
            {
                result = "回覆成功"
            });
        }

        // Get: api/Qa/GetQuestion 後台 拿問與答清單 state:
        [JwtAuthFilter]
        [Route("GetQuestion")]
        [HttpGet]
        public IHttpActionResult GetQuestion(string state = null, int page = 1, int paged = 6)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);

            string user = userseq.Substring(0, 1);
            //會員
            if (user == "M")
            {
                var questions = db.Question.Where(x => x.memberseq == userseq);
                if (state == ((int)Qastate.未回覆).ToString())
                    questions = questions.Where(x => !x.QuestionAnswer.Any());
                else if (state == ((int)Qastate.已回覆).ToString())
                    questions = questions.Where(x => x.QuestionAnswer.Any());

                questions = questions.OrderByDescending(x => x.postday);
                //分頁
                Pagination pagination = new Pagination();
                var questions_ = questions.Skip((page - 1) * paged).Take(paged).ToList();

                pagination.total = questions.Count();
                pagination.count = questions_.Count;
                pagination.per_page = paged;
                pagination.current_page = page;
                pagination.total_page = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(pagination.total) / Convert.ToDouble(pagination.per_page)));


                return Ok(new
                {
                    question = questions_.Select(x => new
                    {
                        x.queseq,
                        membername = GetMemberName(x.memberseq),
                        x.roomseq,
                        roomname = GetRoomName(x.roomseq),
                        x.companyseq,
                        state = x.QuestionAnswer.Any() == true ? Qastate.已回覆.ToString() : Qastate.未回覆.ToString(),
                        x.message,
                        postday = x.postday.Value.ToString("yyyy-MM-dd HH:mm:ss")
                    }),
                    meta = pagination
                });
            }
            //廠商
            else
            {
                var questions = db.Question.Where(x => x.companyseq == userseq);
                if (state == ((int)Qastate.未回覆).ToString())
                    questions = questions.Where(x => !x.QuestionAnswer.Any());
                else if (state == ((int)Qastate.已回覆).ToString())
                    questions = questions.Where(x => x.QuestionAnswer.Any());

                questions = questions.OrderByDescending(x => x.postday);
                //分頁
                Pagination pagination = new Pagination();
                var questions_ = questions.Skip((page - 1) * paged).Take(paged).ToList();

                pagination.total = questions.Count();
                pagination.count = questions_.Count;
                pagination.per_page = paged;
                pagination.current_page = page;
                pagination.total_page = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(pagination.total) / Convert.ToDouble(pagination.per_page)));


                return Ok(new
                {
                    question = questions_.Select(x => new
                    {
                        x.queseq,
                        membername = GetMemberName(x.memberseq),
                        x.roomseq,
                        roomname = GetRoomName(x.roomseq),
                        x.companyseq,
                        state = x.QuestionAnswer.Any() == true ? Qastate.已回覆.ToString() : Qastate.未回覆.ToString(),
                        x.message,
                        postday = x.postday.Value.ToString("yyyy-MM-dd HH:mm:ss")
                    }),
                    meta = pagination
                });
            }
        }

        // Get: api/Qa/GetQuestionDetail 後台 拿問與答清單 詳細 state:
        [JwtAuthFilter]
        [Route("GetQuestionDetail")]
        [HttpGet]
        public IHttpActionResult GetQuestionDetail(string queseq)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);

            string user = userseq.Substring(0, 1);
            //會員 可合併 好像做同一件事
            if (user == "M")
            {
                var question = db.Question.Find(queseq);

                return Ok(new
                {
                    question.queseq,
                    name = GetMemberName(question.memberseq),
                    question = question.message,
                    question_date = question.postday.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    answer = question.QuestionAnswer.Select(y => y.message).FirstOrDefault() is null ? "" : question.QuestionAnswer.Select(y => y.message).FirstOrDefault(),
                    answer_date = question.QuestionAnswer.Select(y => y.postday).FirstOrDefault() is null ? "" : question.QuestionAnswer.Select(y => y.postday).FirstOrDefault().Value.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
            //廠商
            else
            {
                var question = db.Question.Find(queseq);
                return Ok(new
                {
                    question.queseq,
                    name = GetMemberName(question.memberseq),
                    question = question.message,
                    question_date = question.postday.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    answer = question.QuestionAnswer.Select(y => y.message).FirstOrDefault() is null ? "" : question.QuestionAnswer.Select(y => y.message).FirstOrDefault(),
                    answer_date = question.QuestionAnswer.Select(y => y.postday).FirstOrDefault() is null ? "" : question.QuestionAnswer.Select(y => y.postday).FirstOrDefault().Value.ToString("yyyy-MM-dd HH:mm:ss")

                });
            }
        }

        public string GetMemberName(string seq)
        {
            string name = db.Member.Find(seq).membername;
            return name;
        }
        public string GetRoomName(string seq)
        {
            string name = db.Room.Find(seq).roomname;
            return name;
        }
    }
}
