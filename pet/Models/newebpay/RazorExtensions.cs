using Newtonsoft.Json;
using System.Text;
using System.Web;

namespace Spgateway.Models.Util
{
    /// <summary>Razor 擴充函式</summary>
    public static class RazorExtensions
    {

        /// <summary>紀錄 Form Data</summary>
        public static void LogFormData(this HttpRequestBase request, string funcName)
        {
            var formData = request.Unvalidated.Form;
            LogUtil.WriteLog(JsonConvert.SerializeObject(formData));

            var builder = new StringBuilder();
            builder.AppendLine(funcName);
            foreach (string key in formData)
            {
                var val = formData[key];
                builder.AppendLine(string.Format("{0} : {1}", key, val));
            }
            LogUtil.WriteLog(builder.ToString());
        }
    }
}