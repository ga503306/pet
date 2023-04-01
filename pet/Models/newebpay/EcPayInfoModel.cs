using pet.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;

namespace ECPayInfo.Models
{
    /// <summary>
    ///  綠界設定model 先拆出測試會調整的部分
    /// </summary>
    public class ECPayInfoModel
    {
        /// <summary>
        /// 特店編號， 2000132 測試綠界編號
        /// </summary>
        public string MerchantID { get; set; }
        /// <summary>
        /// HashKey
        /// </summary>
        public string HashKey { get; set; }
        /// <summary>
        /// HashIV
        /// </summary>
        public string HashIV { get; set; }
        /// <summary>
        /// 綠界回傳付款資訊的至 此URL
        /// </summary>
        public string ReturnURL { get; set; }
        /// <summary>
        /// 使用者於綠界 付款完成後，綠界將會轉址至 此URL
        /// </summary>
        public string OrderResultURL { get; set; }
        /// <summary>
        /// 使用者於按下返回商店 導向的頁面
        /// </summary>
        public string ClientBackURL { get; set; }
        /// <summary>
        /// 付款方式為 ATM 時，當使用者於綠界操作結束時，綠界回傳 虛擬帳號資訊至 此URL
        /// </summary>
        public string PaymentInfoURL { get; set; }
        /// <summary>
        /// 付款方式為 ATM 時，當使用者於綠界操作結束時，綠界會轉址至 此URL。
        /// </summary>
        public string ClientRedirectURL { get; set; }

    }

    public class ECPayHelper
    {
        /// <summary>
        /// Dictionary<string, string> 綠界資料
        /// </summary>
        /// <param name="eCPayInfoModel"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public Dictionary<string, string> ConvertToECPayOrder(ECPayInfoModel eCPayInfoModel, Order order)
        {
            //以下綠界程式
            var ecpayOrder = new Dictionary<string, string>
        {
            //特店交易編號
            { "MerchantTradeNo",  order.orderseq},

            //特店交易時間 yyyy/MM/dd HH:mm:ss
            { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},

            //交易金額
            { "TotalAmount",  order.amt.Value.ToString()},

            //交易描述
            { "TradeDesc",  "pertrip交易訂單" + order.orderseq},

            //商品名稱
            { "ItemName",  "pertrip交易訂單"},

            //允許繳費有效天數(付款方式為 ATM 時，需設定此值)
            { "ExpireDate",  "3"},

            //自訂名稱欄位1
            { "CustomField1",  ""},

            //自訂名稱欄位2
            { "CustomField2",  ""},

            //自訂名稱欄位3
            { "CustomField3",  ""},

            //自訂名稱欄位4
            { "CustomField4",  ""},

            //綠界回傳付款資訊的至 此URL
            { "ReturnURL",  eCPayInfoModel.ReturnURL},

            //使用者於綠界 付款完成後，綠界將會轉址至 此URL
            //這邊有兩種做法 一種 OrderResultURL 留空 靠 ClientBackURL 他會顯示出一個 返回商店按鈕 這個按鈕是 返回導向網址 而不是POST
            //另一種做法是 OrderResultURL 導向 這裡文件是用POST方法 所以要先回到後端接API 之後發送導頁RedirectPermanent方法 給前端 而不是直接導到前端的頁面
             { "OrderResultURL", eCPayInfoModel.OrderResultURL},
            //{ "OrderResultURL", ""},
            //{ "ClientBackURL",  ECPayInfoModel.ClientBackURL},
             
            //付款方式為 ATM 時，當使用者於綠界操作結束時，綠界回傳 虛擬帳號資訊至 此URL
            { "PaymentInfoURL",  eCPayInfoModel.PaymentInfoURL},

            //付款方式為 ATM 時，當使用者於綠界操作結束時，綠界會轉址至 此URL。
            { "ClientRedirectURL", eCPayInfoModel.ClientRedirectURL},

            //特店編號， 2000132 測試綠界編號
            { "MerchantID", eCPayInfoModel.MerchantID},

            //忽略付款方式
            { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},

            //交易類型 固定填入 aio
            { "PaymentType",  "aio"},

            //選擇預設付款方式 固定填入 ALL
            { "ChoosePayment",  "ALL"},

            //CheckMacValue 加密類型 固定填入 1 (SHA256)
            { "EncryptType",  "1"},
        };
            //檢查碼
            ecpayOrder["CheckMacValue"] = GetCheckMacValue(ecpayOrder, eCPayInfoModel);
            return ecpayOrder;
        }

        /// <summary>
        /// 取得 檢查碼
        /// </summary>
        /// <param name="ECPayOrder"></param>
        /// <returns></returns>
        public string GetCheckMacValue(Dictionary<string, string> ECPayOrder, ECPayInfoModel ECPayInfoModel)
        {
            var param = ECPayOrder.Keys.OrderBy(x => x).Select(key => key + "=" + ECPayOrder[key]).ToList();

            var checkValue = string.Join("&", param);

            //測試用的 HashKey
            var hashKey = ECPayInfoModel.HashKey;

            //測試用的 HashIV
            var HashIV = ECPayInfoModel.HashIV;

            checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";

            checkValue = HttpUtility.UrlEncode(checkValue).ToLower();

            checkValue = GetSHA256(checkValue);

            return checkValue.ToUpper();
        }

        /// <summary>
        /// SHA256 編碼
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetSHA256(string value)
        {
            var result = new System.Text.StringBuilder();
            var sha256 = System.Security.Cryptography.SHA256Managed.Create();
            var bts = System.Text.Encoding.UTF8.GetBytes(value);
            var hash = sha256.ComputeHash(bts);

            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }

            return result.ToString();
        }
    }
}