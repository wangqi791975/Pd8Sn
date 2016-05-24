using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Com.Panduo.Common;

namespace Com.Panduo.Web.Common
{
    public static class CommonToolHelper
    {
        #region 字段模式
        /// <summary>
        /// 新增模式
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToAddMode(this string name)
        {
            return name.ToMode(CommonConst.AddPrefix);
        }
        /// <summary>
        /// 修改模式
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToUpdateMode(this string name)
        {
            return name.ToMode(CommonConst.UpdatePrefix);
        }
        /// <summary>
        /// 删除模式
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToDeleteMode(this string name)
        {
            return name.ToMode(CommonConst.DeletePrefix);
        }

        /// <summary>
        /// 中文模式
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToChineseMode(this string name)
        {
            return name.ToMode(CommonConst.ChinesePrefix);
        }

        /// <summary>
        /// 英文模式
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToEngilshMode(this string name)
        {
            return name.ToMode(CommonConst.EnglistPrefix);
        }

        static private string ToMode(this string name, string mode)
        {
            return mode + name;
        }
        #endregion

        #region 值获取
        /// <summary>
        /// 从NameValueCollection中获取指定属性的值填充对象
        /// </summary>
        /// <typeparam name="T">要填充对象的类型</typeparam>
        /// <param name="controller"></param>
        /// <param name="collection">数据源</param>
        /// <param name="includeProperties">需要填充的属性名称列表</param>
        /// <param name="prefix">属性名称前缀</param>
        /// <returns></returns>
        public static IList<T> TryUpdateModel<T>(this Controller controller, NameValueCollection collection, string[] includeProperties, string prefix = "") where T : new()
        {
            var list = new List<T>();
            if (collection != null && includeProperties != null)
            {
                var first = collection.GetValues(prefix + includeProperties[0]);
                var count = first == null ? 0 : first.Length;

                var type = typeof(T);
                for (var i = 0; i < count; i++)
                {
                    var model = new T();
                    foreach (var includeProperty in includeProperties)
                    {
                        var propertyName = includeProperty;
                        var property = type.GetProperty(propertyName);
                        var values = collection.GetValues(prefix + propertyName);
                        if (values != null && values.Length >= i && !string.IsNullOrEmpty(values[i]) && (property.PropertyType.IsValueType || property.PropertyType.Name.Equals("string", StringComparison.InvariantCultureIgnoreCase)))
                        {
                            property.SetValue(model, Convert.ChangeType(values[i], property.PropertyType), null);
                        }
                    }

                    list.Add(model);
                }
            }

            return list;
        }

        /// <summary>
        /// 从Json中获取对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="collection">数据源</param>
        /// <param name="name">需要填充的属性名称</param>
        /// <returns></returns>
        public static IList<T> TryGetFromJson<T>(this Controller controller, NameValueCollection collection, string name)
        {
            var list = new List<T>();
            if (collection != null && !string.IsNullOrEmpty(name))
            {
                var values = collection.GetValues(name);
                if (values != null && values.Length > 0)
                {
                    list = values.Select(c => c.FromJson<T>()).ToList();
                }
            }
            return list;
        }
        #endregion
    }
}
