(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-61931e94"],{"19bb":function(t,a,i){t.exports=i.p+"img/image-input.220f2ce5.png"},ad8b:function(t,a,i){"use strict";i.r(a);var o=function(){var t=this,a=t.$createElement,i=t._self._c||a;return i("div",{staticClass:"firmSet pb-5"},[i("div",{staticClass:"container mx-auto"},[t._m(0),i("div",{staticClass:"bg-white text-nowrap tab-content border border-top-0",attrs:{id:"myTabContent"}},[i("div",{staticClass:"tab-pane px-3 py-5 fade show active",attrs:{id:"firmIntroduce",role:"tabpanel","aria-labelledby":"firmIntroduce-tab"}},[i("form",{attrs:{action:"#"}},[i("div",{staticClass:"form-group row"},[i("label",{staticClass:"col-md-3 col-lg-2 col-form-label font-weight-bold",attrs:{for:"mainIntroduce"}},[t._v("主頁介紹")]),i("div",{staticClass:"col-md-9 col-lg-10"},[i("textarea",{directives:[{name:"model",rawName:"v-model",value:t.companyData.introduce,expression:"companyData.introduce"}],staticClass:"form-control",attrs:{id:"mainIntroduce",rows:"3"},domProps:{value:t.companyData.introduce},on:{input:function(a){a.target.composing||t.$set(t.companyData,"introduce",a.target.value)}}})])]),i("div",{staticClass:"form-group align-items-center row text-wrap"},[i("label",{staticClass:"col-md-3 col-lg-2 col-form-label font-weight-bold",attrs:{for:"response"}},[t._v("回覆時間")]),i("div",{staticClass:"col-md-9 col-lg-10"},[i("div",{staticClass:"form-check form-check-inline"},[i("input",{directives:[{name:"model",rawName:"v-model",value:t.companyData.morning,expression:"companyData.morning"}],staticClass:"form-check-input",attrs:{id:"morning",type:"checkbox",value:"cat"},domProps:{checked:Array.isArray(t.companyData.morning)?t._i(t.companyData.morning,"cat")>-1:t.companyData.morning},on:{change:function(a){var i=t.companyData.morning,o=a.target,e=!!o.checked;if(Array.isArray(i)){var s="cat",n=t._i(i,s);o.checked?n<0&&t.$set(t.companyData,"morning",i.concat([s])):n>-1&&t.$set(t.companyData,"morning",i.slice(0,n).concat(i.slice(n+1)))}else t.$set(t.companyData,"morning",e)}}}),i("label",{staticClass:"form-check-label",attrs:{for:"morning"}},[t._v("早上")])]),i("div",{staticClass:"form-check form-check-inline"},[i("input",{directives:[{name:"model",rawName:"v-model",value:t.companyData.afternoon,expression:"companyData.afternoon"}],staticClass:"form-check-input",attrs:{id:"afternoon",type:"checkbox",value:"dog"},domProps:{checked:Array.isArray(t.companyData.afternoon)?t._i(t.companyData.afternoon,"dog")>-1:t.companyData.afternoon},on:{change:function(a){var i=t.companyData.afternoon,o=a.target,e=!!o.checked;if(Array.isArray(i)){var s="dog",n=t._i(i,s);o.checked?n<0&&t.$set(t.companyData,"afternoon",i.concat([s])):n>-1&&t.$set(t.companyData,"afternoon",i.slice(0,n).concat(i.slice(n+1)))}else t.$set(t.companyData,"afternoon",e)}}}),i("label",{staticClass:"form-check-label",attrs:{for:"afternoon"}},[t._v("下午")])]),i("div",{staticClass:"form-check form-check-inline"},[i("input",{directives:[{name:"model",rawName:"v-model",value:t.companyData.night,expression:"companyData.night"}],staticClass:"form-check-input",attrs:{id:"night",type:"checkbox",value:"other"},domProps:{checked:Array.isArray(t.companyData.night)?t._i(t.companyData.night,"other")>-1:t.companyData.night},on:{change:function(a){var i=t.companyData.night,o=a.target,e=!!o.checked;if(Array.isArray(i)){var s="other",n=t._i(i,s);o.checked?n<0&&t.$set(t.companyData,"night",i.concat([s])):n>-1&&t.$set(t.companyData,"night",i.slice(0,n).concat(i.slice(n+1)))}else t.$set(t.companyData,"night",e)}}}),i("label",{staticClass:"form-check-label",attrs:{for:"night"}},[t._v("晚上")])]),i("div",{staticClass:"form-check form-check-inline"},[i("input",{directives:[{name:"model",rawName:"v-model",value:t.companyData.midnight,expression:"companyData.midnight"}],staticClass:"form-check-input",attrs:{id:"midnight",type:"checkbox",value:"other"},domProps:{checked:Array.isArray(t.companyData.midnight)?t._i(t.companyData.midnight,"other")>-1:t.companyData.midnight},on:{change:function(a){var i=t.companyData.midnight,o=a.target,e=!!o.checked;if(Array.isArray(i)){var s="other",n=t._i(i,s);o.checked?n<0&&t.$set(t.companyData,"midnight",i.concat([s])):n>-1&&t.$set(t.companyData,"midnight",i.slice(0,n).concat(i.slice(n+1)))}else t.$set(t.companyData,"midnight",e)}}}),i("label",{staticClass:"form-check-label",attrs:{for:"midnight"}},[t._v("半夜")])])])]),i("div",{staticClass:"form-group row"},[i("label",{staticClass:"col-md-3 col-lg-2 col-form-label font-weight-bold",attrs:{for:"spaceName"}},[t._v("主頁相片")]),i("div",{staticClass:"col-md-9 col-lg-10"},[i("input",{directives:[{name:"model",rawName:"v-model.trim",value:t.companyData.bannerimg,expression:"companyData.bannerimg",modifiers:{trim:!0}}],staticClass:"form-control",attrs:{id:"spaceName",type:"text",placeholder:"圖片連結"},domProps:{value:t.companyData.bannerimg},on:{input:function(a){a.target.composing||t.$set(t.companyData,"bannerimg",a.target.value.trim())},blur:function(a){return t.$forceUpdate()}}})])]),i("div",{staticClass:"form-group d-flex mb-0"},[i("label",{staticClass:"d-flex align-items-center ml-auto btn btn-dark",class:{disabled:t.FirmPicUploading},attrs:{for:"upload"}},[i("ring-loader",{staticClass:"custom-class",attrs:{color:"black",loading:t.FirmPicUploading,size:20}}),t._v("主頁照片上傳 "),i("input",{staticClass:"d-none",attrs:{id:"upload",type:"file",disabled:t.FirmPicUploading},on:{change:t.updateFirmPic}})],1)]),t._m(1),i("div",{staticClass:"backgroundIMG mx-auto",style:{backgroundImage:"url("+t.companyData.bannerimg+")"}},[i("img",{staticClass:"w-100 img-fluid",class:{opacityZero:""!=t.companyData.bannerimg},attrs:{src:t.ImageInputImg,alt:""}})]),i("div",{staticClass:"form-group d-flex justify-content-center mt-4"},[i("button",{staticClass:"btn btn-primary",class:{disabled:t.FirmPicUploading},attrs:{type:"button",disabled:t.FirmPicUploading},on:{click:t.saveFirmData}},[t._v(" 儲存 ")])])])]),i("div",{staticClass:"tab-pane p-3 fade",attrs:{id:"firmData",role:"tabpanel","aria-labelledby":"firmData-tab"}},[i("div",{staticClass:"row no-gutters text-wrap"},[i("div",{staticClass:"col-12"},[i("div",{staticClass:"backgroundIMG rounded-circle mx-auto overflow-hidden",staticStyle:{"max-width":"300px"},style:{backgroundImage:"url("+t.companyData.avatar+")"}},[i("img",{staticClass:"w-100 img-fluid",class:{opacityZero:""!=t.companyData.avatar},attrs:{src:t.NoImageImg,alt:""}})]),i("div",{staticClass:"form-group d-flex my-4"},[i("label",{staticClass:"d-flex align-items-center btn btn-primary mx-auto",class:{disabled:t.FirmAvatarUploading},attrs:{for:"uploadAvatar"}},[i("ring-loader",{staticClass:"custom-class",attrs:{color:"black",loading:t.FirmAvatarUploading,size:20}}),t._v("更新頭像 "),i("input",{staticClass:"d-none",attrs:{id:"uploadAvatar",type:"file",disabled:t.FirmAvatarUploading},on:{change:t.updateFirmAvatar}})],1)])]),i("div",{staticClass:"col-lg-6 col-12"},[i("div",{staticClass:"row no-gutters"},[t._m(2),i("div",{staticClass:"col-md-8 col-12"},[i("p",[t._v(t._s(t.companyData.companyname))])])])]),i("div",{staticClass:"col-lg-6 col-12"},[i("div",{staticClass:"row no-gutters"},[t._m(3),i("div",{staticClass:"col-md-8 col-12"},[i("p",[t._v(t._s(t.companyData.companybrand))])])])]),i("div",{staticClass:"col-lg-6 col-12"},[i("div",{staticClass:"row no-gutters"},[t._m(4),i("div",{staticClass:"col-md-8 col-12"},[i("p",[t._v(t._s(t.companyData.email))])])])]),i("div",{staticClass:"col-lg-6 col-12"},[i("div",{staticClass:"row no-gutters"},[t._m(5),i("div",{staticClass:"col-md-8 col-12"},[i("p",[t._v(t._s(t.companyData.companyseq))])])])]),i("div",{staticClass:"col-lg-6 col-12"},[i("div",{staticClass:"row no-gutters"},[t._m(6),i("div",{staticClass:"col-md-8 col-12"},[i("p",[t._v(t._s(t.companyData.phone))])])])]),i("div",{staticClass:"col-lg-6 col-12"},[i("div",{staticClass:"row no-gutters"},[t._m(7),i("div",{staticClass:"col-md-8 col-12"},[i("p",[i("span",[t._v(t._s(t.companyData.country))]),i("span",[t._v(t._s(t.companyData.area))]),i("span",[t._v(t._s(t.companyData.address))])])])])]),i("div",{staticClass:"col-lg-6 col-12"},[i("div",{staticClass:"row no-gutters"},[t._m(8),i("div",{staticClass:"col-md-8 col-12"},[i("p",[t._v(t._s(t.companyData.pblicense))])])])]),i("div",{staticClass:"col-lg-6 col-12"},[i("div",{staticClass:"row no-gutters"},[t._m(9),i("div",{staticClass:"col-md-8 col-12"},[i("p",[t._v(t._s(t.companyData.effectivedate))])])])])])]),i("div",{staticClass:"tab-pane p-3 fade",attrs:{id:"passwordChange",role:"tabpanel","aria-labelledby":"passwordChange-tab"}},[i("ValidationObserver",{scopedSlots:t._u([{key:"default",fn:function(a){var o=a.invalid;return[i("form",{attrs:{action:"#"},on:{submit:function(a){return a.preventDefault(),t.savePassword(a)}}},[i("ValidationProvider",{attrs:{name:"密碼",rules:"required|alpha_num"},scopedSlots:t._u([{key:"default",fn:function(a){var o=a.errors,e=a.classes;return[i("div",{staticClass:"form-group row"},[i("label",{staticClass:"col-md-3 col-form-label font-weight-bold",attrs:{for:"password"}},[t._v("新密碼")]),i("div",{staticClass:"col-md-9"},[i("input",{directives:[{name:"model",rawName:"v-model",value:t.updatePwd.pwd,expression:"updatePwd.pwd"}],staticClass:"form-control",class:e,attrs:{id:"password",type:"password",autocomplete:"off"},domProps:{value:t.updatePwd.pwd},on:{input:function(a){a.target.composing||t.$set(t.updatePwd,"pwd",a.target.value)}}}),i("span",{staticClass:"text-danger"},[t._v(t._s(o[0]))])])])]}}],null,!0)}),i("ValidationProvider",{attrs:{rules:"required|password:@密碼|alpha_num"},scopedSlots:t._u([{key:"default",fn:function(a){var o=a.errors,e=a.classes;return[i("div",{staticClass:"form-group row"},[i("label",{staticClass:"col-md-3 col-form-label font-weight-bold",attrs:{for:"密碼"}},[t._v("再次輸入新密碼")]),i("div",{staticClass:"col-md-9"},[i("input",{directives:[{name:"model",rawName:"v-model",value:t.updatePwd.pwdCheck,expression:"updatePwd.pwdCheck"}],staticClass:"form-control",class:e,attrs:{id:"密碼",type:"password",autocomplete:"off"},domProps:{value:t.updatePwd.pwdCheck},on:{input:function(a){a.target.composing||t.$set(t.updatePwd,"pwdCheck",a.target.value)}}}),i("span",{staticClass:"text-danger"},[t._v(t._s(o[0]))])])])]}}],null,!0)}),i("div",{staticClass:"form-group d-flex justify-content-center mt-4"},[i("button",{staticClass:"btn btn-primary",class:{disabled:o},attrs:{type:"submit",disabled:o}},[t._v(" 修改 ")])])],1)]}}])})],1)])])])},e=[function(){var t=this,a=t.$createElement,i=t._self._c||a;return i("ul",{staticClass:"nav nav-tabs nav-fill text-center",attrs:{id:"myTab",role:"tablist"}},[i("li",{staticClass:"nav-item"},[i("a",{staticClass:"nav-link active",attrs:{id:"firmIntroduce-tab","data-toggle":"tab",href:"#firmIntroduce",role:"tab","aria-controls":"firmIntroduce","aria-selected":"true"}},[t._v("廠商介紹")])]),i("li",{staticClass:"nav-item"},[i("a",{staticClass:"nav-link",attrs:{id:"firmData-tab","data-toggle":"tab",href:"#firmData",role:"tab","aria-controls":"firmData","aria-selected":"false"}},[t._v("廠商資料")])]),i("li",{staticClass:"nav-item"},[i("a",{staticClass:"nav-link",attrs:{id:"passwordChange-tab","data-toggle":"tab",href:"#passwordChange",role:"tab","aria-controls":"passwordChange","aria-selected":"false"}},[t._v("密碼修改")])])])},function(){var t=this,a=t.$createElement,i=t._self._c||a;return i("p",{staticClass:"text-muted text-right mb-0"},[i("small",[t._v("儲存才能成功上傳圖片歐")])])},function(){var t=this,a=t.$createElement,i=t._self._c||a;return i("div",{staticClass:"col-md-4 col-12 font-weight-bold"},[i("p",[t._v("廠商名稱")])])},function(){var t=this,a=t.$createElement,i=t._self._c||a;return i("div",{staticClass:"col-md-4 col-12 font-weight-bold"},[i("p",[t._v("品牌名稱")])])},function(){var t=this,a=t.$createElement,i=t._self._c||a;return i("div",{staticClass:"col-md-4 col-12 font-weight-bold"},[i("p",[t._v("帳號")])])},function(){var t=this,a=t.$createElement,i=t._self._c||a;return i("div",{staticClass:"col-md-4 col-12 font-weight-bold"},[i("p",[t._v("廠商編號")])])},function(){var t=this,a=t.$createElement,i=t._self._c||a;return i("div",{staticClass:"col-md-4 col-12 font-weight-bold"},[i("p",[t._v("電話")])])},function(){var t=this,a=t.$createElement,i=t._self._c||a;return i("div",{staticClass:"col-md-4 col-12 font-weight-bold"},[i("p",[t._v("廠商地址")])])},function(){var t=this,a=t.$createElement,i=t._self._c||a;return i("div",{staticClass:"col-md-4 col-12 font-weight-bold"},[i("p",[t._v("寵物業許可證號")])])},function(){var t=this,a=t.$createElement,i=t._self._c||a;return i("div",{staticClass:"col-md-4 col-12 font-weight-bold"},[i("p",[t._v("有效日期")])])}],s=i("afa5"),n=i.n(s),c=i("19bb"),r=i.n(c),l={props:["identify"],data:function(){return{NoImageImg:n.a,ImageInputImg:r.a,companyData:{introduce:"",morning:!1,afternoon:!1,night:!1,midnight:!1,bannerimg:""},updatePwd:{pwd:"",pwdCheck:""},FirmPicUploading:!1,FirmAvatarUploading:!1}},created:function(){this.getOne()},methods:{updateFirmAvatar:function(t){var a=this,i=t.target.files[0],o=new FormData;o.append("file",i);var e="Company/Uploadimg";this.FirmAvatarUploading=!0;var s=this;this.$http.post(e,o,{headers:{"Content-Type":"multipart/form-data"}}).then((function(t){"圖片格式錯誤"===t.data.result?s.Swal.fire({toast:!0,position:"top-end",icon:"error",title:"圖片格式錯誤",showConfirmButton:!1,timer:2e3}):"上傳圖片錯誤，請至伺服器log查詢錯誤訊息"===t.data.result?s.Swal.fire({toast:!0,position:"top-end",icon:"info",title:"圖片不可超過 2 MB",showConfirmButton:!1,timer:2e3}):(s.Swal.fire({toast:!0,position:"top-end",icon:"success",title:"更新成功",showConfirmButton:!1,timer:2e3}),a.getOne()),a.FirmAvatarUploading=!1})).catch((function(){s.Swal.fire({toast:!0,position:"top-end",icon:"error",title:"更新失敗",showConfirmButton:!1,timer:2e3}),a.FirmAvatarUploading=!1}))},updateFirmPic:function(t){var a=this,i=t.target.files[0],o=this;if(void 0===i)o.Swal.fire({toast:!0,position:"top-end",icon:"info",title:"未選擇圖片",showConfirmButton:!1,timer:2e3});else{var e=new FormData;e.append("file",i);var s="Uploadimg";this.FirmPicUploading=!0,this.$http.post(s,e,{headers:{"Content-Type":"multipart/form-data"}}).then((function(t){"上傳圖片錯誤，請至伺服器log查詢錯誤訊息"===t.data.result?o.Swal.fire({toast:!0,position:"top-end",icon:"info",title:"圖片不可超過 2 MB",showConfirmButton:!1,timer:2e3}):(o.Swal.fire({toast:!0,position:"top-end",icon:"success",title:"上傳成功",showConfirmButton:!1,timer:2e3}),a.companyData.bannerimg=t.data.result),a.FirmPicUploading=!1})).catch((function(){o.Swal.fire({toast:!0,position:"top-end",icon:"error",title:"上傳失敗",showConfirmButton:!1,timer:2e3}),a.FirmPicUploading=!1}))}},saveFirmData:function(){this.$emit("loadAction",!0);var t=this,a={method:"patch",url:"Company/Patchcompany",headers:{},data:{introduce:"".concat(this.companyData.introduce),morning:"".concat(this.companyData.morning),afternoon:"".concat(this.companyData.afternoon),night:"".concat(this.companyData.night),midnight:"".concat(this.companyData.midnight),bannerimg:"".concat(this.companyData.bannerimg)}};this.$http(a).then((function(){t.Swal.fire({toast:!0,position:"top-end",icon:"success",title:"儲存成功",showConfirmButton:!1,timer:2e3}),t.getOne()})).catch((function(){t.Swal.fire({toast:!0,position:"top-end",icon:"error",title:"失敗成功",showConfirmButton:!1,timer:2e3}),t.$emit("loadAction",!1)}))},savePassword:function(){this.$emit("loadAction",!0);var t=this,a={method:"patch",url:"Company/Resetpwd",data:{pwd:"".concat(this.updatePwd.pwd)}};this.$http(a).then((function(){t.Swal.fire({toast:!0,position:"top-end",icon:"success",title:"密碼修改成功",showConfirmButton:!1,timer:2e3}),t.updatePwd.pwd="",t.updatePwd.pwdCheck="",t.getOne()})).catch((function(){t.Swal.fire({toast:!0,position:"top-end",icon:"error",title:"密碼修改失敗",showConfirmButton:!1,timer:2e3}),t.$emit("loadAction",!1)}))},getOne:function(){this.$emit("checkStatus","check"),this.$emit("loadAction",!0);var t=this,a={method:"get",url:"Company/GetOne"};this.$http(a).then((function(a){t.companyData=a.data,t.$emit("loadAction",!1),setTimeout((function(){"廠商"!==t.identify.identity&&(t.Swal.fire({toast:!0,position:"top-end",icon:"error",title:"進入廠商後台失敗",showConfirmButton:!1,timer:2e3}),t.$router.push("/"))}),500)})).catch((function(){t.$emit("loadAction",!1)}))}}},d=l,m=i("2877"),p=Object(m["a"])(d,o,e,!1,null,null,null);a["default"]=p.exports},afa5:function(t,a){t.exports="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAASwAAAEsBAMAAACLU5NGAAAAMFBMVEXm5ubl5eXS0tLk5OTQ0NDi4uLU1NTZ2dnq6ure3t7W1tbb29vg4OD+/v7y8vL29vYjzoSUAAADxElEQVR42u3Xu4sTURQG8O8wRqNYnOvgEwu5jFmMzYQgvhoZQ4LYJMris9AlrKAWGh8gWoyoYCG6Kj4WrdZlwUchohZqoTa66L+w4j8gtlbi3IwRray8TvH9mGSSSyAfZw73gZlvEBTMlRl8SlE4S77gW0eWoGDCK/jcEUXBSBllFI8CMbRwLR8DpnCPsO9cu3i9daENIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIvoLARQQBYy7VCAGEqu7KdzQTwZBB96kMAhryIlRiUXqBoMxo8ioSyd5ei+2q1EFBLkwhkKyy2XcbkIdBM5SLvQXyynXyghdpcQARhTuZpAG+FWvX58UfpQNQlcKo66lANSwBsGwol4D1FVI4RipK9wv/KjHg7/K+1vcLUWf1N1QXqYO+gJ4kpqyqdcNdo3f7SBUg4WxWaOBynDsnq1kPTU9fgNpqOEbQODJnujhhd7QzuUnG5VXECB4fuDS5CMEN6vvdelEFYoPI41D7T1RMzqDEL4s7o6uO17FNttMqnrurGpvFaJRgbVtzJkcuR2no9YmZ+fYhl1ZLsGXZd2Rsd6QHK+ceJpcFeSx7Ju9iY3jxZF9rHOT1rPTsix5eGvMiMAPnRd1H/WGSsnqLMRR9GPFUXTiVpIAN5vR4SzQ49c31s2pnLozVvfWW/FsO7G111pq910odfdpnMfqbjmexTo3cvj41nRRMz64+ujaqNE46q/j43mVF82J1vLfY6HxoNmdsjK/ce9mCwua8VRz81qbjB5BAD9crPHIbv3jIaJxv2GvWbM+qkaV18sajxdObZpTvV56b1CCHzrPXn9qt+L3lhe7f3Jolo13RN3IHlnaHZqe2LQhOTWdtXwAT+ZFY9sqLf19gghGVjxZdT5pv00+jEcrSie7k3ZfNkFEK1GCL3tGx/Z0WzvX9xp2MJ2Wesc2Xt3dak8daIs9pO+y6bTjezqVj3H/tuvl+GDxwd6d89ulGwiHNYRelOmX3hcfRYpMCNR1sFTrRQTqIqQCSC27vC/VppZvbBTAYGOjWcVEYTowcC+jWTK/GxsYAy2ngEIG20AgXAIV5NyQ922g9gska2AQDDbNOSm7t6ySxlXJ86bZKEyal8kMjhgKBLH7YuBchEHo+YiRunOPZnHg5AcyDbL3QAMXOs6jyM8DGRERERERERERERERERERERERERERERERERERERERERERERERERER/Q8GRP9KqkGMAlEAZ8v4NoxCUQDDl/FJUDjlL5j5fk5RMF9nfgBrIrv1RQ7jxAAAAABJRU5ErkJggg=="}}]);
//# sourceMappingURL=chunk-61931e94.94beca4f.js.map