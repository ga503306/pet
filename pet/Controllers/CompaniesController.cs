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
    [RoutePrefix("api/Company")]
    public class CompaniesController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/Company/GetAll
        [JwtAuthFilter]
        [Route("GetAll")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return Ok(db.Company);
        }

        // GET: api/Company/GetOne
        [JwtAuthFilter]
        [Route("GetOne")]
        [HttpGet]
        public IHttpActionResult GetOne()//改token不是id
        {
            //拿已登入的流水
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);

            Company company = db.Company.Find(userseq);
            if (company == null)
            {
                return Ok(new
                {
                    result = "查無資料"
                });

            }
            CompanyGetone companyGetone = new CompanyGetone();
            companyGetone.companyseq = company.companyseq;
            companyGetone.companyname = company.companyname;
            companyGetone.companybrand = company.companybrand;
            companyGetone.phone = company.phone;
            companyGetone.email = company.email;
            companyGetone.country = company.country;
            companyGetone.area = company.area;
            companyGetone.address = company.address;
            companyGetone.pblicense = company.pblicense;
            if (company.effectivedate.HasValue)
                companyGetone.effectivedate = Convert.ToDateTime(company.effectivedate.Value).ToString("yyyy-MM-dd");
            companyGetone.introduce = company.introduce;
            companyGetone.morning = Convert.ToBoolean(company.morning);
            companyGetone.afternoon = Convert.ToBoolean(company.afternoon);
            companyGetone.night = Convert.ToBoolean(company.night);
            companyGetone.midnight = Convert.ToBoolean(company.midnight);
            companyGetone.avatar = company.avatar;
            companyGetone.bannerimg = company.bannerimg;
            //#region 讀取圖片banner
            //if (!Directory.Exists(HttpContext.Current.Server.MapPath("/") + @"/Images/company_Banner/" + company.companyseq))
            //{
            //    //新增資料夾
            //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/") + @"/Images/company_Banner/" + company.companyseq);
            //}
            //DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapPath("/") + @"/Images/company_Banner/" + company.companyseq);
            //FileInfo[] fi = di.GetFiles();

            //DataTable dt = new DataTable();
            //DataColumn dcFilename = new DataColumn("strFilename", Type.GetType("System.String"));
            //dt.Columns.Add(dcFilename);
            //Object[] data = new object[1];

            //foreach (FileInfo file in fi)
            //{
            //    data[0] = file.Name;
            //    dt.Rows.Add(data);
            //}
            //#endregion
            ////banner 圖片
            //if (dt.Rows.Count > 0)
            //    companyGetone.imgurl = "http://pettrip.rocket-coding.com/Images/company_Banner/" + company.companyseq + "/" + dt.Rows[0]["strFilename"];
            //#region 讀取圖片廠商大頭
            //if (!Directory.Exists(HttpContext.Current.Server.MapPath("/") + @"/Images/company_Avatar/" + company.companyseq))
            //{
            //    //新增資料夾
            //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/") + @"/Images/company_Avatar/" + company.companyseq);
            //}
            //di = new DirectoryInfo(HttpContext.Current.Server.MapPath("/") + @"/Images/company_Avatar/" + company.companyseq);
            //fi = di.GetFiles();

            //dt = new DataTable();
            //dcFilename = new DataColumn("strFilename", Type.GetType("System.String"));
            //dt.Columns.Add(dcFilename);
            //data = new object[1];

            //foreach (FileInfo file in fi)
            //{
            //    data[0] = file.Name;
            //    dt.Rows.Add(data);
            //}
            //#endregion
            //if (dt.Rows.Count > 0)
            //    companyGetone.avatar = "http://pettrip.rocket-coding.com/Images/company_Avatar/" + company.companyseq + "/" + dt.Rows[0]["strFilename"];


            return Ok(companyGetone);
        }

        // Patch: api/Company/Patchcompany
        [JwtAuthFilter]
        [HttpPatch]
        [Route("Patchcompany")]
        public IHttpActionResult Patchcompany(CompanyPatch companyPatch)
        {    //CompanyPatch companyPatch
            string error_message = "patch錯誤，請至伺服器log查詢錯誤訊息";
            //拿已登入的流水
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);

            if (ModelState.IsValid)
            {
                using (var transaction1 = db.Database.BeginTransaction())
                {
                    try
                    {
                        Company company = db.Company.Find(userseq);
                        company.introduce = companyPatch.introduce;
                        company.morning = Convert.ToBoolean(companyPatch.morning);
                        company.afternoon = Convert.ToBoolean(companyPatch.afternoon);
                        company.night = Convert.ToBoolean(companyPatch.night);
                        company.midnight = Convert.ToBoolean(companyPatch.midnight);
                        company.bannerimg = companyPatch.bannerimg;
                        db.Entry(company).State = EntityState.Modified;
                        db.SaveChanges();
                        transaction1.Commit();
                    }
                    catch (Exception ex)//失敗
                    {
                        transaction1.Rollback();
                        Utility.log("Patchcompany", ex.ToString());
                        return Ok(new
                        {
                            result = error_message//修改失敗
                        });
                    }
                    return Ok(new
                    {
                        result = "修改成功"
                    });
                }
            }
            else
            {
                return Ok(new
                {
                    result = ModelState
                });
            }
        }

        // Patch: api/Company/Resetpwd
        [JwtAuthFilter]
        [HttpPatch]
        [Route("Resetpwd")]
        public IHttpActionResult Resetpwd(Company company_)
        {
            if (string.IsNullOrWhiteSpace(company_.pwd))
            {
                return Ok(new
                {
                    result = "密碼不能為空或空白"
                });
            }
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);
            Company company = db.Company.Find(userseq);
            company.pwd = Utility.GenerateHashWithSalt(company_.pwd, company.pwdsalt);
            db.Entry(company).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(new
            {
                result = "修改成功"
            });
        }

        // Post: api/Company/Register
        [Route("Register")]
        [HttpPost]
        public IHttpActionResult Register(CompanyRegisterModel companyRegisterModel)
        {
            string error_message = "Register錯誤，請至伺服器log查詢錯誤訊息";
            //格式
            if (!ModelState.IsValid)
            {
                return Ok(new
                {
                    result = "格式錯誤"
                });
            }
            JArray jArray = Utility.getjson("https://paim.coa.gov.tw/api/BusinessList?IsActive=1");
            bool PBL_flag = false;//證書號是否有在名單上
            foreach (var item in jArray)
            {
                if (item["PBLicense"].ToString() == companyRegisterModel.pblicense) {
                    if (Convert.ToDateTime(item["EffectiveDate"]) == Convert.ToDateTime(companyRegisterModel.effectivedate))
                    {
                        PBL_flag = true;
                    }
                    break;
                }
            }
            //證書號
            if (!PBL_flag)
            {
                return Ok(new
                {
                    result = "證書號不在合法寵物業者名單上"
                });
            }
           
            try
            {
                //信箱重複
                if (companyExists(companyRegisterModel.email))
                    return Ok(new
                    {
                        result = "信箱重複"
                    });
                //正常流程
                using (var transaction1 = db.Database.BeginTransaction())
                {
                    string today = DateTime.Now.ToString("yyyyMMdd");
                    Company getseq = db.Company.Where(x => x.companyseq.Contains(today)).OrderByDescending(x => x.companyseq).FirstOrDefault();
                    int seq = getseq is null ? 0000 : Convert.ToInt32((getseq.companyseq.Substring(9, 4)));//流水號

                    Company company = new Company();
                    company.companyseq = "C" + DateTime.Now.ToString("yyyyMMdd") + (seq + 1).ToString("0000");
                    company.companyname = companyRegisterModel.companyname;
                    company.companybrand = companyRegisterModel.companybrand;
                    company.phone = companyRegisterModel.phone;
                    company.email = companyRegisterModel.email;
                    company.pwdsalt = Utility.CreateSalt(); ;
                    company.pwd = Utility.GenerateHashWithSalt(companyRegisterModel.pwd, company.pwdsalt);
                    company.country = companyRegisterModel.country;
                    company.area = companyRegisterModel.area;
                    company.address = companyRegisterModel.address;
                    company.pblicense = companyRegisterModel.pblicense;
                    company.effectivedate = companyRegisterModel.effectivedate;
                    company.avatar = companyRegisterModel.avatar;
                    company.state = true; //狀態1 通過
                    company.del_flag = "N";
                    db.Company.Add(company);
                    db.SaveChanges();
                    transaction1.Commit();
                }

                return Ok(new
                {
                    result = "註冊成功"
                });
            }
            catch (Exception ex)
            {
                Utility.log("廠商註冊", ex.ToString());
                return Ok(new
                {
                    result = error_message
                });

            }
        }

        // Post: api/Company/Login
        [Route("Login")]
        [HttpPost]
        public IHttpActionResult Login(CompanyLoginModel companyLoginModel)
        {
            if (ModelState.IsValid)
            {
                Company company = GetUser(companyLoginModel.email);
                if (company == null)
                {
                    return Ok(new
                    {
                        result = "登入失敗:查無此信箱，請註冊"
                    });
                }

                bool Validated = ValidateUser(companyLoginModel.pwd, company.pwd, company.pwdsalt);
                if (Validated) //驗證成功 帳密一致true / 驗證失敗false 
                {
                    //string userData = JsonConvert.SerializeObject(company);
                    ////Utility.SetAuthenTicket(userData, companyLoginModel.email);
                    ////宣告一個驗證票
                    //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, companyLoginModel.email, DateTime.Now, DateTime.Now.AddHours(3), false, userData);
                    ////加密驗證票
                    //string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                    ////建立Cookie
                    //HttpCookie authenticationcookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    ////將Cookie寫入回應
                    //HttpContext.Current.Response.Cookies.Add(authenticationcookie);

                    string userData = JsonConvert.SerializeObject(company);
                    JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
                    string jwtToken = jwtAuthUtil.GenerateToken(companyLoginModel.email, company.companyseq);

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
                    if (company.del_flag == "Y")
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

        // Post: api/Company/Logout
        [Route("Logout")]
        [HttpPost]
        public IHttpActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return Ok(new
            {
                result = "登出成功"
            });
        }

        // Post: api/Company/Uploadimg
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
                Company company = db.Company.Find(userseq);
                company.avatar = filename;
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();

                return Ok(new
                {
                    result = filename
                });
            }
            catch (Exception ex)
            {
                Utility.log("Patchcompany", ex.ToString());
                return Ok(new
                {
                    result = error_message
                });
            }

        }
        // string company  member 廠商或會員  頭貼路徑
        #region Function
        private Company GetUser(string email)
        {
            Company company = db.Company.SingleOrDefault(o => o.email == email);
            if (company == null)
            {
                return null;
            }
            return company;
        }

        //跟資料庫比對帳號密碼 回傳廠商物件
        private bool ValidateUser(string pwd, string pwd_db, string salt)
        {
            string saltPassword = Utility.GenerateHashWithSalt(pwd, salt);
            return saltPassword == pwd_db ? true : false;
        }

        #endregion

        #region test
        public HttpResponseMessage PostUserImage()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            //string _userdata = jwtAuthUtil.Getuserdata(token);
            //Company userdata = JsonConvert.DeserializeObject<Company>(_userdata);
            //try
            //{
            //    var postedFile = HttpContext.Current.Request.Files.Count > 0
            //        ? HttpContext.Current.Request.Files[0]
            //        : null;
            //    if (postedFile != null && postedFile.ContentLength > 0)
            //    {
            //        //string extension = postedFile.FileName.Split('.')[postedFile.FileName.Split('.').Length - 1];
            //        //int MaxContentLength = 1024 * 1024 * 1; //Size = 1MB
            //        string fileName = Utility.SaveUpImage(postedFile);
            //        //IList<string> AllowedFileExtensions = new List<string> {".jpg", ".png", ".svg"};
            //        //if (!AllowedFileExtensions.Contains(extension))
            //        //{
            //        //    return Request.CreateResponse(HttpStatusCode.BadRequest, new
            //        //    {
            //        //        success = false,
            //        //        message = "請上傳圖片正確格式，可接受格式為 .jpg, .png, .svg"
            //        //    });
            //        //}
            //        //產生圖片連結
            //        UriBuilder uriBuilder = new UriBuilder(HttpContext.Current.Request.Url)
            //        {
            //            Path = $"/Upload/Userimg/{fileName}"
            //        };
            //        //Userimage myfolder name where i want to save my image
            //        Uri imageUrl = uriBuilder.Uri;
            //        member.manpic = imageUrl.ToString();
            //        db.Entry(member).State = EntityState.Modified;
            //        db.SaveChanges();
            //        return Request.CreateResponse(HttpStatusCode.OK, new
            //        {
            //            success = true,
            //            message = "已上傳個人圖片",
            //            imageUrl
            //        });
            //    }
            //    return Request.CreateResponse(HttpStatusCode.NotFound, new
            //    {
            //        success = false,
            //        message = "無圖片，請選擇圖片上傳"
            //    });
            //}
            //catch
            //{
            //    throw;
            //}
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion

        //// GET: api/Companies
        //public IQueryable<company> Getcompany()
        //{
        //    return db.company;
        //}

        //// GET: api/Companies/5
        //[ResponseType(typeof(company))]
        //public IHttpActionResult Getcompany(string id)
        //{
        //    company company = db.company.Find(id);
        //    if (company == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(company);
        //}

        //// PUT: api/Companies/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult Putcompany(string id, company company)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != company.companyseq)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(company).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!companyExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Companies
        //[ResponseType(typeof(company))]
        //public IHttpActionResult Postcompany(company company)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.company.Add(company);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (companyExists(company.companyseq))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtRoute("DefaultApi", new { id = company.companyseq }, company);
        //}

        //// DELETE: api/Companies/5
        //[ResponseType(typeof(company))]
        //public IHttpActionResult Deletecompany(string id)
        //{
        //    company company = db.company.Find(id);
        //    if (company == null)
        //    {
        //        return NotFound();
        //    }

        //    db.company.Remove(company);
        //    db.SaveChanges();

        //    return Ok(company);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool companyExists(string email)
        {
            return db.Company.Count(e => e.email == email) > 0;
        }
    }
}