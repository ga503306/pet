namespace Spgateway.Models
{
    /// <summary>
    /// 銀行端基本資訊
    /// </summary>
    public class BankInfoModel
    {
        /// <summary>
        /// 商店代號
        /// </summary>
        public string MerchantID { get; set; }

        /// <summary>
        /// AES 加密/SHA256 加密 Key
        /// </summary>
        public string HashKey { get; set; }

        /// <summary>
        /// AES 加密/SHA256 加密 IV
        /// </summary>
        public string HashIV { get; set; }

        /// <summary>
        /// 支付完成 返回商店網址
        /// <para>1.交易完成後，以 Form Post 方式導回商店頁面。</para>
        /// <para>2.若為空值，交易完成後，消費者將停留在智付通付款或取號完成頁面。</para>
        /// <para>3.只接受80與443 Port。</para>
        /// </summary>
        public string ReturnURL { get; set; }

        /// <summary>
        /// 支付通知網址
        /// <para>1.以幕後方式回傳給商店相關支付結果資料</para>
        /// <para>2.只接受80與443 Port。</para>
        /// </summary>
        public string NotifyURL { get; set; }
        
        /// <summary>
        /// 商店取號網址
        /// <para>1.系統取號後以 form post 方式將結果導回商店指定的網址</para>
        /// <para>2.此參數若為空值，則會顯示取號結果在智付通頁面。</para>
        /// </summary>
        public string CustomerURL { get; set; }

        /// <summary>
        /// 授權網址
        /// </summary>
        public string AuthUrl { get; set; }

        /// <summary>
        /// (取消)請退款網址
        /// </summary>
        public string CloseUrl { get; set; }

    }

    //商店傳來的 ReturnURL
    //see: https://developers.allpay.com.tw/AioCreditCard/PaidNotice
    //屬性只有 string  和 int type !!
    public class OrderResultModel
    {
        /// <summary>
        /// 商店代號( 註冊歐付寶會員時，歐付寶會提供一組商店代號。)
        /// </summary>
        public string MerchantID { get; set; }

        /// <summary>
        /// 合作特店交易編號( 訂單產生時傳送給allPay的廠商交易編號。英數字大小寫混合。)
        /// </summary>
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 合作特店商店代碼
        /// </summary>
        public string StoreID { get; set; }

        /// <summary>
        /// 交易狀態,( 1:交易成功，其餘代碼為交易失敗。)
        /// </summary>
        public int RtnCode { get; set; }

        /// <summary>
        /// 交易訊息, 告知付款結果。
        /// </summary>
        public string RtnMsg { get; set; }

        /// <summary>
        /// 綠界的交易編號(請保存綠界的交易編號與合作特店交易編號[MerchantTradeNo]的關連。)
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public int TradeAmt { get; set; }

        /// <summary>
        /// 付款時間, (日期時間格式：yyyy/MM/dd HH:mm:ss)
        /// </summary>
        public string PaymentDate { get; set; }

        /// <summary>
        /// 會員選擇的付款方式
        /// </summary>
        public string PaymentType { get; set; }

        /// <summary>
        /// 交易手續費
        /// </summary>
        public int PaymentTypeChargeFee { get; set; }

        /// <summary>
        /// 訂單成立時間,( 日期時間格式：yyyy/MM/dd HH:mm:ss)
        /// </summary>
        public string TradeDate { get; set; }


        /// <summary>
        /// 是否為模擬付款, 1-模擬付款/0-非模擬付款
        /// </summary>
        public int SimulatePaid { get; set; }

        /// <summary>
        /// 自訂名稱欄位1
        /// </summary>
        public string CustomField1 { get; set; }

        /// <summary>
        /// 自訂名稱欄位2
        /// </summary>
        public string CustomField2 { get; set; }

        /// <summary>
        /// 自訂名稱欄位3
        /// </summary>
        public string CustomField3 { get; set; }

        /// <summary>
        /// 自訂名稱欄位4
        /// </summary>
        public string CustomField4 { get; set; }

        /// <summary>
        /// 檢查碼
        /// </summary>
        public string CheckMacValue { get; set; }

        /// <summary>
        /// 繳費銀行代碼
        /// </summary>
        public string BankCode { get; set; }
        /// <summary>
        /// 繳費虛擬帳號
        /// </summary>
        public string vAccount { get; set; }
        /// <summary>
        /// 繳費期限
        /// </summary>
        public string ExpireDate { get; set; }
        /// <summary>
        /// 繳費代碼 (如果是條碼，則此欄位回傳空白)
        /// </summary>
        public string PaymentNo { get; set; }
        /// <summary>
        /// 條碼第一段號碼(格式為9碼數字，如果是代碼，則此欄位回傳空白)
        /// </summary>
        public string Barcode1 { get; set; }
        /// <summary>
        /// 條碼第二段號碼(格式為9碼數字，如果是代碼，則此欄位回傳空白)
        /// </summary>
        public string Barcode2 { get; set; }
        /// <summary>
        /// 條碼第三段號碼(格式為9碼數字，如果是代碼，則此欄位回傳空白)
        /// </summary>
        public string Barcode3 { get; set; }

        /// <summary>
        /// 是否完成繳費
        /// </summary>
        public bool IsPayOff { get; set; }
    }


    public class OrderPeriodResultModel : OrderResultModel
    {
        /// <summary>
        /// 週期類型 Y/M/D
        /// </summary>
        public string PeriodType { get; set; }

        /// <summary>
        /// 執行頻率
        /// </summary>
        public int Frequency { get; set; }

        /// <summary>
        /// 執行次數
        /// </summary>
        public int ExecTimes { get; set; }

    }
}