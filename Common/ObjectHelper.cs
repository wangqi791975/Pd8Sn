using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Com.Panduo.Common
{
    /// <summary>
    /// 对象辅助类
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        /// 对象间属性值复制
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <typeparam name="TTarget">目标数据类型</typeparam>
        /// <param name="source">源数据</param>
        /// <returns></returns>
        public static TTarget CopyProperties<TSource, TTarget>(this TSource source)
            where TTarget : class, new()
            where TSource : class
        {
            return source.CopyProperties<TSource, TTarget>(null);
        }

        /// <summary>
        /// 对象间属性值复制
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <typeparam name="TTarget">目标数据类型</typeparam>
        /// <param name="source">源数据</param>
        /// <param name="ignoreProperties">要忽略的属性名称</param>
        /// <returns></returns>
        public static TTarget CopyProperties<TSource, TTarget>(this TSource source, string[] ignoreProperties)
            where TTarget : class, new()
            where TSource : class
        {
            return source.CopyProperties(default(TTarget), ignoreProperties);
        }

        /// <summary>
        /// 对象间属性值复制
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <typeparam name="TTarget">目标数据类型</typeparam>
        /// <param name="source">源数据</param>
        /// <param name="target">咪表数据</param>
        /// <param name="ignoreProperties">要忽略的属性名称</param>
        /// <returns></returns>
        public static TTarget CopyProperties<TSource, TTarget>(this TSource source, TTarget target, string[] ignoreProperties)
            where TTarget : class, new()
            where TSource : class
        { 
            if (source != null)
            {
                if (target == null)
                { 
                    target = new TTarget();
                }

                var properties = source.GetType().GetProperties();
                var targetType = target.GetType();
                var targetProperties = targetType.GetProperties();

                foreach (var propertyInfo in properties)
                {
                    var name = propertyInfo.Name;
                    var type = propertyInfo.PropertyType;
                    //if ((type.IsValueType || type.Equals(typeof(string))) && (ignoreProperties == null || !ignoreProperties.Any(c => c.ToLower().Equals(name.ToLower()))))
                    if ((ignoreProperties == null || !ignoreProperties.Any(c => c.ToLower().Equals(name.ToLower()))))
                    {
                        var targetProperty = targetType.GetProperty(name) ?? targetProperties.FirstOrDefault(c => string.Equals(name, c.Name, StringComparison.InvariantCultureIgnoreCase));
                        if (targetProperty != null && targetProperty.CanWrite && (targetProperty.PropertyType.Equals(type) || type.FullName.IndexOf(targetProperty.PropertyType.FullName) > 0 || targetProperty.PropertyType.FullName.IndexOf(type.FullName) > 0))
                        {
                            var value = propertyInfo.GetValue(source, null);
                            targetProperty.SetValue(target, value, null);
                        }
                    }
                }
            }

            return target;
        }

        /// <summary>
        /// 复制列表数据
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <typeparam name="TTarget">目标数据类型</typeparam>
        /// <param name="source">源数据列表</param>
        /// <param name="ignoreProperties">要忽略的属性名称</param>
        /// <returns></returns>
        public static IList<TTarget> CopyPropertiesList<TSource, TTarget>(this IList<TSource> source, string[] ignoreProperties)
            where TTarget : class, new()
            where TSource : class
        {
            var returnList = new List<TTarget>();
            if (source != null && source.Count > 0)
            {
                foreach (var objSource in source)
                {
                    var obj = CopyProperties<TSource, TTarget>(objSource, ignoreProperties);
                    if (obj != null)
                    {
                        returnList.Add(obj);
                    }
                }
            }
            return returnList;
        }

        /// <summary>
        /// 复制列表数据
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <typeparam name="TTarget">目标数据类型</typeparam>
        /// <param name="source">源数据列表</param> 
        /// <returns></returns>
        public static IList<TTarget> CopyPropertiesList<TSource, TTarget>(this IList<TSource> source)
            where TTarget : class, new()
            where TSource : class
        {
            return source.CopyPropertiesList<TSource, TTarget>(null);
        }

        /// <summary>
        /// 判断两个对象的属性值是否一样并获取它们之间的差异组成的字符
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <param name="exclude"></param>
        /// <returns></returns>
        public static string GetChangedValue<T>(this T obj1, T obj2, string[] exclude) where T : class
        {
            var result = new StringBuilder();
            if (obj1 != null && obj2 != null && obj1.GetType().Equals(obj2.GetType()))
            {
                var properties = obj1.GetType().GetProperties();
                foreach (var propertyInfo in properties)
                {
                    var value1 = propertyInfo.GetValue(obj1, null);
                    var value2 = propertyInfo.GetValue(obj2, null);
                    var name = propertyInfo.Name;
                    var dispalyName = propertyInfo.GetDisplayName();
                    var type = propertyInfo.PropertyType;
                    if ((type.IsValueType || type.Equals(typeof(string))) && (exclude == null || !exclude.Any(c => c.ToLower().Equals(name.ToLower()))))
                    {
                        if ((value2 != null && !value2.Equals(value1)) || (value1 != null && !value1.Equals(value2)))
                        {
                            result.AppendLine(string.Format("{0}:[{1}]->[{2}]", string.IsNullOrEmpty(dispalyName) ? name : dispalyName, value1, value2));
                        }
                    }
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// 获取一个对象的属性值键值对
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetProperyKeyValue<T>(this T obj) where T : class
        {
            var result = new List<string>();
            if (obj != null)
            {
                var properties = obj.GetType().GetProperties();
                foreach (var propertyInfo in properties)
                {
                    var value = propertyInfo.GetValue(obj, null);
                    var name = propertyInfo.Name;
                    var type = propertyInfo.PropertyType;
                    if (type.IsValueType || type.Equals(typeof(string)))
                    {
                        result.Add(string.Format("[{0}:{1}]", name, value));
                    }
                }
            }

            return result.Aggregate((a, b) => a + "," + b);
        }

        /// <summary>
        /// 获取显示名
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static string GetDisplayName(this MemberInfo member)
        {
            var attribute = member.GetCustomAttributes(typeof(DisplayNameAttribute), false).SingleOrDefault() as DisplayNameAttribute;

            return attribute == null ? string.Empty : attribute.DisplayName;
        }

        public static T GetAttribute<T>(this MemberInfo member, bool isRequired) where T : Attribute
        {
            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null && isRequired)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The {0} attribute must be defined on member {1}",
                        typeof(T).Name,
                        member.Name));
            }

            return (T)attribute;
        }

        public static string GetPropertyDisplayName<T>(Expression<Func<T, object>> propertyExpression)
        {
            var memberInfo = GetPropertyInformation(propertyExpression.Body);
            if (memberInfo == null)
            {
                throw new ArgumentException(
                    "No property reference expression was found.",
                    "propertyExpression");
            }

            var attr = memberInfo.GetAttribute<DisplayNameAttribute>(false);
            if (attr == null)
            {
                return memberInfo.Name;
            }

            return attr.DisplayName;
        }

        public static MemberInfo GetPropertyInformation(Expression propertyExpression)
        {
            Debug.Assert(propertyExpression != null, "propertyExpression != null");
            MemberExpression memberExpr = propertyExpression as MemberExpression;
            if (memberExpr == null)
            {
                UnaryExpression unaryExpr = propertyExpression as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                {
                    memberExpr = unaryExpr.Operand as MemberExpression;
                }
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
            {
                return memberExpr.Member;
            }

            return null;
        }
    }
}
