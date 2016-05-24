//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ChannelController.cs
//创 建 人：罗海明
//创建时间：2015/03/03 10:49:50 
//功能说明：后台渠道商管理控制器
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Customer;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web.Controllers
{
    public class ChannelController : BaseController
    {
        private int AdminId
        {
            get { return SessionHelper.CurrentAdminUser == null ? 0 : SessionHelper.CurrentAdminUser.AdminId; }
        }

        public ActionResult Search()
        {
            return View();
        }
        /// <summary>
        /// 渠道商列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChannelList()
        {
            int page = Request["page"].ParseTo(1);
            int pageSize = Request["pageSize"].ParseTo(20);
            var searchCriteria = new Dictionary<ChannelSearchCriteria, object>();
            var sorterCriteria = new List<Sorter<ChannelSorterCriteria>>();
            var channellist = ServiceFactory.ChannelService.GetChannel(page, pageSize, searchCriteria, sorterCriteria);
            return View(channellist);
        }

        /// <summary>
        /// 添加渠道商显示页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddChannel()
        {
            return View();
        }

        /// <summary>
        /// 添加渠道商
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Add()
        {
            var hashtable = new Hashtable();
            hashtable.Add("msg", string.Empty);
            hashtable.Add("error", false);
            try
            {
                var strCustomerEmail = Request["txtCustomerEmail"] as string;
                if (RegexHelper.IsEmail(strCustomerEmail))
                {
                    if (!strCustomerEmail.IsNullOrEmpty())
                    {
                        ServiceFactory.ChannelService.AddChannel(strCustomerEmail, AdminId);
                        hashtable["getlist"] = true;
                        hashtable["url"] = "/Channel/ChannelList";
                    }
                    else
                    {
                        hashtable["msg"] = "请输入客户邮箱";
                        hashtable["error"] = true;
                    }
                }
                else
                {
                    hashtable["msg"] = "请输入正确的邮箱格式，如：example@test.com";
                    hashtable["error"] = true;
                }
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "ERROR_CUSTOMER_EMAIL_NOT_EXIST":
                        hashtable["msg"] = "系统中找不到该客户，请确认";
                        hashtable["error"] = true;
                        break;
                    case "ERROR_CUSTOMER_EXIST":
                        hashtable["msg"] = "输入的客户邮箱已经存在渠道商客户列表中，请确认";
                        hashtable["error"] = true;
                        break;
                    case "ERROR_CUSTOMER_IS_CLUB":
                        hashtable["msg"] = "你所输入的客户邮箱已经是club会员，不允许加为渠道商，请确认";
                        hashtable["error"] = true;
                        break;
                }
            }

            return Json(hashtable);
        }

        /// <summary>
        /// 根据客户Id删除渠道商
        /// </summary>
        /// <returns></returns>
        public JsonResult Delete()
        {
            var hashtable = new Hashtable();
            hashtable.Add("error", false);
            hashtable.Add("msg", string.Empty);
            var cId = Request["ids"].ParseTo(-1);//客户Id
            try
            {
                ServiceFactory.ChannelService.DeleteChannelByCustomerId(cId, AdminId);
                hashtable.Add("getlist", true);
                hashtable.Add("url", "/Channel/ChannelList");
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
            }

            return Json(hashtable);
        }
    }
}
