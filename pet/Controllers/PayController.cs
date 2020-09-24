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
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using pet.Fillter;
using pet.Filter;
using pet.Hubs;
using pet.Models;
using pet.Security;
using Spgateway.Models;
using Spgateway.Models.Util;
using WebApplication1.Models;

namespace pet.Controllers
{


    [RoutePrefix("api/Pay")]
    public class PayController : ApiController
    {
        private Model1 db = new Model1();

        private BankInfoModel _bankInfoModel = new BankInfoModel
        {
            MerchantID = "MS113701675",
            HashKey = "7VgBmKoiPRIvGtS26weblRPSoYfpQiOb",
            HashIV = "Cae6W3IT32n1OI8P",
            ReturnURL = "http://pettrip.rocket-coding.com",
            NotifyURL = "http://pettrip.rocket-coding.com/api/Pay/Notify",
            CustomerURL = "http://yourWebsitUrl/Bank/SpgatewayCustomer",
            AuthUrl = "https://ccore.spgateway.com/MPG/mpg_gateway",
            CloseUrl = "https://core.newebpay.com/API/CreditCard/Close"
        };

        // Post: api/pay/Getinfo
        [JwtAuthFilter]
        [Route("Getinfo")]
        [HttpPost]
        public IHttpActionResult SpgatewayPayBill(Order order)
        {
            string error_message = "下單錯誤，請至伺服器log查詢錯誤訊息";
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);
            
            //後端 判斷日期有沒有被下訂
            List<Order> orders = db.Order.Where(x => x.state == (int)Orderstate.已付款 && x.roomseq == order.roomseq).ToList();//已下的訂單
            List<string> date = new List<string>();//排除的日期
            foreach (Order o in orders)
            {
                date.AddRange(Utility.Data(o.orderdates.Value, o.orderdatee.Value));
            }
            if (date.Contains(order.orderdates.Value.ToString("yyyy-MM-dd")) ||
                date.Contains(order.orderdatee.Value.ToString("yyyy-MM-dd")))
            {
                error_message = "日期已被下訂";
                return Ok(new
                {
                    result = error_message
                });
            }

            Room room = db.Room.Find(order.roomseq);
            Company company = db.Company.Find(room.companyseq);
            order.companyseq = room.companyseq;
            order.companyname = company.companybrand;
            order.roomname = room.roomname;
            order.memberseq = userseq; //登入者 流水號
            order.country = company.country;
            order.area = company.area;
            order.address = company.address;
            //order.name 前端傳進
            //order.tel 前端傳進
            //order.pettype 
            //order.petsize
            //數量前端傳 金額room表拿
            //order.petamount
            order.roomprice = room.roomprice;
            order.roomamount_amt = room.roomamount_amt;
            //是否有勾 前端傳  重新回room表拿金額
            order.medicine_infeed_amt = room.medicine_infeed_amt;
            order.medicine_paste_amt = room.medicine_paste_amt;
            order.medicine_pill_amt = room.medicine_pill_amt;
            order.bath_amt = room.bath_amt;
            order.hair_amt = room.hair_amt;
            order.nails_amt = room.nails_amt;
            order.state = 0; //未付款
            //orderdatee orderdates
            #region 金額// 處理金額
            TimeSpan s = new TimeSpan(order.orderdatee.Value.Ticks - order.orderdates.Value.Ticks);
            int amt = 0;
            amt += (room.roomprice.Value + room.roomamount_amt.Value * (order.petamount.Value - 1)) * (s.Days + 1); //每間金額 * 天數  //隻 * 每隻金額

            if (order.medicine_infeed.Value) //判斷藥
                amt = amt + room.medicine_infeed_amt.Value * (order.petamount.Value);
            if (order.medicine_paste.Value)
                amt = amt + room.medicine_paste_amt.Value * (order.petamount.Value);
            if (order.medicine_pill.Value)
                amt = amt + room.medicine_pill_amt.Value * (order.petamount.Value);

            if (order.bath.Value) //判斷加購
                amt = amt + room.bath_amt.Value * (order.petamount.Value);
            if (order.hair.Value)
                amt = amt + room.hair_amt.Value * (order.petamount.Value);
            if (order.nails.Value)
                amt = amt + room.nails_amt.Value * (order.petamount.Value);

