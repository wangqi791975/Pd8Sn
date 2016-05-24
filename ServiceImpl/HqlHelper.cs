using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Common;

namespace Com.Panduo.ServiceImpl
{
    /// <summary>
    /// Hql辅助类
    /// </summary>
    public class HqlHelper
    {
        private static readonly string VALUE_SORT_ASC = "asc";
        private static readonly string VALUE_SORT_DESC = "desc";
        private static readonly IDictionary<HqlOperator, string> HqlOperatorMap = new Dictionary<HqlOperator, string>
            {
                {HqlOperator.Eq, "="},
                {HqlOperator.Neq, "!="},
                {HqlOperator.Gt, ">"},
                {HqlOperator.Egt, ">="},
                {HqlOperator.Lt, "<"},
                {HqlOperator.Elt, "<="},
                {HqlOperator.Like, "like"},
                {HqlOperator.NotLike, "not like"},
                {HqlOperator.Between, "between"},
                {HqlOperator.NotBetween, "not between"},
                {HqlOperator.In, "in"},
                {HqlOperator.NotIn, "not in"},
                {HqlOperator.Empty, "is empty"},
                {HqlOperator.NotEmpty, "is not empty"}, 
            };

        private string _baseHql;
        private IList<string> _whereList;
        private IList<string> _sorterList;
        private IDictionary<string, object> _paramMap;

        /// <summary>
        /// 构造函数
        /// </summary>
        public HqlHelper()
            : this(string.Empty)
        {

        }

        /// <summary>
        /// 构造函数
        /// 不包含查询和排序的HQL字符串，比如 SELECT a FROM UserPO a
        /// </summary>
        /// <param name="baseHql"></param>
        public HqlHelper(string baseHql)
        {
            _baseHql = baseHql;
            _whereList = new List<string>();
            _sorterList = new List<string>();
            _paramMap = new Dictionary<string, object>();
        }

        /// <summary>
        /// 添加查询条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">查询字段</param>
        /// <param name="hqlOperator">查询操作符</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="paramValue">参数值</param>
        public void AddWhere<T>(string field, HqlOperator hqlOperator, string paramName, T paramValue)
        {
            if (!string.IsNullOrEmpty(field))
            {
                switch (hqlOperator)
                {
                    case HqlOperator.Eq:
                    case HqlOperator.Neq:
                        if (!typeof(T).IsValueType && paramValue == null)
                        {
                            _whereList.Add(string.Format("({0} is {1} null)", field, hqlOperator == HqlOperator.Neq ? "not" : string.Empty));
                        }
                        else
                        {
                            _whereList.Add(string.Format("({0} {1} :{2})", field, HqlOperatorMap[hqlOperator], paramName));
                            _paramMap.Add(paramName, paramValue);
                        }
                        break;
                    case HqlOperator.Gt:
                    case HqlOperator.Egt:
                    case HqlOperator.Lt:
                    case HqlOperator.Elt:
                        _whereList.Add(string.Format("({0} {1} :{2})", field, HqlOperatorMap[hqlOperator], paramName));
                        _paramMap.Add(paramName, paramValue);
                        break;
                    case HqlOperator.Like:
                    case HqlOperator.NotLike:
                        _whereList.Add(string.Format("({0} {1} :{2})", field, HqlOperatorMap[hqlOperator], paramName));
                        _paramMap.Add(paramName, "%" + paramValue + "%");
                        break;
                    case HqlOperator.In:
                    case HqlOperator.NotIn:  
                        //if (typeof(T).IsArray || typeof(T).IsGenericType )
                        if (paramValue is IEnumerable)
                        {
                            _whereList.Add(string.Format("({0} {1} (:{2}))", field, HqlOperatorMap[hqlOperator], paramName));
                            _paramMap.Add(paramName, paramValue);
                        }
                        break;
                    case HqlOperator.Between:
                    case HqlOperator.NotBetween:
                        if (typeof(T).IsArray)
                        {
                            _whereList.Add(string.Format("({0} {1} :from{2} and :to{2})", field, HqlOperatorMap[hqlOperator], paramName));
                            //todo 还未完成 
                            //_paramMap.Add("from" + paramName, Array.get(paramValue, 0));
                            //paramMap.Add("to" + paramName, Array.get(paramValue, 1));
                        }
                        break;
                    case HqlOperator.Empty:
                    case HqlOperator.NotEmpty:
                        _whereList.Add(string.Format("({0} {1})", field, HqlOperatorMap[hqlOperator]));
                        break;
                    case HqlOperator.Exp:
                        _whereList.Add(field);
                        if (!string.IsNullOrEmpty(paramName))
                        {
                            _paramMap.Add(paramName, paramValue);
                        }
                        break;
                    case HqlOperator.String:
                        _whereList.Add(field);
                        break;
                }
            }
        }

