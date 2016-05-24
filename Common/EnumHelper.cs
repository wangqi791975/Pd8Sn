using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Com.Panduo.Common
{ 
    /// <summary>
    /// 枚举辅助类
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 根据枚举名称获取枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="name">枚举名称</param>
        /// <returns></returns>
        public static T ToEnum<T>(this string name) where T : struct, IConvertible
        {
            return name.ToEnum(default(T));
        }

        /// <summary>
        /// 根据枚举名称获取枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="name">枚举名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T ToEnum<T>(this string name, T defaultValue) where T : struct, IConvertible
        {
            if (defaultValue.GetType().IsEnum && Enum.IsDefined(typeof(T), name))
            {
                return (T)Enum.Parse(typeof(T), name, true);
            }

            return defaultValue;
        }

        /// <summary>
        /// 根据枚举值获取枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">枚举值</param>
        /// <returns></returns>
        public static T ToEnum<T>(this int value) where T : struct, IConvertible
        {
            return ToEnum(value, default(T));
        }

        /// <summary>
        /// 根据枚举值获取枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">枚举值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T ToEnum<T>(this int value, T defaultValue) where T : struct, IConvertible
        {
            var map = ToEnumMap<T>();
            if (map != null && map.ContainsKey(value))
            {
                return map[value].ToEnum<T>();
            }

            return defaultValue;
        }

        /// <summary>
        /// 获取枚举键值对字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IDictionary<int, string> ToEnumMap<T>() where T : struct, IConvertible
        {
            if (typeof(T).IsEnum)
            {
                return Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(c => (int)Enum.Parse(typeof(T), c.ToString(), true), c => c.ToString());
            }

            return new Dictionary<int, string>();
        }

        /// <summary>
        /// 获取枚举值键值对
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IList<KeyValuePair<string, int>> ToEnumList<T>() where T : struct, IConvertible
        {
            if (typeof(T).IsEnum)
            {
                return Enum.GetValues(typeof(T)).Cast<T>().Select(c => new KeyValuePair<string, int>(c.ToString(), (int)Enum.Parse(typeof(T), c.ToString(), true))).ToList();
            }

            return new List<KeyValuePair<string, int>>();
        }

        /// <summary>
        /// 获取枚举值键值对
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IList<T> ToEnums<T>() where T : struct, IConvertible
        { 
            if (typeof(T).IsEnum)
            {
                return Enum.GetValues(typeof(T)).Cast<T>().Select(c=>c).ToList();
            }

            return new List<T>();
        }

        #region 取得枚举值的描述

        /// <summary>
        /// 获取枚举项目的描述
        /// </summary>
        /// <param name="obj">枚举项目</param>
        /// <returns></returns>
        public static string GetDescription(this object obj)
        {
            return GetDescription(obj, false);
        }
        /// <summary>
        /// 获取枚举项目的描述
        /// </summary>
        /// <param name="obj">枚举项目</param>
        /// <param name="isTop"></param>
        /// <returns></returns>
        public static string GetDescription(this object obj, bool isTop)
        {
            if (obj == null)
                return string.Empty;

            try
            {
                Type enumType = obj.GetType();
                DescriptionAttribute des = null;

                if (isTop)
                    des = (DescriptionAttribute)Attribute.GetCustomAttribute(enumType, typeof(DescriptionAttribute));
                else
                {
                    FieldInfo fieldInfo = enumType.GetField(System.Enum.GetName(enumType, obj));
                    des = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
                }

                if (des != null && des.Description != null)
                    return des.Description;
            }
            catch
            { }

            return obj.ToString();
        }

        #endregion
    }
}
