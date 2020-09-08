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
using System.Web.Security;
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
                                                     x.state == true &&
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
            List<Room> room = db.Room.Where(x => x.del_flag == "N" && x.state == true).ToList();
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
        // Get: api/Room/GetRooms //用在廠商後台 顯示廠商所有房間
        [JwtAuthFilter]
        [Route("GetRooms")]
        [HttpGet]
        public IHttpActionResult GetRooms()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);

            List<RoomBackendModel> roomModel = new List<RoomBackendModel>();
            List<Room> room = db.Room.Where(x => x.companyseq == userseq && x.del_flag == "N").ToList();
            foreach (Room r in room)
            {
                RoomBackendModel roomModel_ = new RoomBackendModel();
                roomModel_.companyseq = r.companyseq;
                roomModel_.roomseq = r.roomseq;
                roomModel_.roomname = r.roomname;
                roomModel_.state = r.state.HasValue;
                Company company = db.Company.Find(r.companyseq);//暫存廠商
                if (r.pettype_cat.Value)
                    roomModel_.pettype += "貓咪，";
                if (r.pettype_dog.Value)
                    roomModel_.pettype += "狗，";
                if (r.pettype_other.Value)
                    roomModel_.pettype += "其他，";
                roomModel_.pettype = roomModel_.pettype.Remove(roomModel_.pettype.LastIndexOf("，"), 1);
                roomModel.Add(roomModel_);
            }
            return Ok(roomModel);
        }

        // Get: api/Room/GetRooms/5
        [JwtAuthFilter]
        [HttpGet]
        [Route("GetRooms")]
        public IHttpActionResult GetRooms(string id)
        {
            Room room = db.Room.Find(id);
            room.img1 = room.img1 is null ? "" : room.img1;
            room.img2 = room.img2 is null ? "" : room.img2;
            room.img3 = room.img3 is null ? "" : room.img3;
            room.img4 = room.img4 is null ? "" : room.img4;
            return Ok(room);
        }

        // Post: api/Room/Create
        [JwtAuthFilter]
        [Route("Create")]
        [HttpPost]
        public IHttpActionResult Create(Room room)
        {
            string error_message = "Create，請至伺服器log查詢錯誤訊息";
            try
            {
                //拿已登入的流水
                string token = Request.Headers.Authorization.Parameter;
                JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
                string userseq = jwtAuthUtil.Getuserseq(token);
                //必填欄位
                room.companyseq = userseq;//必填欄位
                room.del_flag = "N";
                room.state = true;
                //重新驗證model
                ModelState.Clear();
                Validate(room);

                if (room.pettype_cat is false && room.pettype_dog is false && room.pettype_other is false)
                {
                    error_message = "寵物類型至少選一項";
                    throw new Exception("寵物類型至少選一項");
                }

                if (ModelState.IsValid)
                {
                    using (var transaction1 = db.Database.BeginTransaction((System.Data.IsolationLevel.RepeatableRead)))
                    {
                        string today = DateTime.Now.ToString("yyyyMMdd");
                        Room getseq = db.Room.Where(x => x.roomseq.Contains(today)).OrderByDescending(x => x.roomseq).FirstOrDefault();
                        int seq = getseq is null ? 0000 : Convert.ToInt32((getseq.roomseq.Substring(9, 4)));//流水號
                        room.roomseq = "R" + DateTime.Now.ToString("yyyyMMdd") + (seq + 1).ToString("0000");

                        db.Room.Add(room);
                        db.SaveChanges();
                        transaction1.Commit();

                    }
                }
                else
                {
                    return Ok(new
                    {
                        result = ModelState
                    });
                }

                return Ok(new
                {
                    result = "上架成功"
                });
            }
            catch (Exception ex)
            {
                Utility.log("廠商上架", ex.ToString());
                return Ok(new
                {
                    result = error_message
                });
            }

        }

        // Patch: api/Room/Edit
        [JwtAuthFilter]
        [Route("Edit")]
        [HttpPatch]
        public IHttpActionResult Edit(Room room)
        {
            string error_message = "patch錯誤，請至伺服器log查詢錯誤訊息";
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string userseq = jwtAuthUtil.Getuserseq(token);

            try
            {
                room.companyseq = userseq;
                room.del_flag = "N";//如果刪除的話 應該也不會進來編輯
                //重新驗證model
                ModelState.Clear();
                Validate(room);

                if (room.pettype_cat is false && room.pettype_dog is false && room.pettype_other is false)
                {
                    error_message = "寵物類型至少選一項";
                    throw new Exception("寵物類型至少選一項");
                }

                db.Room.Attach(room);
                foreach (PropertyInfo p in room.GetType().GetProperties())
                {
                    if (p.GetValue(room) != null)
                    {
                        db.Entry<Room>(room).Property(p.Name).IsModified = true;
                    }
                }

                db.SaveChanges();
            }
            catch (Exception ex)//失敗
            {
                Utility.log("PatchRoomEdit", ex.ToString());
                return Ok(new
                {
                    result = error_message//修改失敗
                });
            }
            return Ok(new
            {
                result = "修改成功"
            });
        }

        // Delete: api/Room/Delete
        [JwtAuthFilter]
        [Route("Delete")]
        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            string error_message = "delete錯誤，請至伺服器log查詢錯誤訊息";
            try
            {
                Room room = db.Room.Find(id);
                room.del_flag = "Y";
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Utility.log("PatchRoomdelete", ex.ToString());
                return Ok(new
                {
                    result = error_message//修改失敗
                });
            }
            return Ok(new
            {
                result = "刪除成功"
            });
        }

        // Post: api/Room/StateUpdate
        [JwtAuthFilter]
        [Route("StateUpdate")]
        [HttpPost]
        public IHttpActionResult StateUpdate(string id)
        {
            string error_message = "update錯誤，請至伺服器log查詢錯誤訊息";
            try
            {
                Room room = db.Room.Find(id);
                if (room.state == true)
                    room.state = false;
                else
                    room.state = true;
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Utility.log("PatchRoomupdate", ex.ToString());
                return Ok(new
                {
                    result = error_message//修改失敗
                });
            }
            return Ok(new
            {
                result = "更新成功"
            });
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