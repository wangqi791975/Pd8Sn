using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Com.Panduo.Web.Common; 

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// 网站缓存辅助类
    /// </summary>
    public partial class CacheManager
    {
        #region 公用方法
        private volatile Cache _siteCache = null;
        public static readonly CacheManager Instance = new CacheManager();

        private CacheManager()
        {
            _siteCache = HttpRuntime.Cache;
        }

        /// <summary>
        /// 获取Cache值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetCache(string key)
        {
            return _siteCache.Get(key);
        }

        /// <summary>
        /// 设置Cache值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetCache(string key, object value)
        {
            SetCache(key, value, null, TimeSpan.FromDays(1));
        }

        /// <summary>
        /// 有过期时间戳的设置Cache值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间戳</param>
        public void SetCache(string key, object value, TimeSpan expire)
        {
            SetCache(key, value, null, expire);
        }

        /// <summary>
        /// 有缓存依赖的设置Cache值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheDependency">缓存依赖</param>
        public void SetCache(string key, object value,CacheDependency cacheDependency)
        {
            SetCache(key, value, cacheDependency, TimeSpan.FromDays(1));
        }

        /// <summary>
        /// 有过期时间和缓存以来的设置Cache值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheDependency">缓存依赖</param>
        /// <param name="expire">过期时间</param>
        public void SetCache(string key, object value, CacheDependency cacheDependency, TimeSpan expire)
        {
            if (GetCache(key) == null)
            {
                _siteCache.Insert(key, value, cacheDependency, Cache.NoAbsoluteExpiration, expire, CacheItemPriority.NotRemovable, null);
            }
            else
            {
                _siteCache.Remove(key);
                _siteCache.Insert(key, value, cacheDependency, Cache.NoAbsoluteExpiration, expire, CacheItemPriority.NotRemovable, null);
            }
        }

        /// <summary>
        /// 清除所有Cache
        /// </summary>
        public void ClearAllCache()
        {
            var cacheBox = _siteCache.GetEnumerator();
            while (cacheBox.MoveNext())
            {
                _siteCache.Remove(cacheBox.Key.ToString());
            }
        }
        
        /// <summary>
        /// 清除指定键的Cache
        /// </summary>
        /// <param name="key"></param>
        public void ClearCache(string key)
        {
            _siteCache.Remove(key);
        }

        /// <summary>
        /// 获取所有的IIS缓存Key
        /// </summary>
        /// <returns></returns>
        public IList<string> GetAllCacheKey()
        {
            var list = new List<string>();
            var cacheBox = _siteCache.GetEnumerator();
            while (cacheBox.MoveNext())
            {
                list.Add(cacheBox.Key.ToString());
            }

            return list;
        }
        #endregion 
    }
}
