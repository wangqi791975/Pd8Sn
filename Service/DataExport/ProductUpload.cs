
using System;

namespace Com.Panduo.Service.DataExport
{
    /// <summary>
    ///描述：网站上货信息
    ///创建人: 万天文
    ///创建时间：04/08/2015 16:04:01
    /// </summary>
    [Serializable]
    public class ProductUpload
    {
        /// <summary>
        /// 时间区间
        /// </summary>
        public virtual string DateTimeInterval { get; set; }

        /// <summary>
        /// 网站
        /// </summary>
        public virtual string WebSite { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual string ProductModel { get; set; }

        /// <summary>
        /// 中文名称
        /// </summary>
        public virtual string ChineseName { get; set; }

        /// <summary>
        /// 产品线(产品在ERP里对应的类别)
        /// </summary>
        public virtual string CategoryErp { get; set; }
    }
}

