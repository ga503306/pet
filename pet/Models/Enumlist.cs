using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pet.Models
{
    public enum Roomstate
    {
        未上架 = 0,
        已上架 = 1
    }
    public enum Orderstate
    {//0付款失敗 1進行中(付款成功) 2已取消3以退款 4已完成
        訂單未成立 = 0,
        已付款 = 1,
        已取消 = 2,
        已退款 = 3,
        已完成 = 4
        //已完成未評價 = 4,
        //已完成已評價 = 5
    }
}