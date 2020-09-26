using pet.Filter;
using pet.Models;
using pet.Security;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Web.Http;
using WebApplication1.Models;

namespace pet.Controllers
{
    [RoutePrefix("api/Home")]
    public class DefaultController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/Home/GetAllInfo //首頁 拿所有資訊
        [Route("GetAllInfo")]
        [HttpGet]
        public IHttpActionResult GetAllInfo(int count = 5)
        {
            //首頁 廠商評價最高 5  廠商seq count 可不帶
            List<Company> companies = db.Company.Where(x => x.del_flag == "N" && x.Room.Count != 0).OrderBy(x => x.companyseq).ToList();

            var company = companies.Select(x => new
            {
                x.companyseq,
                x.companybrand,
                bannerimg = x.bannerimg is null ? "" : x.bannerimg,
                avatar = x.avatar is null ? "" : x.avatar,
                x.country,
                x.area,
                x.address,
                pettype_cat = check(x.Room.Where(y => y.pettype_cat == true).Count()),
                pettype_dog = check(x.Room.Where(y => y.pettype_dog == true).Count()),
                pettype_other = check(x.Room.Where(y => y.pettype_other == true).Count()),
                roomprice_min = x.Room.Min(y => y.roomprice) is null ? 0 : x.Room.Min(y => y.roomprice),
                roomprice_max = x.Room.Max(y => y.roomprice) is null ? 0 : x.Room.Max(y => y.roomprice),
                rooms = x.Room.Where(y => y.state == Convert.ToBoolean(Roomstate.已上架)).Count(),
                evaluation = Utility.Evaluation(x.companyseq, "0"),
                evaluation_count = Utility.Evaluation(x.companyseq, "1")
            }).AsQueryable();
            company = company.OrderByDescending(x => x.evaluation).Take(count);

            //首頁 房間 訂購最多 5 count 可不帶

            var rooms = db.Room.Where(x => x.del_flag == "N" && x.state == true).ToList();

            var rooms_ = rooms.GroupBy(x => x.roomseq).Select(x => new  //分組 先拿出所有房間流水號
            {
                roomseq = x.Select(y => y.roomseq).FirstOrDefault()
            });

            List<Order> orders = db.Order.Where(x => x.state == (int)Orderstate.已付款 || x.state == (int)Orderstate.已完成).ToList();//拿出所有 已付款和已完成訂單
            List<Order> toporders = new List<Order>();//temp

            foreach (var r in rooms_)//找到 房間號的訂單
            {
                List<Order> order = orders.Where(x => x.roomseq == r.roomseq).ToList();
                toporders.AddRange(order);
            }

            var toporders_ = toporders.GroupBy(x => x.roomseq).Select(x => new //算出房間有幾筆成功的訂單
            {
                x.Key,
                roomseq = x.Select(y => y.roomseq).FirstOrDefault(),
                roomcount = x.Count()
            }).OrderByDescending(x => x.roomcount).Take(count);

            List<Room> toprooms = new List<Room>();//temp

            foreach (var t in toporders_)
            {
                Room room = rooms.Where(x => x.roomseq == t.roomseq).FirstOrDefault(); //把正確的房間資料抓回
                toprooms.Add(room);
            }
            //評價 五星 日期排序 新的前五筆 
            var evalution = db.Evalution.Where(x => x.star == 5 && x.memo != "").OrderByDescending(x => x.postday).Take(5).ToList();

            //首頁 房間總數
            int roomcount = rooms.Count();
            //首頁 廠商總數
            int compantcount = companies.Count();
            //首頁 成交訂單總數
            int ordercount = db.Order.Where(x => x.state == (int)Orderstate.已付款 || x.state == (int)Orderstate.已完成).Count();

            var result = new
            {
                evalution,
                company,
                rooms = toprooms.Select(x => new
                {
                    x.roomseq,
                    x.roomname,
                    x.companyseq,
                    x.Company.companybrand,
                    x.pettype_cat,
                    x.pettype_dog,
                    x.pettype_other,
                    x.roomamount,
                    x.petsizes,
                    x.petsizee,
                    x.roomprice,
                    x.img1
                }),
                roomcount,
                compantcount,
                ordercount
            };

            return Ok(result);
        }


        // GET: api/Home/GetTopcustom //首頁 廠商評價最高 5  廠商seq count 可不帶
        [Route("GetTopcustom")]
        [HttpGet]
        public IHttpActionResult GetTopcustom(int count = 5)
        {

            List<Company> companies = db.Company.Where(x => x.del_flag == "N" && x.Room.Count != 0).OrderBy(x => x.companyseq).ToList();

            var company = companies.Select(x => new
            {
                x.companyseq,
                x.companybrand,
                bannerimg = x.bannerimg is null ? "" : x.bannerimg,
                avatar = x.avatar is null ? "" : x.avatar,
                x.country,
                x.area,
                x.address,
                pettype_cat = check(x.Room.Where(y => y.pettype_cat == true).Count()),
                pettype_dog = check(x.Room.Where(y => y.pettype_dog == true).Count()),
                pettype_other = check(x.Room.Where(y => y.pettype_other == true).Count()),
                roomprice_min = x.Room.Min(y => y.roomprice) is null ? 0 : x.Room.Min(y => y.roomprice),
                roomprice_max = x.Room.Max(y => y.roomprice) is null ? 0 : x.Room.Max(y => y.roomprice),
                rooms = x.Room.Where(y => y.state == Convert.ToBoolean(Roomstate.已上架)).Count(),
                evaluation = Utility.Evaluation(x.companyseq, "0"),
                evaluation_count = Utility.Evaluation(x.companyseq, "1")
            }).AsQueryable();
            company = company.OrderByDescending(x => x.evaluation).Take(count);

            return Ok(company);
        }

        // GET: api/Home/GetTopcustom //首頁 房間 訂購最多 5 count 可不帶 //方法不同info 是從訂單出發 info是從房間出發(better)
        [Route("GetToproom")]
        [HttpGet]
        public IHttpActionResult GetToproom(int count = 5)
        {
            var order = db.Order.GroupBy(x => x.roomseq).Select(x => new
            {
                x.Key,
                roomseq = x.Select(y => y.roomseq).FirstOrDefault(),
                roomname = x.Select(y => y.roomname).FirstOrDefault(),
                roomcount = x.Count(),

            }).OrderByDescending(x => x.roomcount).Take(count);

            //var rooms = db.Room.Where(x => x.del_flag == "N" && x.state == true && order.Select(y => y.roomseq).Contains(x.roomseq)).AsQueryable();
            var rooms = db.Room.Where(x => x.del_flag == "N" && x.state == true).ToList();
            List<Room> toproom = new List<Room>();

            foreach (var o in order)
            {
                Room room = rooms.Where(x => x.roomseq == o.roomseq).FirstOrDefault();
                if (room != null)
                    toproom.Add(room);
            }

            var result = new
            {
                rooms = toproom.Select(x => new
                {
                    x.roomseq,
                    x.roomname,
                    x.companyseq,
                    x.Company.companybrand,
                    x.pettype_cat,
                    x.pettype_dog,
                    x.pettype_other,
                    x.roomamount,
                    x.petsizes,
                    x.petsizee,
                    x.roomprice,
                    x.img1
                }),
            };
            return Ok(result);
        }
        public bool check(int count)//檢查廠商房間 paytype用
        {
            if (count > 0)
                return true;
            else
                return false;
        }
    }

}
