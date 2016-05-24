using System;

namespace Com.Panduo.Service.SiteConfigure
{
    /// <summary>
    /// 国家信息
    /// </summary>
     [Serializable]
    public class Country
    {
        /// <summary>
        /// 国家ID
        /// </summary>
        public virtual int CountryId
        {
            get;
            set;
        }

        /// <summary>
        /// 国家名称
        /// </summary>
        public virtual string CountryName
        {
            get;
            set;
        }

        /// <summary>
        /// 二位简码
        /// </summary>
        public virtual string SimpleCode2
        {
            get;
            set;
        }

        /// <summary>
        /// 三位简码
        /// </summary>
        public virtual string SimpleCode3
        {
            get;
            set;
        }

        /// <summary>
        /// 显示格式
        /// </summary>
        public virtual string DisplayFormat
        {
            get;
            set;
        }

        /// <summary>
        /// 显示格式1（待定）
        /// </summary>
        public virtual string DisplayFormat1
        {
            get;
            set;
        }


        /// <summary>
        /// 国家类型(是否常用国家)
        /// </summary>
        public virtual bool IsCommonCountry
        {
            get;
            set;
        }

        /// <summary>
        /// 排序（针对常用国家排序，非常用国家按字母排序A-Z）
        /// </summary>
        public virtual int DisplayOrder
        {
            get;
            set;
        }

        /// <summary>
        /// 洲ID
        /// </summary>
        public virtual int ContinentId
        {
            get;
            set;
        }

        /// <summary>
        /// 区域ID
        /// </summary>
        public virtual int AreaId
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        public virtual bool IsDisplay
        {
            get;
            set;
        }

    }
}
