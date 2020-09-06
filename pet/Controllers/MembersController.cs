using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using pet.Filter;
using pet.Models;
using pet.Security;
using WebApplication1.Models;

namespace pet.Controllers
{
    [RoutePrefix("api/Member")]
    public class MembersController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/Member/GetOne
        [JwtAuthFilter]
        [Route("GetOne")]
        [HttpGet]
        public IHttpActionResult GetOne()
        {
            //拿已登入的流水
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);

            Member member = db.Member.Find(userseq);
            if (member == null)
            {
                return Ok(new
                {
                    result = "查無資料"
                });

            }
            MemberGetone memberGetone  = new MemberGetone();
            memberGetone.memberseq = member.memberseq;
            memberGetone.membername = member.membername;
            memberGetone.phone = member.phone;
            memberGetone.email = member.email;
            memberGetone.avatar = member.avatar;
            return Ok(memberGetone);
        }

        // Post: api/Member/Register
        [Route("Register")]
        [HttpPost]
        public IHttpActionResult Register(MemberRegisterModel memberRegisterModel)
        {
            string error_message = "Register錯誤，請至伺服器log查詢錯誤訊息";
            try
            {
                //信箱重複
                if (MemberExists(HttpContext.Current.Request.Form["email"]))
                    return Ok(new
                    {
                        result = "信箱重複"
                    });
                //正常流程
                string today = DateTime.Now.ToString("yyyyMMdd");
                using (var transaction1 = db.Database.BeginTransaction())
                {
                    try
                    {
                        Member getseq = db.Member.Where(x => x.memberseq.Contains(today)).OrderByDescending(x => x.memberseq).FirstOrDefault();
                        int seq = getseq is null ? 0000 : Convert.ToInt32((getseq.memberseq.Substring(9, 4)));//流水號
                        Member member = new Member();
                        member.memberseq = "M" + DateTime.Now.ToString("yyyyMMdd") + (seq + 1).ToString("0000");
                        member.membername = memberRegisterModel.membername;
                        member.phone = memberRegisterModel.phone;
                        member.email = memberRegisterModel.email;
                        member.pwdsalt = Utility.CreateSalt(); ;
                        member.pwd = Utility.GenerateHashWithSalt(memberRegisterModel.pwd, member.pwdsalt);
                        member.del_flag = "N";
                        member.avatar = memberRegisterModel.avatar;
                        db.Member.Add(member);
                        db.SaveChanges();
                        transaction1.Commit();
                    }
                    catch (DbUpdateException)
                    {
                        transaction1.Rollback();
                    }
                }

                return Ok(new
                {
                    result = "註冊成功"
                });
            }
            catch (Exception ex)
            {
                Utility.log("會員註冊", ex.ToString());
                return Ok(new
                {
                    result = error_message
                });

            }
        }

        // Post: api/Member/Login
        [Route("Login")]
        [HttpPost]
        public IHttpActionResult Login(CompanyLoginModel companyLoginModel)//companglinemodel與member相同
        {
            if (ModelState.IsValid)
            {
                Member member = GetUser(companyLoginModel.email);
                if (member == null)
                {
                    return Ok(new
                    {
                        result = "登入失敗:查無此信箱，請註冊"
                    });
                }

                bool Validated = ValidateUser(companyLoginModel.pwd, member.pwd, member.pwdsalt);
                if (Validated) //驗證成功 帳密一致true / 驗證失敗false 
                {
                    string userData = JsonConvert.SerializeObject(member);
                    JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
                    string jwtToken = jwtAuthUtil.GenerateToken(companyLoginModel.email, member.memberseq);

                    return Ok(new
                    {
                        result = "登入成功",
                        token = jwtToken,
                        //name = FormsAuthentication.FormsCookieName,
                        //value = encryptedTicket
                    });
                }
                else
                {
                    //判斷 帳號是否被封鎖
                    if (member.del_flag == "Y")
                    {
                        return Ok(new
                        {
                            result = "登入失敗:此信箱已被平台封鎖"
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            result = "登入失敗:信箱或密碼錯誤"
                        });
                    }
                }
            }
            return Ok(new
            {
                result = "信箱或是密碼輸入格式有誤"
            });
        }

        // Patch: api/Member/Resetpwd
        [JwtAuthFilter]
        [HttpPatch]
        [Route("Resetpwd")]
        public IHttpActionResult Resetpwd(Member member_)
        {
            if(string.IsNullOrWhiteSpace(member_.pwd))
            {
                return Ok(new
                {
                    result = "密碼不能為空或空白"
                });
            }
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);
            Member member = db.Member.Find(userseq);
            member.pwd = Utility.GenerateHashWithSalt(member_.pwd, member.pwdsalt);
            db.Entry(member).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(new
            {
                result = "修改成功"
            });
        }

        // Post: api/Member/Uploadimg
        [JwtAuthFilter]
        [HttpPost]
        [Route("Uploadimg")]
        public IHttpActionResult Uploadimg()
        {
            string error_message = "Uploadimg錯誤，請至伺服器log查詢錯誤訊息";
            //拿已登入的流水
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);
            string filename = null;
            //string parameter = HttpContext.Current.Request.Form["parameter"]; //company_Avatar //company_Banner
            try
            {
                //處理圖片
                HttpPostedFile postedFile = HttpContext.Current.Request.Files.Count > 0
                                          ? HttpContext.Current.Request.Files[0]
                                          : null;
                if (postedFile != null)
                {
                    string extension = postedFile.FileName.Split('.')[postedFile.FileName.Split('.').Length - 1];
                    if (String.Compare(extension, "jpg", true) == 0 || String.Compare(extension, "png", true) == 0)
                    {
                        filename = Utility.SaveUpImage(postedFile, "", userseq);
                    }
                    else
                    {
                        error_message = "圖片格式錯誤";
                        throw new Exception("圖片格式錯誤");
                    }
                }
                //補db 存資料庫
                Member member = db.Member.Find(userseq);
                member.avatar = filename;
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();

                return Ok(new
                {
                    result = filename
                });
            }
            catch (Exception ex)
            {
                Utility.log("PatchMember", ex.ToString());
                return Ok(new
                {
                    result = error_message
                });
            }

        }

        #region Function
        private Member GetUser(string email)
        {
            Member member = db.Member.SingleOrDefault(o => o.email == email);
            if (member == null)
            {
                return null;
            }
            return member;
        }

        //跟資料庫比對帳號密碼 回傳廠商物件
        private bool ValidateUser(string pwd, string pwd_db, string salt)
        {
            string saltPassword = Utility.GenerateHashWithSalt(pwd, salt);
            return saltPassword == pwd_db ? true : false;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MemberExists(string email)
        {
            return db.Member.Count(e => e.email == email) > 0;
        }
    }
}