
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SiteConfigure
{
    /// <summary>
    ///描述：币种汇率表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:33
    /// </summary>
    [Class(Table = "t_currency", Lazy = false, NameType = typeof(CurrencyPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CurrencyPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "CurrencyId", Column = "currency_id")]
        [Generator(2, Class = "native")]
        public virtual int CurrencyId
        {
            get;
            set;
        }
        /// <summary>
        /// 中文名称
        /// </summary>
        [Property(Column = "chinese_name")]
        public virtual string ChineseName
        {
            get;
            set;
        }
        /// <summary>
        /// 币种编码
        /// </summary>
        [Property(Column = "currency_code")]
        public virtual string CurrencyCode
        {
            get;
            set;
        }
        /// <summary>
        /// 币种前缀符号(可为空)
        /// </summary>
        [Property(Column = "symbol_left")]
        public virtual string SymbolLeft
        {
            get;
            set;
        }
        /// <summary>
        /// 币种后缀符号(可为空)
        /// </summary>
        [Property(Column = "symbol_right")]
        public virtual string SymbolRight
        {
            get;
            set;
        }
        /// <summary>
        /// 保留小数点位数
        /// </summary>
        [Property(Column = "decimal_places")]
        public virtual int DecimalPlaces
        {
            get;
            set;
        }
        /// <summary>
        /// 汇率
        /// </summary>
        [Property(Column = "`value`", Precision = 18, Scale = 8)]
        public virtual decimal Value
        {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// </summary>
        [Property(Column = "`status`")]
        public virtual bool Status
        {
            get;
            set;
        }

        /// <summary>
        /// 排序
        /// </summary>
        [Property(Column = "`sort_order`")]
        public virtual int SortOrder
        {
            get;
            set;
        }


        /// <summary>
        /// 修改时间
        /// </summary>
        [Property(Column = "date_modified")]
        public virtual DateTime? DateModified
        {
            get;
            set;
        }


        /// <summary>
        /// 币种简短符号
        /// </summary>
        [Property(Column = "symbol_short")]
        public virtual string SymbolShort
        {
            get;
            set;
        }
    }
}