        /// <summary>
        /// 添加排序字段
        /// </summary>
        /// <param name="field"></param>
        /// <param name="isAsc"></param>
        public void AddSorter(string field, bool isAsc)
        {
            if (!string.IsNullOrEmpty(field))
            {
                _sorterList.Add(string.Format("{0} {1}", field, isAsc ? VALUE_SORT_ASC : VALUE_SORT_DESC));
            }
        }

        /// <summary>
        /// 获取不包含查询和排序的HQL字符串，比如 SELECT a FROM UserPO a
        /// </summary>
        /// <returns></returns>
        public string BaseHql
        {
            get { return _baseHql; }
            set { _baseHql = value; }
        }

        /// <summary>
        ///  获取查询条件字符串列表,比如要查询Id的语句：a.id = :id
        /// </summary>
        public IList<string> WhereList
        {
            get { return _whereList; }
        }

        /// <summary>
        /// 获取排序条件字符串列表，比如按照Id排序语句：a.id asc
        /// </summary>
        /// <returns></returns>
        public IList<String> SorterList
        {
            get { return _sorterList; }
        }

        /// <summary>
        /// 获取查询参数字典
        /// </summary>
        public IDictionary<string, object> ParamMap
        {
            get { return _paramMap; }
        }

        /// <summary>
        /// 获取组合好的Hql语句
        /// </summary>
        /// <returns></returns>
        public string Hql
        {
            get
            {
                //1.构建基本hql
                var hql = new StringBuilder(_baseHql);

                //2.构造查询条件
                if (_whereList != null && !_whereList.IsNullOrEmpty())
                {
                    hql.Append(" where ").Append(_whereList.Join(" and "));
                }

                //3.构建排序条件
                if (_sorterList != null && !_sorterList.IsNullOrEmpty())
                {
                    hql.Append(" order by ").Append(_sorterList.Join(" , "));
                }

                return hql.ToString();
            }
        }

    }

    /**
	 * Hql语句操作符
	 * @author phc
	 */
    public enum HqlOperator
    {
        /// <summary>
        /// 等于(如果参数值为null会转化为 is null)
        /// </summary>
        Eq,
        /// <summary>
        /// 不等于(如果参数值为null会转化为 is not null)
        /// </summary>
        Neq,
        /// <summary>
        /// 大于
        /// </summary>
        Gt,
        /// <summary>
        /// 大于等于
        /// </summary>
        Egt,
        /// <summary>
        /// 小于
        /// </summary>
        Lt,
        /// <summary>
        /// 小于等于
        /// </summary>
        Elt,
        /// <summary>
        /// 模糊查询Like
        /// </summary>
        Like,
        /// <summary>
        /// 模糊查询 not like 
        /// </summary>
        NotLike,
        /// <summary>
        /// 在区间内 between
        /// </summary>
        Between,
        /// <summary>
        /// 不在区间内Not between
        /// </summary>
        NotBetween,
        /// <summary>
        /// 在范围内 in
        /// </summary>
        In,
        /// <summary>
        /// 不在范围内not in
        /// </summary>
        NotIn,
        /// <summary>
        /// 空数据
        /// </summary>
        Empty,
        /// <summary>
        /// 非空数据
        /// </summary>
        NotEmpty,
        /// <summary>
        /// 符合Hql的任意表达式
        /// </summary>
        Exp,
        /// <summary>
        /// 符合Hql的任意字符串
        /// </summary>
        String
    }
}
