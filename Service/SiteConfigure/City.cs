using System;

namespace Com.Panduo.Service.SiteConfigure
{
    /// <summary>
    /// 城市
    /// </summary>
    [Serializable]
    public class City
    {
        /// <summary>
        ///城市名称
        /// </summary>
        public virtual string CityName
        {
            get;
            set;
        }
        /// <summary>
        /// 所属省Id
        /// </summary>
        public virtual int ProvinceId
        {
            get;
            set;
        }
        /// <summary>
        /// 城市Id
        /// </summary>
        public virtual int CityId
        {
            get;
            set;
        }

        /// <summary>
        /// 城市编码
        /// </summary>
        public virtual string CityCode
        {
            get;
            set;
        }

    }
}
