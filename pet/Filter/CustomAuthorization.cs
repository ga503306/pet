using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace pet.Fillter
{
    public class CustomAuthorization : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var req = actionContext.Request;
            if (!req.Headers.Contains("x-key") || req.Headers.GetValues("x-key") == null)
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage()
                {
                    Content = new StringContent("{\"result\":\"無授權\"}")
                };
                responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                actionContext.Response = responseMessage;
            }

            //actionContext.Response = new HttpResponseMessage
            //{
            //    StatusCode = HttpStatusCode.OK,
            //    Content = new StringContent("You are unauthorized to access this resource")
            //};
        }
    }
}