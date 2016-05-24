using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.ServiceConst
{
    /// <summary>
    /// Memcached 常量配置
    /// </summary>
    public class MemcachedConst
    {
        #region 常量
        /// <summary>
        /// All的Key
        /// </summary>
        public static readonly string KEY_ALL = "ALL";
        #region 产品相关
        /// <summary>
        /// 产品Key
        /// </summary>
        public static readonly string KEY_PRODUCT = "PRODUCT";
        /// <summary>
        /// 产品CODEKey
        /// </summary>
        public static readonly string KEY_PRODUCT_CODE = "PRODUCT_CODE";
        /// <summary>
        /// 产品是否相似商品Key
        /// </summary>
        public static readonly string KEY_PRODUCT_HAS_SIMILAR = "PRODUCT_HAS_SIMILAR";

        /// <summary>
        /// 产品图片Key
        /// </summary>
        public static readonly string KEY_PRODUCT_IMG = "PRODUCT_IMG";
        /// <summary>
        /// 产品图片列表Key
        /// </summary>
        public static readonly string KEY_PRODUCT_IMG_LIST = "PRODUCT_IMG_LIST";
        /// <summary>
        /// 相似商品属性值列表Key
        /// </summary>
        public static readonly string KEY_PRODUCT_SIMILAR_PROPERTY_VALUE = "PRODUCT_SIMILAR_PROPERTY_VALUE"; 
        #endregion

        #region 类别相关
        /// <summary>
        /// 类别Key
        /// </summary>
        public static readonly string KEY_CATEGORY = "CATEGORY";
        /// <summary>
        /// 根类别Key
        /// </summary>
        public static readonly string KEY_CATEGORY_ROOT = "CATEGORY_ROOT";
        /// <summary>
        /// 根类别Key
        /// </summary>
        public static readonly string KEY_CATEGORY_SUB = "CATEGORY_SUB";

        /// <summary>
        /// 类别树Key
        /// </summary>
        public static readonly string KEY_CATEGORY_TREE = "CATEGORY_TREE";

        /// <summary>
        /// 类别语种Key
        /// </summary>
        public static readonly string KEY_CATEGORY_LANGUAGE = "CATEGORY_LANGUAGE"; 
        #endregion

        #region 属性相关
        /// <summary>
        /// 属性Key
        /// </summary>
        public static readonly string KEY_PROPERTY = "PROPERTY";
        /// <summary>
        /// 属性Key多语种
        /// </summary>
        public static readonly string KEY_PROPERTY_LANGUAGE = "PROPERTY_LANGUAGE";
        /// <summary>
        /// 属性值key
        /// </summary>
        public static readonly string KEY_PROPERTY_VALUE = "PROPERTY_VALUE";
        /// <summary>
        /// 属性值key多语种
        /// </summary>
        public static readonly string KEY_PROPERTY_VALUE_LANGUAGE = "PROPERTY_VALUE_LANGUAGE";
        /// <summary>
        /// 属性值组Key
        /// </summary>
        public static readonly string KEY_PROPERTY_VALUE_GROUP = "PROPERTY_VALUE_GROUP";
        /// <summary>
        /// 属性值组Key多语种
        /// </summary>
        public static readonly string KEY_PROPERTY_VALUE_GROUP_LANGUAGE = "PROPERTY_VALUE_GROUP_LANGUAGE"; 
        /// <summary>
        /// 类别属性Key(没有多语种)
        /// </summary>
        public static readonly string KEY_PROPERTY_CATEGORY = "PROPERTY_CATEGORY";
        #endregion

        #region 币种 Currencies
        /// <summary>
        /// 币种
        /// </summary>
        public static readonly string KEY_CURRENCY = "CURRENCY";
        /// <summary>
        /// 有效币种
        /// </summary>
        public static readonly string KEY_CURRENCY_VALID = "CURRENCY_VALID";

        #endregion

        #region 配置相关
        /// <summary>
        /// 系统配置
        /// </summary>
        public static readonly string KEY_SYSTEM_CONFIG = "SYSTEM_CONFIG";
        #endregion

        #region DailyDeal
        /// <summary>
        /// DailyDeal Key
        /// </summary>
        public static readonly string KEY_DAILYDEAL = "DAILYDEAL";
        /// <summary>
        /// DailyDeal标语库 Key
        /// </summary>
        public static readonly string KEY_DAILYDEAL_TITLE = "DAILYDEAL_TITLE";
        /// <summary>
        /// DailyDeal标语库语种Key
        /// </summary>
        public static readonly string KEY_DAILYDEAL_TITLE_LANGUAGE = "DAILYDEAL_TITLE_LANGUAGE"; 
        #endregion

        #region 后台
        /// <summary>
        /// 后台菜单Key
        /// </summary>
        public static readonly string KEY_ADMIN_MENUS = "ADMIN_MENUS";
        #endregion

        #endregion

        #region 方法

        /// <summary>
        /// 转换为指定对象的key，比如产品888的key为PRODUCT_8888
        /// </summary>
        /// <param name="baseKey">对象基础的key，比如产品调用MemcachedConst.KEY_PRODUCT得到</param>
        /// <param name="id">对象Id,产品Id为8888</param>
        /// <returns></returns>
        public static string ToObjectKey(string baseKey, object id)
        {
            return string.Format("{0}_{1}", baseKey, id);
        }

        /// <summary>
        /// 转换为指定对象的key，比如产品888的key为PRODUCT_8888_1
        /// </summary>
        /// <param name="baseKey">对象基础的key，比如产品调用MemcachedConst.KEY_PRODUCT得到</param>
        /// <param name="id">对象Id,产品Id为8888</param>
        /// <param name="languageId">语种ID,比如德语为2</param>
        /// <returns></returns>
        public static string ToObjectKey(string baseKey, object id, int languageId)
        {
            return ToObjectKey(baseKey, id, languageId.ToString());
        }

        /// <summary>
        /// 转换为指定对象的key，比如产品888的key为PRODUCT_8888_DE
        /// </summary>
        /// <param name="baseKey">对象基础的key，比如产品调用MemcachedConst.KEY_PRODUCT得到</param>
        /// <param name="id">对象Id,产品Id为8888</param>
        /// <param name="languageCode">语种ID,比如德语为DE</param>
        /// <returns></returns>
        public static string ToObjectKey(string baseKey, object id, string languageCode)
        {
            return string.Format("{0}_{1}_{2}", baseKey, id, languageCode);
        }

        /// <summary>
        /// 转换为指定对象的key，比如所有产品的key为ALL_PRODUCT
        /// </summary>
        /// <param name="baseKey">对象基础的key，比如产品调用MemcachedConst.KEY_PRODUCT得到</param> 
        /// <returns></returns>
        public static string ToAllKey(string baseKey)
        {
            return string.Format("{0}_{1}", KEY_ALL, baseKey);
        }

        /// <summary>
        /// 转换为指定对象的key，比如所有德语产品的key为ALL_PRODUCT_2
        /// </summary>
        /// <param name="baseKey">对象基础的key，比如产品调用MemcachedConst.KEY_PRODUCT得到</param> 
        /// <param name="languageId">语种ID,比如德语为2</param>
        /// <returns></returns>
        public static string ToAllKey(string baseKey, int languageId)
        {
            return ToAllKey(baseKey, languageId.ToString());
        }

        /// <summary>
        /// 转换为指定对象的key，比如所有产品的key为ALL_PRODUCT_DE
        /// </summary>
        /// <param name="baseKey">对象基础的key，比如产品调用MemcachedConst.KEY_PRODUCT得到</param> 
        /// <param name="languageCode">语种ID,比如德语为DE</param>
        /// <returns></returns>
        public static string ToAllKey(string baseKey, string languageCode)
        {
            return string.Format("{0}_{1}_{2}", KEY_ALL, baseKey, languageCode);
        }
        #endregion
    }
}
