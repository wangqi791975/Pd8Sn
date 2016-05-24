using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.SiteConfigure
{
    /// <summary>
    /// 系统服务
    /// </summary>
    public interface ISystemService
    {
       /// <summary>
        /// 加载缓存
       /// </summary>
        /// <param name="isLoadProductCache">是否加载产品缓存</param>
        void LoadCache(bool isLoadProductCache = false);

        /// <summary>
        /// 初始化后加载缓存
        /// </summary>
        void LoadCacheAtferInit();

        /// <summary>
        /// 清空缓存
        /// </summary>
        bool ClearAllCache();

        /// <summary>
        /// 清除指定Key的缓存
        /// </summary>
        /// <param name="key">缓存的Key</param>
        /// <returns></returns>
        bool ClearCache(string key);

        /// <summary>
        /// 是否存在指定Key对应的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsExists(string key);
    }
}
