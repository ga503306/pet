﻿using System;
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

        [Route("GetCompanys")]
        [HttpGet]
        public IHttpActionResult GetCompanys(int page = 1, int paged = 6)
        {
            Pagination pagination = new Pagination();
            List<Company> companies = db.Company.Where(x => x.del_flag == "N" && x.Room.Count != 0).OrderBy(x => x.companyseq).Skip((page - 1) * paged).Take(paged).ToList();

            pagination.total = db.Company.Where(x => x.del_flag == "N" && x.Room.Count != 0).Count();
            pagination.count = companies.Count;
            pagination.per_page = paged;
            pagination.current_page = page;
            pagination.total_page = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(pagination.total) / Convert.ToDouble(pagination.per_page)));
            //if (companies.Select(x => new { pettype_cat = x.Room.Where(y => y.pettype_cat == true) }).Count() > 0)
            //    pettype_cat = true;
            //if (companies.Select(x => new { pettype_dog = x.Room.Where(y => y.pettype_dog == true) }).Count() > 0)
            //    pettype_dog = true;
            //if (companies.Select(x => new { pettype_other = x.Room.Where(y => y.pettype_other == true) }).Count() > 0)
            //    pettype_other = true;

            var result = new
            {
                companies = companies.Select(x => new
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
                    rooms = x.Room.Count()
                }),
                meta = pagination
            };

            //List<RoomCompanyModel> roomCompanyModels = new List<RoomCompanyModel>();

            //foreach (Company company in companies)
            //{
            //    RoomCompanyModel roomCompanyModel = new RoomCompanyModel();
            //    roomCompanyModel.companyseq = company.companyseq;
            //    roomCompanyModel.companybrand = company.companybrand;
            //    roomCompanyModel.avatar = company.avatar;
            //    roomCompanyModel.country = company.country;
            //    roomCompanyModel.area = company.area;
            //    roomCompanyModel.address = company.address;
            //    List<Room> rooms = db.Room.Where(x => x.companyseq == company.companyseq &&
            //                                         x.state == true &&
            //                                         x.del_flag == "N").ToList();
            //    //廠商有 (已上架 && 未刪除) 的寄宿空間
            //    if (rooms.Count > 0)
            //    {
            //        bool hascat = false;
            //        bool hasdog = false;
            //        bool hasother = false;
            //        int price_min = int.MaxValue;
            //        int price_max = int.MinValue;
            //        foreach (Room r in rooms)
            //        {
            //            //如果某一間房間有勾貓 就在廠商顯示貓總類
            //            if (r.pettype_cat.Value)
            //                hascat = true;
            //            if (r.pettype_dog.Value)
            //                hasdog = true;
            //            if (r.pettype_other.Value)
            //                hasother = true;
            //            //紀錄最大金額 最小金額
            //            if (r.roomprice > price_max)
            //                price_max = r.roomprice.Value;
            //            if (r.roomprice < price_min)
            //                price_min = r.roomprice.Value;
            //        }
            //        if (hascat)
            //            roomCompanyModel.pettype += "貓咪 ";
            //        if (hasdog)
            //            roomCompanyModel.pettype += "狗 ";
            //        if (hasother)
            //            roomCompanyModel.pettype += "其他 ";

            //        roomCompanyModel.roomprice_min = price_min;
            //        roomCompanyModel.roomprice_max = price_max;
            //    }
            //   roomCompanyModel.rooms = rooms is null ? 0 : rooms.Count;
            //    roomCompanyModels.Add(roomCompanyModel);
            // }

            return Ok(result);
        }
        // Get: api/Room/GetRoomslist //用在廠商頁 只顯示已上架
        [Route("GetRoomslist")]
        [HttpGet]
        public IHttpActionResult GetRoomslist(string id)
        {
            Company company = db.Company.Find(id);
            var result = new
            {
                company = new
                {
                    company.companyname,
                    company.companybrand,
                    avatar = company.avatar is null ? "" : company.avatar,
                    bannerimg = company.bannerimg is null ? "" : company.bannerimg,
                    company.introduce,
                    company.morning,
                    company.afternoon,
                    company.night,
                    company.midnight,
                    pettype_cat = check(company.Room.Where(y => y.pettype_cat == true).Count()),
                    pettype_dog = check(company.Room.Where(y => y.pettype_dog == true).Count()),
                    pettype_other = check(company.Room.Where(y => y.pettype_other == true).Count()),
                    company.Room.Count
                },
                roomlists = company.Room.Select(x => new
                {
                    x.roomseq,
                    x.companyseq,
                    x.roomname,
                    company.companybrand,
                    img1 = x.img1 is null ? "" : x.img1,
                    img2 = x.img2 is null ? "" : x.img2,
                    img3 = x.img3 is null ? "" : x.img3,
                    img4 = x.img4 is null ? "" : x.img4,
                    company.country,
                    company.area,
                    x.pettype_cat,
                    x.pettype_dog,
                    x.pettype_other,
                    x.visit,
                    x.roomamount,
                    x.petsizes,
                    x.petsizee,
                    x.roomprice,

                })
            };
            //改寫法
            //List<RoomModel> roomModel = new List<RoomModel>();
            //RoomModel roomModel = new RoomModel();
            //Company company = db.Company.Find(id);

            //List<Room> room = db.Room.Where(x => x.companyseq == id && x.del_flag == "N" && x.state == true).ToList();
            //foreach (Room r in room)
            //{
            //    Roomlist roomlist = new Roomlist();
            //    roomlist.companyseq = r.companyseq;
            //    roomlist.companybrand = company.companybrand;
            //    roomlist.avatar = company.avatar;
            //    roomlist.country = company.country;
            //    roomlist.area = company.area;
            //    roomlist.address = company.address;
            //    if (r.pettype_cat.Value)
            //        roomlist.pettype += "貓咪，";
            //    if (r.pettype_dog.Value)
            //        roomlist.pettype += "狗，";
            //    if (r.pettype_other.Value)
            //        roomlist.pettype += "其他，";
            //    if (!string.IsNullOrWhiteSpace(roomlist.pettype))//不是空的裁切字串
            //        roomlist.pettype = roomlist.pettype.Remove(roomlist.pettype.LastIndexOf("、"), 1);
            //    roomlist.roomprice = r.roomprice;
            //    roomModel.roomlists.Add(roomlist);
            //}

            //roomModel.companyDetail.companyname = company.companyname;
            //roomModel.companyDetail.companybrand = company.companybrand;
            //roomModel.companyDetail.pettype = company.companybrand;
            //roomModel.companyDetail.star = "";
            //roomModel.companyDetail.star_total = "";
            //if (company.morning.Value)
            //    roomModel.companyDetail.reply += "早上、";
            //if (company.afternoon.Value)
            //    roomModel.companyDetail.reply += "下午、";
            //if (company.night.Value)
            //    roomModel.companyDetail.reply += "晚上、";
            //if (company.midnight.Value)
            //    roomModel.companyDetail.reply += "深夜、";
            //if (!string.IsNullOrWhiteSpace(roomModel.companyDetail.reply))//不是空的裁切字串
            //    roomModel.companyDetail.reply = roomModel.companyDetail.reply.Remove(roomModel.companyDetail.reply.LastIndexOf("、"), 1);
            //roomModel.companyDetail.rooms = company.companybrand;

            return Ok(result);
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


            List<Room> room = db.Room.Where(x => x.companyseq == userseq && x.del_flag == "N").ToList();
            var result = new
            {
                room = room.Select(
                    x => new
                    {
                        x.companyseq,
                        x.roomseq,
                        x.roomname,
                        x.pettype_cat,
                        x.pettype_dog,
                        x.pettype_other,
                        x.state
                    })
            };

            // List<RoomBackendModel> roomModel = new List<RoomBackendModel>();
            //foreach (Room r in room)
            //{
            //    RoomBackendModel roomModel_ = new RoomBackendModel();
            //    roomModel_.companyseq = r.companyseq;
            //    roomModel_.roomseq = r.roomseq;
            //    roomModel_.roomname = r.roomname;
            //    roomModel_.state = r.state.Value;
            //    Company company = db.Company.Find(r.companyseq);//暫存廠商
            //    if (r.pettype_cat.Value)
            //        roomModel_.pettype += "貓咪，";
            //    if (r.pettype_dog.Value)
            //        roomModel_.pettype += "狗，";
            //    if (r.pettype_other.Value)
            //        roomModel_.pettype += "其他，";
            //    roomModel_.pettype = roomModel_.pettype.Remove(roomModel_.pettype.LastIndexOf("，"), 1);
            //    roomModel.Add(roomModel_);
            //}

            return Ok(result);
        }

        // Get: api/Room/GetRooms/5 //後台廠商編輯上架
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

        // Get: api/Room/GetRoomsFront/5 //前台 點入廠商上架的產品
        [HttpGet]
        [Route("GetRoomsFront")]
        public IHttpActionResult GetRoomsFront(string id)
        {
            //未完
            
            Room room = db.Room.Find(id);
            Company company = db.Company.Find(room.companyseq);
            var result = new
            {
                company = new
                {
                    company.companyname,
                    company.companybrand,
                    company.avatar,
                    company.morning,
                    company.afternoon,
                    company.night,
                    company.midnight,
                    company.country,
                    company.area,
                    company.address
                },
                room = new
                {
                    room.roomseq,
                    room.roomname,
                    room.companyseq,
                    room.introduce,
                    room.pettype_cat,
                    room.pettype_dog,
                    room.pettype_other,
                    room.petsizes,
                    room.petsizee,
                    room.roomamount,
                    room.roomprice,
                    room.roomamount_amt,
                    room.walk,
                    room.canned,
                    room.feed,
                    room.catlitter,
                    room.visit,
                    room.medicine_infeed,
                    room.medicine_infeed_amt,
                    room.medicine_paste,
                    room.medicine_paste_amt,
                    room.medicine_pill,
                    room.medicine_pill_amt,
                    room.bath,
                    room.bath_amt,
                    room.hair,
                    room.hair_amt,
                    room.nails,
                    room.nails_amt,
                    room.state,
                    img1 = room.img1 is null ? "" : room.img1,
                    img2 = room.img2 is null ? "" : room.img2,
                    img3 = room.img3 is null ? "" : room.img3,
                    img4 = room.img4 is null ? "" : room.img4
                }
            };
            return Ok(result);
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
                    room.medicine_infeed_amt = room.medicine_infeed_amt == null ? 0 : room.medicine_infeed_amt.Value;
                    room.medicine_paste_amt = room.medicine_paste_amt == null ? 0 : room.medicine_paste_amt.Value;
                    room.medicine_pill_amt = room.medicine_pill_amt == null ? 0 : room.medicine_pill_amt.Value;
                    room.bath_amt = room.bath_amt == null ? 0 : room.bath_amt.Value;
                    room.hair_amt = room.hair_amt == null ? 0 : room.hair_amt.Value;
                    room.nails_amt = room.nails_amt == null ? 0 : room.nails_amt.Value;
                    db.Room.Add(room);
                    db.SaveChanges();
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

        public bool check(int count)//檢查廠商房間 paytype用
        {
            if (count > 0)
                return true;
            else
                return false;
        }
    }
}