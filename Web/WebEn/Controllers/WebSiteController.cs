using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Com.Panduo.Service;
using Com.Panduo.ServiceImpl; 
using Com.Panduo.Web.Common;
using Com.Panduo.Web.Common.Mvc.Model;
using Com.Panduo.Web.PaymentCommon.Common;

namespace Com.Panduo.Web.Controllers
{
    /// <summary>
    /// 网站管理与维护控制器
    /// </summary>
    public class WebSiteController : BaseController
    {
        #region 清除所有
        /// <summary>
        /// 清除所有缓存（服务层(Memcache)和IIS)
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearAllCache()
        {
            var jsonData = new JsonData();
            #region 验证
            if (!VerifyClearCache(jsonData))
            {
                return Json(jsonData,JsonRequestBehavior.AllowGet);
            }
            #endregion

            jsonData.Succeed = false;
            #region 执行操作
            try
            {
                //清除服务层缓存
                jsonData.Succeed = ServiceFactory.SystemService.ClearAllCache();
            }
            catch (Exception exception)
            {
                jsonData.Succeed = false;
                jsonData.Message = string.Format("清除服务层缓存失败，原因:{0}", exception.Message);

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }


            jsonData.Succeed = false;

            try
            {
                //清除IIS缓存
                CacheManager.Instance.ClearAllCache();

                jsonData.Succeed = true;
            }
            catch (Exception exception)
            {
                jsonData.Succeed = false;
                jsonData.Message = string.Format("清IIS除缓存失败，原因:{0}", exception.Message);
            }
            #endregion

            if (jsonData.Succeed)
            {
                jsonData.Message = "清除缓存成功";
            }

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 清除所有服务层缓存(Memcache)
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearAllMemcachedCache()
        {
            var jsonData = new JsonData();

            #region 验证
            if (!VerifyClearCache(jsonData))
            {
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            #endregion

            jsonData.Succeed = false;

            #region 执行操作
            try
            {
                //清除服务层缓存
                jsonData.Succeed = ServiceFactory.SystemService.ClearAllCache();
            }
            catch (Exception exception)
            {
                jsonData.Succeed = false;
                jsonData.Message = string.Format("清除服务层缓存失败，原因:{0}", exception.Message);
            }
            #endregion

            if (jsonData.Succeed)
            {
                jsonData.Message = "清除缓存成功";
            }

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 清除所有IIS缓存
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearAllWebCache()
        {
            var jsonData = new JsonData();

            #region 验证
            if (!VerifyClearCache(jsonData))
            {
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            #endregion

            jsonData.Succeed = false;

            #region 执行操作
            try
            {
                //清除IIS缓存
                CacheManager.Instance.ClearAllCache();

                jsonData.Succeed = true;
            }
            catch (Exception exception)
            {
                jsonData.Message = string.Format("清IIS除缓存失败，原因:{0}", exception.Message);
            }
            #endregion

            if (jsonData.Succeed)
            {
                jsonData.Message = "清除IIS缓存成功";
            }

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region 单个清除

        /// <summary>
        /// 清除指定Key的IIS缓存
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearWebCache()
        {
            var jsonData = new JsonData();

            #region 验证
            if (!VerifyClearCache(jsonData))
            {
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
             
            var cacheKey = Request["cacheKey"];
            jsonData.Succeed = false;
            if (string.IsNullOrEmpty(cacheKey))
            {
                jsonData.Message = "要清除缓存的Key不能为空";

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }

            if (CacheManager.Instance.GetCache(cacheKey) == null)
            {
                jsonData.Message = "要清除缓存的缓存不存在";

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            #endregion

            #region 执行操作
            try
            {
                //清除IIS缓存
                CacheManager.Instance.ClearCache(cacheKey);
            }
            catch (Exception exception)
            {
                jsonData.Succeed = false;
                jsonData.Message = string.Format("清IIS除缓存失败，原因:{0}", exception.Message);
            }
            #endregion

            if (jsonData.Succeed)
            {
                jsonData.Message = "清除缓存成功";
            }

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 清除指定Key的Memcached缓存
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearMemcachedCache()
        {
            var jsonData = new JsonData();

            #region 验证
            if (!VerifyClearCache(jsonData))
            {
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }

            var cacheKey = Request["cacheKey"];
            jsonData.Succeed = false;
            if (string.IsNullOrEmpty(cacheKey))
            {
                jsonData.Message = "要清除缓存的Key不能为空";

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }

            if (!ServiceFactory.SystemService.IsExists(cacheKey))
            {
                jsonData.Message = "要清除缓存的Key不存在";

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }

            #endregion

            #region 执行操作
            try
            {
                jsonData.Succeed = ServiceFactory.SystemService.ClearCache(cacheKey);
            }
            catch (Exception exception)
            {
                jsonData.Succeed = false;
                jsonData.Message = string.Format("清除缓存失败，原因:{0}", exception.Message);
            }
            #endregion

            if (jsonData.Succeed)
            {
                jsonData.Message = "清除缓存成功";
            }

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region 加载缓存
        /// <summary>
        /// 加载Memcached缓存
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadMemcachedCache()
        {
            var jsonData = new JsonData();

            #region 验证
            //是否加载商品缓存
            var isLoadProduct = Request["includeProduct"].ParseTo(false);
            if (!VerifyClearCache(jsonData))
            {
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }


            jsonData.Succeed = false; 
            #endregion

            #region 执行操作
            try
            {
                ServiceFactory.SystemService.LoadCache(isLoadProduct);

                jsonData.Succeed = true;
                jsonData.Message = "加载Memcache缓存成功!";
            }
            catch (Exception exception)
            {
                jsonData.Succeed = false;
                jsonData.Message = string.Format("加载Memcache缓存失败，原因:{0}", exception.Message);
            } 

            #endregion 

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 验证清除缓存权限
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        private bool VerifyClearCache(JsonData jsonData)
        {
            jsonData.Succeed = false;
            var token = Request["token"] ?? string.Empty;
            if (!string.Equals(Web.Common.ConfigHelper.ClearSiteCacheToken,token))
            {
                jsonData.Message = "清除或加载缓存的秘钥不正确!";
                jsonData.Data = token;
            }
            else
            {
                jsonData.Succeed = true;
            } 
            return jsonData.Succeed;
        }
        #endregion
    }
}
