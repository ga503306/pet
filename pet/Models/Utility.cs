﻿using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pet.Controllers;
using pet.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;

namespace WebApplication1.Models
{
    public class Utility
    {

        #region "密碼加密"
        public const int DefaultSaltSize = 5;
        /// <summary>
        /// 產生Salt
        /// </summary>
        /// <returns>Salt</returns>
        public static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[DefaultSaltSize];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }
        ///// <summary>
        ///// 密碼加密
        ///// </summary>
        ///// <param name="password">密碼明碼</param>
        ///// <returns>Hash後密碼</returns>
        //public static string CreateHash(string password)
        //{
        //    string salt = CreateSalt();
        //    string saltAndPassword = String.Concat(password, salt);
        //    string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPassword, "SHA1");
        //    hashedPassword = string.Concat(hashedPassword, salt);
        //    return hashedPassword;
        //}

        /// <summary>
        /// Computes a salted hash of the password and salt provided and returns as a base64 encoded string.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt to use in the hash.</param>
        public static string GenerateHashWithSalt(string password, string salt)
        {
            // merge password and salt together
            string sHashWithSalt = password + salt;
            // convert this merged value to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(sHashWithSalt);
            // use hash algorithm to compute the hash
            HashAlgorithm algorithm = new SHA256Managed();
            // convert merged bytes to a hash as byte array
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            // return the has as a base 64 encoded string
            return Convert.ToBase64String(hash);
        }

        #endregion

        #region "將使用者資料寫入cookie,產生AuthenTicket"
        /// <summary>
        /// 將使用者資料寫入cookie,產生AuthenTicket
        /// </summary>
        /// <param name="userData">使用者資料</param>
        /// <param name="userId">UserAccount</param>
        static public void SetAuthenTicket(string userData, string userId)
        {
            //宣告一個驗證票
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userId, DateTime.Now, DateTime.Now.AddHours(3), false, userData);
            //加密驗證票
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            //建立Cookie
            HttpCookie authenticationcookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            //將Cookie寫入回應

