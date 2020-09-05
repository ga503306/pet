using Jose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace pet.Security
{
    public class JwtAuthUtil
    {
        public string GenerateToken(string Username,string Userseq)
        {
            string secret = "pettrip";//加解密的key,如果不一樣會無法成功解密
            Dictionary<string, Object> claim = new Dictionary<string, Object>();//payload 需透過token傳遞的資料
            claim.Add("Username", Username);
            claim.Add("Userseq", Userseq);
            claim.Add("Exp", DateTime.Now.AddSeconds(Convert.ToInt32("1200")).ToString());//Token 時效設定100秒
            var payload = claim;
            var token = Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS512);//產生token
            return token;
        }

        //public string Getuserdata(string token)
        //{
        //    string secret = "pettrip";//加解密的key,如果不一樣會無法成功解密
        //    var jwtObject = Jose.JWT.Decode<Dictionary<string, Object>>(
        //                token,
        //                Encoding.UTF8.GetBytes(secret),
        //                JwsAlgorithm.HS512);
        //    return jwtObject["Userdata"].ToString();
        //}

        public string Getuserseq(string token)
        {
            string secret = "pettrip";//加解密的key,如果不一樣會無法成功解密
            var jwtObject = Jose.JWT.Decode<Dictionary<string, Object>>(
                        token,
                        Encoding.UTF8.GetBytes(secret),
                        JwsAlgorithm.HS512);
            return jwtObject["Userseq"].ToString();
        }
    }
}