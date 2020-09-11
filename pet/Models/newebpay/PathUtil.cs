using System.Web;

namespace Spgateway.Models.Util
{
    public class PathUtil
    {
        /// <summary>
        /// 取得檔案實體路徑
        /// </summary>
        /// <param name="path">檔案路徑</param>
        /// <returns></returns>
        public static string MapPath(string path)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(path);
            }

            return HttpRuntime.AppDomainAppPath + path.Replace("~", string.Empty).Replace('/', '\\');
        }
    }
}