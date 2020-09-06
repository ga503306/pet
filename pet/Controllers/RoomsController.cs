using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using pet.Filter;
using pet.Models;
using pet.Security;
using WebApplication1.Models;

namespace pet.Controllers
{
    [RoutePrefix("api/Room")]
    public class RoomsController : ApiController
    {
        private Model1 db = new Model1();
        // Get: api/Room/GetCompanys //前台拿所有廠商 未被封鎖的
        [JwtAuthFilter]
        [Route("GetCompanys")]
        [HttpGet]
        public IHttpActionResult GetCompanys()
        {
            List<RoomCompanyModel> roomCompanyModels = new List<RoomCompanyModel>();
            List<Company> companies = db.Company.Where(x => x.del_flag == "N").ToList();
            foreach (Company company in companies)
            {
                RoomCompanyModel roomCompanyModel = new RoomCompanyModel();
                roomCompanyModel.companyseq = company.companyseq;
                roomCompanyModel.companybrand = company.companybrand;
                roomCompanyModel.avatar = company.avatar;
                roomCompanyModel.country = company.country;
                roomCompanyModel.area = company.area;
                roomCompanyModel.address = company.address;
                List<Room> rooms = db.Room.Where(x => x.companyseq == company.companyseq &&
                                                     x.state == Roomstate.已上架 &&
                                                     x.del_flag == "N").ToList();
                //廠商有 (已上架 && 未刪除) 的寄宿空間
                if (rooms.Count > 0)
                {
                    bool hascat = false;
                    bool hasdog = false;
                    bool hasother = false;
                    int price_min = int.MaxValue;
                    int price_max = int.MinValue;
                    foreach (Room r in rooms)
                    {
                        //如果某一間房間有勾貓 就在廠商顯示貓總類
                        if (r.pettype_cat.Value)
                            hascat = true;
                        if (r.pettype_dog.Value)
                            hasdog = true;
                        if (r.pettype_other.Value)
                            hasother = true;
                        //紀錄最大金額 最小金額
                        if (r.roomprice > price_max)
                            price_max = r.roomprice.Value;
                        if (r.roomprice < price_min)
                            price_min = r.roomprice.Value;
                    }
                    if (hascat)
                        roomCompanyModel.pettype += "貓咪 ";
                    if (hasdog)
                        roomCompanyModel.pettype += "狗 ";
                    if (hasother)
                        roomCompanyModel.pettype += "其他 ";

                    roomCompanyModel.roomprice_min = price_min;
                    roomCompanyModel.roomprice_max = price_max;
                }
                roomCompanyModel.rooms = rooms is null ? 0 : rooms.Count;
                roomCompanyModels.Add(roomCompanyModel);
            }

            return Ok(roomCompanyModels);
        }
        // Get: api/Room/GetRoomslist //用在廠商頁 只顯示已上架
        [JwtAuthFilter]
        [Route("GetRoomslist")]
        [HttpGet]
        public IHttpActionResult GetRoomslist()
        {
            List<RoomModel> roomModel = new List<RoomModel>();
            List<Room> room = db.Room.Where(x => x.del_flag == "N" && x.state == Roomstate.已上架).ToList();
            foreach (Room r in room)
            {
                RoomModel roomModel_ = new RoomModel();
                roomModel_.companyseq = r.companyseq;
                Company company = db.Company.Find(r.companyseq);//廠商暫存
                roomModel_.companybrand = company.companybrand;
                roomModel_.avatar = company.avatar;
                roomModel_.country = company.country;
                roomModel_.area = company.area;
                roomModel_.address = company.address;
                if (r.pettype_cat.Value)
                    roomModel_.pettype += "貓咪 ";
                if (r.pettype_dog.Value)
                    roomModel_.pettype += "狗 ";
                if (r.pettype_other.Value)
                    roomModel_.pettype += "其他 ";
                roomModel_.roomprice = r.roomprice;
                roomModel.Add(roomModel_);
            }
            return Ok(roomModel);
        }
        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoomExists(string id)
        {
            return db.Room.Count(e => e.roomseq == id) > 0;
        }
    }
}