            HttpContext.Current.Response.Cookies.Add(authenticationcookie);

        }
        #endregion


        #region "取得使用者權限"
        /// <summary>
        /// 取得使用者權限
        /// </summary>
        /// <param name="member">使用者</param>
        //public static void getperssion(member member)
        //{
        //    system.text.stringbuilder permissionsbuilder = new system.text.stringbuilder();
        //    foreach (role role in member.roles)
        //    {
        //        string rp = role.permission ?? "";
        //        string[] p = rp.split(',');
        //        foreach (string s in p.where(s => permissionsbuilder.tostring().indexof(s + ",") == -1))
        //        {
        //            permissionsbuilder.append(s + ",");
        //        }
        //    }
        //    string mp = member.permission ?? "";
        //    string[] mps = mp.split(',');
        //    foreach (string s in mps.where(s => permissionsbuilder.tostring().indexof(s + ",") == -1))
        //    {
        //        permissionsbuilder.append(s + ",");
        //    }
        //    member.permission = permissionsbuilder.tostring();
        //}
        #endregion


        #region  "取得TreeView 的Script"
        static private StringBuilder _memuResultBuilder = new StringBuilder();
        /// <summary>
        /// 取得TreeView 的Script
        /// </summary>
        /// <param name="permission">權限字串</param>
        /// <returns>取得TreeView 的Script</returns>
        public static string GetMenu(string permission)
        {

            permission = permission ?? "";
            System.Xml.XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath("~/Config/Menu.xml"));
            XmlNode rootnode = xmlDoc.DocumentElement;

            _memuResultBuilder = new StringBuilder();
            if (rootnode != null)
                foreach (XmlNode cnode in rootnode.ChildNodes)
                {
                    if (cnode.Attributes != null)
                    {
                        _memuResultBuilder.Append(string.Format("{{title: \"{0}\",key: \"{1}\", tooltip: \"{0}\" ", cnode.Attributes["Title"].Value, cnode.Attributes["Value"].Value));
                        if (permission.IndexOf(cnode.Attributes["Value"].Value, StringComparison.Ordinal) != -1)
                        {
                            _memuResultBuilder.Append(", select: true ");
                        }
                    }
                    if (cnode.HasChildNodes)
                    {
                        _memuResultBuilder.Append(", children: [");
                        GetTreeScript(cnode, permission);
                        _memuResultBuilder.Append("]");
                    }
                    _memuResultBuilder.Append("},");

                }
            return _memuResultBuilder.ToString().TrimEnd(',');
        }

        private static void GetTreeScript(XmlNode node, string permission)
        {
            foreach (XmlNode cnode in node.ChildNodes)
            {
                if (cnode.Attributes != null)
                {
                    _memuResultBuilder.Append(string.Format("{{title: \"{0}\",key: \"{1}\", tooltip: \"{0}\" ", cnode.Attributes["Title"].Value, cnode.Attributes["Value"].Value));
                    if (permission.IndexOf(cnode.Attributes["Value"].Value, StringComparison.Ordinal) != -1)
                    {
                        _memuResultBuilder.Append(", select: true ");
                    }
                }
                if (cnode.HasChildNodes)
                {
                    _memuResultBuilder.Append(", children: [");
                    GetTreeScript(cnode, permission);
                    _memuResultBuilder.Append("]");
                }
                _memuResultBuilder.Append("},");

            }
            _memuResultBuilder = new StringBuilder(_memuResultBuilder.ToString().TrimEnd(','));
        }
        #endregion

        #region "取得左方menu"
        static private StringBuilder _strMenuBuilder;
        /// <summary>
        /// 取得左方menu
        /// </summary>
        /// <param name="member"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        //public static string GetLeftMenu(Member member, string controllerName, XmlDocument xmlDoc)
        //{

        //    XmlNode rootnode = xmlDoc.DocumentElement;

        //    _strMenuBuilder = new System.Text.StringBuilder("");
        //    foreach (XmlNode node in rootnode.ChildNodes)
        //    {
        //        //判斷是否有權限顯示
        //        if (node.Attributes != null &&
        //            member.Permission.IndexOf(node.Attributes["Value"].Value, System.StringComparison.Ordinal) > -1)
        //        {
        //            GetMenuValue(controllerName, node, member);
        //        }
        //        else
        //        {
        //            string[] pString = node.Attributes["Value"].Value.Split('-');
        //            foreach (var s in pString)
        //            {
        //                if (member.Permission.IndexOf(s, System.StringComparison.Ordinal) > -1)
        //                {
        //                    GetMenuValue(controllerName, node, member);
        //                }
        //            }
        //        }
        //    }
        //    return _strMenuBuilder.ToString();
        //}

        //private static void GetMenuValue(string controllerName, XmlNode node, Member member)
        //{
        //    string strClass = "";
        //    if (node.HasChildNodes)
        //    {
        //        strClass = "parent ";
        //    }
        //    //判斷是否使用中
        //    strClass +=
        //        node.Attributes["Value"].Value.ToLower().IndexOf(controllerName.ToLower(), System.StringComparison.Ordinal) > -1
        //            ? "active"
        //            : "inactive";
        //    _strMenuBuilder.Append(" <li class=\"" + strClass + "\">");

        //    //取得((MvcHandler)HttpContext.Current.Handler).RequestContext 物件

        //    UrlHelper urlHelper = new UrlHelper(((MvcHandler)HttpContext.Current.Handler).RequestContext);

        //    _strMenuBuilder.Append("<div class=\"sidebar-menu-item-wrapper\">\n");
        //    _strMenuBuilder.Append("<a href=\"" +
        //                           urlHelper.Action(node.Attributes["Action"].Value, node.Attributes["Controller"].Value,
        //                               new { area = "Admin" }) +
        //                           "\" data-target-page=\"page-dashboard\">\n");
        //    _strMenuBuilder.Append("<i class=\"" + node.Attributes["Icon"].Value + "\"></i>\n");
        //    _strMenuBuilder.Append(" <span>" + node.Attributes["Title"].Value + "</span>\n");
        //    _strMenuBuilder.Append(" </a>\n");
        //    if (node.HasChildNodes)
        //    {
        //        _strMenuBuilder.Append("<ul>\n");
        //        GetSubMenu(node, controllerName, urlHelper, member);
        //        _strMenuBuilder.Append("</ul>\n");
        //    }
        //    _strMenuBuilder.Append("</div>\n");
        //    _strMenuBuilder.Append("</li>\n");
        //}

        //private static void GetSubMenu(XmlNode pNode, String controllerName, UrlHelper urlHelper, Member member)
        //{
        //    foreach (XmlNode node in pNode.ChildNodes)
        //    {
        //        //判斷是否有權限顯示
        //        if (node.Attributes != null &&
        //            member.Permission.IndexOf(node.Attributes["Value"].Value, System.StringComparison.Ordinal) > -1)
        //        {
        //            GetSubMenuValue(controllerName, urlHelper, node, member);
        //        }
        //        else
        //        {
        //            string[] pString = node.Attributes["Value"].Value.Split('-');
        //            foreach (var s in pString)
        //            {
        //                if (member.Permission.IndexOf(s, System.StringComparison.Ordinal) > -1)
        //                {
        //                    GetSubMenuValue(controllerName, urlHelper, node, member);
        //                }
        //            }
        //        }

        //    }
        //}

        //private static void GetSubMenuValue(string controllerName, UrlHelper urlHelper, XmlNode node, Member member)
        //{
        //    string strClass = "";
        //    if (node.HasChildNodes)
        //    {
        //        strClass = "parent ";
        //    }
        //    //判斷是否使用中
        //    strClass += node.Attributes["Value"].Value.ToLower().IndexOf(controllerName.ToLower()) > -1 ? "active" : "inactive";

        //    _strMenuBuilder.Append(" <li class=\"" + strClass + "\">");
        //    _strMenuBuilder.Append("<a href=\"" +
        //                           urlHelper.Action(node.Attributes["Action"].Value, node.Attributes["Controller"].Value) +
        //                           "\" data-target-page=\"page-dashboard\">\n");
        //    _strMenuBuilder.Append("<i class=\"" + node.Attributes["Icon"].Value + "\"></i>\n");
        //    _strMenuBuilder.Append(" <span>" + node.Attributes["Title"].Value + "</span>\n");
        //    _strMenuBuilder.Append(" </a>\n");
        //    if (node.HasChildNodes)
        //    {
        //        _strMenuBuilder.Append("<ul>\n");
        //        GetSubMenu(node, controllerName, urlHelper, member);
        //        _strMenuBuilder.Append("</ul>\n");
        //    }
        //    _strMenuBuilder.Append("</li>\n");
        //}

        #endregion







        #region"儲存上傳檔案"
        /// <summary>
        /// 儲存上傳檔案
        /// </summary>
        /// <param name="upfile">HttpPostedFile 物件</param>
        /// <returns>儲存檔名</returns>
        static public string SaveUpFile(HttpPostedFileBase upfile)
        {
            //取得副檔名
            string extension = upfile.FileName.Split('.')[upfile.FileName.Split('.').Length - 1];
            string fileName = upfile.FileName.Substring(0, upfile.FileName.Length - extension.Length - 1);
            string fileNameTemp = fileName;
            int i = 1;
            while (System.IO.File.Exists(Path.Combine(HttpContext.Current.Server.MapPath("~/upfiles"), string.Format("{0}.{1}", fileNameTemp, extension))))
            {
                fileNameTemp = fileName + "(" + i.ToString(CultureInfo.InvariantCulture) + ")";
                i++;
            }

            //新檔案名稱
            fileName = string.Format("{0}.{1}", fileNameTemp, extension);
            string savedName = Path.Combine(HttpContext.Current.Server.MapPath("~/upfiles"), fileName);
            upfile.SaveAs(savedName);
            return fileName;
        }
        #endregion

        #region"儲存上傳圖片"
        /// <summary>
        /// 儲存上傳圖片
        /// </summary>
        /// <param name="upfile">HttpPostedFile 物件</param>
        /// <returns>儲存檔名</returns>
        static public string SaveUpImage(HttpPostedFile upfile, string features, string seq)
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("/") + @"/Images/" + features + "/" + seq))
            {
                //新增資料夾
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/") + @"/Images/" + features + "/" + seq);
            }
            //取得副檔名
            string extension = upfile.FileName.Split('.')[upfile.FileName.Split('.').Length - 1];
            //新檔案名稱
            string fileName = String.Format("{0:yyyyMMddhhmmsss}.{1}", DateTime.Now, extension);
            //string fileName = String.Format("image.{0}", extension );
            //string savedName = Path.Combine(HttpContext.Current.Server.MapPath("~/upfiles/images"), fileName);
            string savedName = HttpContext.Current.Server.MapPath("~/Images")
                             + "/" + features
                             + "/" + seq
                             + "/" + fileName;
            //GenerateThumbnailImage(fileName, upfile, savedName, "", 1080, 720);
            upfile.SaveAs(savedName);
            //return "http://pettrip.rocket-coding.com/Images/" + features + "/" + seq + "/" + fileName;
            return "http://" + HttpContext.Current.Request.Url.Host + "/Images/" + features + "/" + seq + "/" + fileName;
        }
        #endregion


        #region "舉世無敵縮圖程式"
        /// <summary>
        /// 舉世無敵縮圖程式(多載)
        /// 1.會自動判斷是比較高還是比較寬，以比較大的那一方決定要縮的尺寸
        /// 2.指定寬度，等比例縮小
        /// 3.指定高度，等比例縮小
        /// </summary>
        /// <param name="name">原檔檔名</param>
        /// <param name="source">來源檔案的Stream,可接受上傳檔案</param>
        /// <param name="target">目的路徑</param>
        /// <param name="suffix">縮圖辯識符號</param>
        /// <param name="MaxWidth">指定要縮的寬度</param>
        /// <param name="MaxHight">指定要縮的高度</param>
        /// <remarks></remarks>
        static public void GenerateThumbnailImage(string name, HttpPostedFile source, string target, string suffix, int MaxWidth, int MaxHight)
        {
            System.Drawing.Image baseImage = Image.FromStream(source.InputStream, true, true);
            Single ratio = 0.0F; //存放縮圖比例
            Single h = baseImage.Height;//圖像原尺寸高度
            Single w = baseImage.Width;//圖像原尺寸寬度
            int ht;//圖像縮圖後高度
            int wt; //圖像縮圖後寬度
            if (w > h)
            {//圖像比較寬
                ratio = MaxWidth / w;//計算寬度縮圖比例
                if (MaxWidth < w)
                {
                    ht = Convert.ToInt32(ratio * h);
                    wt = MaxWidth;
                }
                else
                {
                    ht = Convert.ToInt32(baseImage.Height);
                    wt = Convert.ToInt32(baseImage.Width);
                }
            }
            else
            {//比較高
                ratio = MaxHight / h;//計算寬度縮圖比例
                if (MaxHight < h)
                {
                    ht = MaxHight;
                    wt = Convert.ToInt32(ratio * w);
                }
                else
                {
                    ht = Convert.ToInt32(baseImage.Height);
                    wt = Convert.ToInt32(baseImage.Width);
                }
            }
            //string Newname = target + "\\" + suffix + name;
            string Newname = target;
            System.Drawing.Bitmap img = new System.Drawing.Bitmap(wt, ht);
            System.Drawing.Graphics graphic = Graphics.FromImage(img);
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.InterpolationMode = InterpolationMode.Default;
            graphic.DrawImage(baseImage, 0, 0, wt, ht);
            img.Save(Newname);

            img.Dispose();
            graphic.Dispose();
            baseImage.Dispose();

        }
        #endregion

        #region "加解密函式"
        /// <summary>
        /// TripleDES解密
        /// </summary>
        /// <param name="byteEncryption">要解密的Byte()</param>
        /// <returns>解密後的字串</returns>
        /// <remarks></remarks>
        static public string TripleDesDecoding(byte[] byteEncryption)
        {
            TripleDESCryptoServiceProvider TDES = new TripleDESCryptoServiceProvider();
            byte[] iv = { 228, 7, 39, 133, 31, 159, 107, 181 };
            byte[] key = { 168, 159, 239, 4, 198, 215, 9, 253, 248, 56, 191, 68, 140, 68, 230, 130, 27, 162, 182, 240, 52, 116, 130, 18 };
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, TDES.CreateDecryptor(key, iv), CryptoStreamMode.Write);
            byte[] strInput = byteEncryption;
            cs.Write(strInput, 0, strInput.Length);
            cs.FlushFinalBlock();
            return System.Text.Encoding.Unicode.GetString((byte[])ms.ToArray());

        }
        /// <summary>
        /// TripleDES加密
        /// </summary>
        /// <param name="noEncryptionString">要加密的字串</param>
        /// <returns>加密後的Byte()</returns>
        /// <remarks></remarks>
        static public byte[] TripleDesEncryption(string noEncryptionString)
        {
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            byte[] iv = { 228, 7, 39, 133, 31, 159, 107, 181 };
            byte[] key = { 168, 159, 239, 4, 198, 215, 9, 253, 248, 56, 191, 68, 140, 68, 230, 130, 27, 162, 182, 240, 52, 116, 130, 18 };
            MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, tdes.CreateEncryptor(key, iv), CryptoStreamMode.Write);
            byte[] strInput = System.Text.Encoding.Unicode.GetBytes(noEncryptionString);
            cs.Write(strInput, 0, strInput.Length);
            cs.FlushFinalBlock();
            return ms.ToArray();

        }

        #region "加密函式"
        static public string Eecoding(string cryString)
        {
            string strReturn = "";
            char[] cryChar = cryString.ToCharArray();
            for (int i = 0; (i <= cryChar.Length - 1); i++)
            {
                strReturn += EncodingMyCode(Asc(cryChar[i]));
                Random randomNumber = new Random();
                double x = (90 - 65) + randomNumber.NextDouble() * 90;
                strReturn += ((char)(x));
            }
            return strReturn;
        }
        #endregion

        #region "加密字碼轉換對應表"
        private static char EncodingMyCode(byte bytChar)
        {
            switch (bytChar)
            {
                case 48:
                    return 'z';

                case 49:
                    return 'B';

                case 50:
                    return 'a';

                case 51:
                    return 'U';

                case 52:
                    return 'D';

                case 53:
                    return 'e';

                case 54:
                    return 'W';

                case 55:
                    return 'A';

                case 56:
                    return 'K';

                case 57:
                    return '3';

                case 65:
                    return 'L';

                case 66:
                    return 'f';

                case 67:
                    return 'T';

                case 68:
                    return 'o';

                case 69:
                    return '4';

                case 70:
                    return 'n';

                case 71:
                    return 'k';

                case 72:
                    return 'C';

                case 73:
                    return 'r';

                case 74:
                    return 'b';

                case 75:
                    return 'P';

                case 76:
                    return 's';

                case 77:
                    return 'V';

                case 78:
                    return '2';

                case 79:
                    return 'c';

                case 80:
                    return 'Y';

                case 81:
                    return 'u';

                case 82:
                    return '9';

                case 83:
                    return 'x';

                case 84:
                    return 'N';

                case 85:
                    return 'v';

                case 86:
                    return '8';

                case 87:
                    return 'l';

                case 88:
                    return 'M';

                case 89:
                    return 'g';

                case 90:
                    return '0';

                case 97:
                    return 'F';

                case 98:
                    return 'q';

                case 99:
                    return 'S';

                case 100:
                    return '1';

                case 101:
                    return 'd';

                case 102:
                    return 'j';

                case 103:
                    return 'G';

                case 104:
                    return 'Q';

                case 105:
                    return 'y';

                case 106:
                    return 'Z';

                case 107:
                    return '7';

                case 108:
                    return 'w';

                case 109:
                    return 'J';

                case 110:
                    return 't';

                case 111:
                    return 'X';

                case 112:
                    return '6';

                case 113:
                    return 'h';

                case 114:
                    return 'R';

                case 115:
                    return 'H';

                case 116:
                    return 'm';

                case 117:
                    return 'O';

                case 118:
                    return '5';

                case 119:
                    return 'p';

                case 120:
                    return 'I';

                case 121:
                    return 'i';

                case 122:
                    return 'E';

                default:
                    return ' ';

            }
        }

        #endregion


        #region "解密函式"
        static public string Decoding(string encryptionString)
        {
            char[] ecryChar = encryptionString.ToCharArray();
            string newstring = "";
            for (int i = 0; (i <= ecryChar.Length - 1); i++)
            {
                if (((i % 2) != 0))
                {

                    newstring += Chr(DncodingMyCode(ecryChar[i - 1]));

                }
            }
            return newstring;
        }
        #endregion

        #region "解密字碼轉換對應表"
        private static byte DncodingMyCode(char bytChar)
        {
            switch (bytChar)
            {
                case 'z':
                    return 48;

                case 'B':
                    return 49;

                case 'a':
                    return 50;

                case 'U':
                    return 51;

                case 'D':
                    return 52;

                case 'e':
                    return 53;

                case 'W':
                    return 54;

                case 'A':
                    return 55;

                case 'K':
                    return 56;

                case '3':
                    return 57;
                // 'A~Z

                case 'L':
                    return 65;

                case 'f':
                    return 66;

                case 'T':
                    return 67;

                case 'o':
                    return 68;

                case '4':
                    return 69;

                case 'n':
                    return 70;

                case 'k':
                    return 71;

                case 'C':
                    return 72;

                case 'r':
                    return 73;

                case 'b':
                    return 74;

                case 'P':
                    return 75;

                case 's':
                    return 76;

                case 'V':
                    return 77;

                case '2':
                    return 78;

                case 'c':
                    return 79;

                case 'Y':
                    return 80;

                case 'u':
                    return 81;

                case '9':
                    return 82;

                case 'x':
                    return 83;

                case 'N':
                    return 84;

                case 'v':
                    return 85;

                case '8':
                    return 86;

                case 'l':
                    return 87;

                case 'M':
                    return 88;

                case 'g':
                    return 89;

                case '0':
                    return 90;
                // a~z

                case 'F':
                    return 97;

                case 'q':
                    return 98;

                case 'S':
                    return 99;

                case '1':
                    return 100;

                case 'd':
                    return 101;

                case 'j':
                    return 102;

                case 'G':
                    return 103;

                case 'Q':
                    return 104;

                case 'y':
                    return 105;

                case 'Z':
                    return 106;

                case '7':
                    return 107;

                case 'w':
                    return 108;

                case 'J':
                    return 109;

                case 't':
                    return 110;

                case 'X':
                    return 111;

                case '6':
                    return 112;

                case 'h':
                    return 113;

                case 'R':
                    return 114;

                case 'H':
                    return 115;

                case 'm':
                    return 116;

                case 'O':
                    return 117;

                case '5':
                    return 118;

                case 'p':
                    return 119;

                case 'I':
                    return 120;

                case 'i':
                    return 121;

                case 'E':
                    return 122;

                default:
                    return 0;

            }
        }
        #endregion
        #endregion
        #region "Porting Common VB functions to C#: Asc()"
        public static byte Asc(char ch)
        {
            //Return the character value of the given character
            return Encoding.ASCII.GetBytes(new[] { ch }, 0, 1)[0];
        }
        #endregion

        #region "Porting Common VB functions to C#: Chr()"
        public static Char Chr(int i)
        {
            //Return the character of the given character value
            return Convert.ToChar(i);
        }
        #endregion

        #region "判斷是否為數字"
        /// <summary>
        /// 判斷是否為數字
        /// </summary>
        /// <param name="inputData">輸入字串</param>
        /// <returns>bool</returns>
        public static bool IsNumber(string inputData)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(inputData, "^[0-9]+$");
        }
        #endregion

        #region "驗證E-mail格式"
        /// <summary>
        /// 驗證E-mail格式
        /// </summary>
        /// <param name="strIn">輸入E-mail</param>
        /// <returns></returns>
        public static bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            return System.Text.RegularExpressions.Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        #endregion

        #region "驗證手機格式"
        /// <summary>
        /// 驗證手機格式
        /// </summary>
        /// <param name="strIn">輸入手機</param>
        /// <returns></returns>
        public static bool IsValidCellPhone(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            return System.Text.RegularExpressions.Regex.IsMatch(strIn, @"^(09([0-9]){8})$");
        }
        #endregion



        #region "發信"
        public static void SystemSendMail(string fromAddress, string toAddress, string subject, string mailBody)
        {

            MailMessage mailMessage = new MailMessage(fromAddress, toAddress);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = mailBody;
            // SMTP Server
            SmtpClient mailSender = new SmtpClient(ConfigurationManager.AppSettings["MailServer"]);
            //System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential("krtc7938888@krtc.com.tw", "8888");
            //mailSender.Credentials = basicAuthenticationInfo;
            try
            {

                mailSender.Send(mailMessage);
                mailMessage.Dispose();
            }
            catch
            {
                return;
            }
        }

        public static void SystemSendMailCc(string fromAddress, string toAddress, string subject, string mailBody)
        {

            MailMessage mailMessage = new MailMessage("1452@System.com.tw", toAddress);


            mailMessage.Subject = subject;
            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = mailBody;

            // SMTP Server
            SmtpClient client = new SmtpClient();
            try
            {
                client.EnableSsl = false;
                client.Send(mailMessage);
                mailMessage.Dispose();
            }
            catch (Exception)
            {

                return;
            }

            client = null;
        }

        public static void SendGmailMail(string fromAddress, string toAddress, string Subject, string MailBody, string password)
        {

            MailMessage mailMessage = new MailMessage(fromAddress, toAddress);
            mailMessage.Subject = Subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = MailBody;
            // SMTP Server
            SmtpClient mailSender = new SmtpClient("smtp.gmail.com");
            System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(fromAddress, password);
            mailSender.Credentials = basicAuthenticationInfo;
            mailSender.Port = 587;
            mailSender.EnableSsl = true;

            try
            {

                mailSender.Send(mailMessage);
                mailMessage.Dispose();
            }
            catch
            {
                return;
            }
        }
        #endregion

        #region "轉換DataTable 成CSV 格式"
        public static string DataTableToExcelCsv(DataTable dataTable, string sepChar)
        {
            if (dataTable.Rows.Count > 0)
            {
                string sep = "";
                StringBuilder sb = new StringBuilder();
                StringBuilder builder = new StringBuilder();
                foreach (DataColumn col in dataTable.Columns)
                {
                    builder.Append(sep).Append(col.ColumnName);
                    sep = sepChar;
                }
                sb.Append(builder + "\n");
                //then write all the rows
                foreach (DataRow row in dataTable.Rows)
                {
                    sep = "";
                    builder = new StringBuilder();
                    foreach (DataColumn col in dataTable.Columns)
                    {
                        string dat = "" + row[col.ColumnName];
                        dat = dat.Replace("\"", "\"\"");
                        if (IsNumber(dat))
                        {
                            dat = "=\"" + dat + "\"";
                        }
                        else
                        {
                            dat = "\"" + dat + "\"";
                        }

                        dat = dat.Replace("\n", "");
                        builder.Append(sep).Append(dat);

                        sep = sepChar;
                    }
                    sb.Append(builder + "\n");
                }
                return sb.ToString();
            }
            return "";
        }
        #endregion

        #region 錯誤log
        public static Boolean log(string log_name, string log_str)
        {
            string file_name = DateTime.Now.ToString("yyyyMMdd");
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "//log//" + file_name + ".txt";


            if (System.IO.File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(DateTime.Now.ToString() + "        " + log_name + "頁面:  錯誤訊息:" + log_str);
                    }
                }
            }
            else
            {
                FileStream fileStream = new FileStream(path, FileMode.Create);
                fileStream.Close();   //切記開了要關,不然會被佔用而無法修改喔!!!

                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine(DateTime.Now.ToString() + "        " + log_name + "頁面:  錯誤訊息:" + log_str);
                }
            }
            return true;
        }
        #endregion

        #region Getno 流水號
        public static string GetNo(string nokey, string ap, string date)
        {
            //宣告SQL的連線
            SqlConnection Conn = new SqlConnection();
            Conn.ConnectionString = ConfigurationManager.ConnectionStrings["Model1_m"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_getseq", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@nokey", SqlDbType.NVarChar);
                cmd.Parameters["@nokey"].Value = nokey;
                cmd.Parameters.Add("@ap", SqlDbType.NVarChar);
                cmd.Parameters["@ap"].Value = ap;
                cmd.Parameters.Add("@date", SqlDbType.NVarChar);
                cmd.Parameters["@date"].Value = date;
                Conn.Open();
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); //ExecureScalar
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                    return dt.Rows[0]["sno"].ToString();
                else
                    return null;
            }
            catch (Exception ex)
            {
                log("GetNo:", ex.ToString());
                return null;
            }
            finally
            {
                Conn.Close();
            }
        }
        #endregion

        #region 爬api
        public static JArray getjson(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url); //request請求
            req.Timeout = 10000; //request逾時時間
            req.Method = "GET"; //request方式
            HttpWebResponse respone = (HttpWebResponse)req.GetResponse(); //接收respone
            StreamReader streamReader = new StreamReader(respone.GetResponseStream(), Encoding.UTF8); //讀取respone資料
            string result = streamReader.ReadToEnd(); //讀取到最後一行
            respone.Close();
            streamReader.Close();
            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.LoadXml(result);
            JObject jsondata = JsonConvert.DeserializeObject<JObject>(result); //將資料轉為json物件
            JArray json = new JArray();
            json = JsonConvert.DeserializeObject<JArray>(jsondata["Message"].ToString());
            return json;
        }
        #endregion


        #region 日期起訖 回傳中間所有日期
        //只判斷日期 不判斷時間
        public static List<string> Data(DateTime statedate, DateTime enddate)
        {
            List<string> date = new List<string>();
            date.Add(statedate.ToString("yyyy-MM-dd"));
            while (statedate != enddate)
            {
                statedate = statedate.AddDays(1);
                date.Add(statedate.ToString("yyyy-MM-dd"));
            }
            return date;
        }
        #endregion

        //計算評價平均 state = 0 回傳平均 1 回傳筆數
        public static double Evaluation(string companyseq, string state = "0")
        {
            pet.Models.Model1 db = new pet.Models.Model1();
            List<Order> orders = db.Order.Where(x => x.companyseq == companyseq).ToList();
            double all = 0;
            int count = 0;
            foreach (Order o in orders)
            {
                if (o.Evalution.Select(x => x.star).FirstOrDefault() != null)
                {
                    all += (double)o.Evalution.Select(x => x.star).FirstOrDefault();
                    count++;
                }
            }
            if (state == "0")
            {
                if (count == 0)
                    return 0;
                return Math.Round((all / count), 2);
            }
            else
                return count;
        }

        //signalR_notice 發信者,收信者,內容,類型noticetype
        public static void signalR_notice(string fromseq, string toseq, string seq, string text, Noticetype type)
        {
            pet.Models.Model1 db = new pet.Models.Model1();
            Notice notice = new Notice();
            notice.fromseq = fromseq;
            notice.toseq = toseq;
            notice.seq = seq;
            notice.state = Convert.ToBoolean(Noticestate.未讀);
            notice.text = text;
            notice.postday = DateTime.Now;
            notice.type = (int)type;

            db.Notice.Add(notice);
            db.SaveChanges();
        }
    }
}