using pet.Filter;
using pet.Models;
using pet.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Web;
using WebApplication1.Models;

namespace pet.Controllers
{
    [RoutePrefix("api")]
    public class ValuesController : ApiController
    {
        // Post: api/Uploadimg
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

        // Post: api/CheckIdentity
        [JwtAuthFilter]
        [HttpGet]
        [Route("GetIdentity")]
        public IHttpActionResult GetIdentity()
        {
            string error_message = "GetIdentity，請至伺服器log查詢錯誤訊息";
            //拿已登入的流水
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);
            string Identity = null;
            try
            {
                string temp = userseq.Substring(0, 1);

                if (temp == "C")
                    Identity = "廠商";
                else
                    Identity = "會員";

                return Ok(new
                {
                    result = Identity
                });
            }
            catch (Exception ex)
            {
                Utility.log("GetIdentity", ex.ToString());
                return Ok(new
                {
                    result = error_message
                });
            }

        }
    }
}