            //amt += room.roomprice.Value * (s.Days+1); 
            //amt += order.petamount.Value * room.roomamount_amt.Value; //隻 * 每隻金額

            #endregion

            order.amt = amt;
            order.postday = DateTime.Now;
            order.del_flag = "N";
            db.Order.Add(order);

            ModelState.Clear();
            Validate(order);

            if (!ModelState.IsValid)
            {
                Utility.log("下單 pay/Getinfo", ModelState.ToString());
                return Ok(new
                {
                    result = error_message
                });
            }

            db.SaveChanges();

            string version = "1.5";

            // 目前時間轉換 +08:00, 防止傳入時間或Server時間時區不同造成錯誤
            DateTimeOffset taipeiStandardTimeOffset = DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0));

            TradeInfo tradeInfo = new TradeInfo()
            {
                // * 商店代號
                MerchantID = _bankInfoModel.MerchantID,
                // * 回傳格式
                RespondType = "String",
                // * TimeStamp
                TimeStamp = taipeiStandardTimeOffset.ToUnixTimeSeconds().ToString(),
                // * 串接程式版本
                Version = version,
                // * 商店訂單編號
                //MerchantOrderNo = $"T{DateTime.Now.ToString("yyyyMMddHHmm")}",
                MerchantOrderNo = order.orderseq,
                // * 訂單金額
                Amt = order.amt.Value,
                // * 商品資訊
                ItemDesc = "pertrip交易訂單" + order.orderseq,
                // 繳費有效期限(適用於非即時交易)
                ExpireDate = null,
                // 支付完成 返回商店網址
                ReturnURL = _bankInfoModel.ReturnURL,
                // 支付通知網址
                NotifyURL = _bankInfoModel.NotifyURL,
                // 商店取號網址
                CustomerURL = _bankInfoModel.CustomerURL,
                // 支付取消 返回商店網址
                ClientBackURL = null,
                // * 付款人電子信箱
                Email = string.Empty,
                // 付款人電子信箱 是否開放修改(1=可修改 0=不可修改)
                EmailModify = 0,
                // 商店備註
                OrderComment = null,
                // 信用卡 一次付清啟用(1=啟用、0或者未有此參數=不啟用)
                CREDIT = null,
                // WEBATM啟用(1=啟用、0或者未有此參數，即代表不開啟)
                WEBATM = null,
                // ATM 轉帳啟用(1=啟用、0或者未有此參數，即代表不開啟)
                VACC = null,
                // 超商代碼繳費啟用(1=啟用、0或者未有此參數，即代表不開啟)(當該筆訂單金額小於 30 元或超過 2 萬元時，即使此參數設定為啟用，MPG 付款頁面仍不會顯示此支付方式選項。)
                CVS = null,
                // 超商條碼繳費啟用(1=啟用、0或者未有此參數，即代表不開啟)(當該筆訂單金額小於 20 元或超過 4 萬元時，即使此參數設定為啟用，MPG 付款頁面仍不會顯示此支付方式選項。)
                BARCODE = null
            };
            //暫定都信用卡
            tradeInfo.CREDIT = 1;
            //if (string.Equals(payType, "CREDIT"))
            //{
            //    tradeInfo.CREDIT = 1;
            //}
            //else if (string.Equals(payType, "WEBATM"))
            //{
            //    tradeInfo.WEBATM = 1;
            //}
            //else if (string.Equals(payType, "VACC"))
            //{
            //    // 設定繳費截止日期
            //    tradeInfo.ExpireDate = taipeiStandardTimeOffset.AddDays(1).ToString("yyyyMMdd");
            //    tradeInfo.VACC = 1;
            //}
            //else if (string.Equals(payType, "CVS"))
            //{
            //    // 設定繳費截止日期
            //    tradeInfo.ExpireDate = taipeiStandardTimeOffset.AddDays(1).ToString("yyyyMMdd");
            //    tradeInfo.CVS = 1;
            //}
            //else if (string.Equals(payType, "BARCODE"))
            //{
            //    // 設定繳費截止日期
            //    tradeInfo.ExpireDate = taipeiStandardTimeOffset.AddDays(1).ToString("yyyyMMdd");
            //    tradeInfo.BARCODE = 1;
            //}

            Atom<string> result = new Atom<string>()
            {
                IsSuccess = true
            };

            var inputModel = new SpgatewayInputModel
            {
                MerchantID = _bankInfoModel.MerchantID,
                Version = version
            };

            // 將model 轉換為List<KeyValuePair<string, string>>, null值不轉
            List<KeyValuePair<string, string>> tradeData = LambdaUtil.ModelToKeyValuePairList<TradeInfo>(tradeInfo);
            // 將List<KeyValuePair<string, string>> 轉換為 key1=Value1&key2=Value2&key3=Value3...
            var tradeQueryPara = string.Join("&", tradeData.Select(x => $"{x.Key}={x.Value}"));
            // AES 加密
            inputModel.TradeInfo = CryptoUtil.EncryptAESHex(tradeQueryPara, _bankInfoModel.HashKey, _bankInfoModel.HashIV);
            // SHA256 加密
            inputModel.TradeSha = CryptoUtil.EncryptSHA256($"HashKey={_bankInfoModel.HashKey}&{inputModel.TradeInfo}&HashIV={_bankInfoModel.HashIV}");

            // 將model 轉換為List<KeyValuePair<string, string>>, null值不轉
            List<KeyValuePair<string, string>> postData = LambdaUtil.ModelToKeyValuePairList<SpgatewayInputModel>(inputModel);



            return Ok(postData);

        }

        // Post: api/pay/Notify
        [Route("Notify")]
        [HttpPost]
        public IHttpActionResult SpgatewayNotify()
        {
            // 取法同SpgatewayResult
            var httpRequestBase = new HttpRequestWrapper(HttpContext.Current.Request);
            RazorExtensions.LogFormData(httpRequestBase, "SpgatewayNotify(支付完成)");
            // Status 回傳狀態 
            // MerchantID 回傳訊息
            // TradeInfo 交易資料AES 加密
            // TradeSha 交易資料SHA256 加密
            // Version 串接程式版本
            NameValueCollection collection = HttpContext.Current.Request.Form;

            if (collection["MerchantID"] != null && string.Equals(collection["MerchantID"], _bankInfoModel.MerchantID) &&
                collection["TradeInfo"] != null && string.Equals(collection["TradeSha"], CryptoUtil.EncryptSHA256($"HashKey={_bankInfoModel.HashKey}&{collection["TradeInfo"]}&HashIV={_bankInfoModel.HashIV}")))
            {
                var decryptTradeInfo = CryptoUtil.DecryptAESHex(collection["TradeInfo"], _bankInfoModel.HashKey, _bankInfoModel.HashIV);

                // 取得回傳參數(ex:key1=value1&key2=value2),儲存為NameValueCollection
                NameValueCollection decryptTradeCollection = HttpUtility.ParseQueryString(decryptTradeInfo);
                SpgatewayOutputDataModel convertModel = LambdaUtil.DictionaryToObject<SpgatewayOutputDataModel>(decryptTradeCollection.AllKeys.ToDictionary(k => k, k => decryptTradeCollection[k]));

                //LogUtil.WriteLog(JsonConvert.SerializeObject(convertModel));

                // TODO 將回傳訊息寫入資料庫

                // return Content(JsonConvert.SerializeObject(convertModel));

                if (convertModel.Status == "SUCCESS")
                {
                    //MerchantOrderNo = "O202009100034"
                    //convertModel.MerchantOrderNo
                    Order order = db.Order.Find(convertModel.MerchantOrderNo);
                    order.state = 1;
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();

                    //signalr即時通知
                    Utility.signalR_notice(order.memberseq, order.companyseq, order.orderseq, "", Noticetype.下單通知);
                    var context = GlobalHost.ConnectionManager.GetHubContext<DefaultHub>();
                    var connectid = db.Signalr.Where(x => x.whoseq == order.companyseq).Select(x => x.connectid).ToList();//需要通知的廠商signalr connectid
                    foreach (var c in connectid)
                    {
                        context.Clients.Client(c).Get();
                    }
                }
                else//付款失敗
                {

                }
                return Ok();
            }
            else
            {
                //LogUtil.WriteLog("MerchantID/TradeSha驗證錯誤");
            }

            return Ok();
        }
    }

}
