using Jose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace pet.Filter
{
    public class JwtAuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            string secret = "pettrip";//加解密的key,如果不一樣會無法成功解密
            var request = actionContext.Request;
            try
            {
                if (!WithoutVerifyToken(request.RequestUri.ToString()))
                {
                    if (request.Headers.Authorization == null || request.Headers.Authorization.Scheme != "Bearer")
                    {
                            HttpResponseMessage responseMessage = new HttpResponseMessage()
                            {
                                Content = new StringContent("{\"result\":\"無授權\"}")
                            };
                            responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            actionContext.Response = responseMessage;
                    }
                    else
                    {
                        //解密後會回傳Json格式的物件(即加密前的資料)
                        var jwtObject = Jose.JWT.Decode<Dictionary<string, Object>>(
                        request.Headers.Authorization.Parameter,
                        Encoding.UTF8.GetBytes(secret),
                        JwsAlgorithm.HS512);

                        if (IsTokenExpired(jwtObject["Exp"].ToString()))
                        {
                            throw new System.Exception("Token Expired");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage()
                {
                    Content = new StringContent("{\"result\":\"無授權\"}")
                };
                responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                actionContext.Response = responseMessage;

            }
            base.OnActionExecuting(actionContext);
        }

        //Login不需要驗證因為還沒有token
        public bool WithoutVerifyToken(string requestUri)
        {
            if (requestUri.EndsWith("/Login"))
                return true;
            return false;
        }

        //驗證token時效
        public bool IsTokenExpired(string dateTime)
        {
            return Convert.ToDateTime(dateTime) < DateTime.Now;
        }

    }
}