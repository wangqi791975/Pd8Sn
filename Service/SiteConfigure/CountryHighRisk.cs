using System;

namespace Com.Panduo.Service.SiteConfigure
{
    /// <summary>
    /// 高危国家
    /// </summary>
     [Serializable]
    public class CountryHighRisk
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
        /// 国家英文名称
        /// </summary>
        public virtual string CountryName
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime DateCreated
        {
            get;
            set;
        }

        /// <summary>
        /// 管理员ID
        /// </summary>
        public virtual int AdminId
        {
            get;
            set;
        }

    }
}
