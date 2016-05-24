using System;

namespace Com.Panduo.Service.Product
{
    /// <summary>
    /// 产品上货信息
    /// </summary>
    [Serializable]
    public class UploadProduct
    {
        /// <summary>
        /// 产品线
        /// </summary>
        public virtual string ProductClass { get; set; }
        /// <summary>
        /// 货号
        /// </summary>
        public virtual string ProductCode { get; set; }
        /// <summary>
        /// 上货价格
        /// </summary>
        public virtual decimal Price { get; set; }
        /// <summary>
        /// 属性名
        /// </summary>
        public virtual string PropertyName { get; set; }
        /// <summary>
        /// 价格阶梯
        /// </summary>
        public virtual decimal PriceStep { get; set; }
        /// <summary>
        /// 产品重量（每组）
        /// </summary>
        public virtual decimal Weight { get; set; }
        /// <summary>
        /// 体积重量（每组）
        /// </summary>
        public virtual decimal VolumeWeight { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public virtual string[] PropertyValues { get; set; }
        /// <summary>
        /// 英文名
        /// </summary>
        public virtual string ProductEnName { get; set; }
        /// <summary>
        /// 英文描述
        /// </summary>
        public virtual string ProductEnDesc { get; set; }
        /// <summary>
        /// 德文名
        /// </summary>
        public virtual string ProductDeName { get; set; }
        /// <summary>
        /// 德文描述
        /// </summary>
        public virtual string ProductDeDesc { get; set; }
        /// <summary>
        /// 俄文名
        /// </summary>
        public virtual string ProductRuName { get; set; }
        /// <summary>
        /// 俄文描述
        /// </summary>
        public virtual string ProductRuDesc { get; set; }
        /// <summary>
        /// 法文名
        /// </summary>
        public virtual string ProductFrName { get; set; }
        /// <summary>
        /// 法文描述
        /// </summary>
        public virtual string ProductFrDesc { get; set; }
        /// <summary>
        /// 西语名
        /// </summary>
        public virtual string ProductEsName { get; set; }
        /// <summary>
        /// 西文描述
        /// </summary>
        public virtual string ProductEsDesc { get; set; }
        /// <summary>
        /// 意大利语名
        /// </summary>
        public virtual string ProductLtName { get; set; }
        /// <summary>
        /// 意大利语描述
        /// </summary>
        public virtual string ProductLtDesc { get; set; }
        /// <summary>
        /// 日语名
        /// </summary>
        public virtual string ProductJpName { get; set; }
        /// <summary>
        /// 日语描述
        /// </summary>
        public virtual string ProductJpDesc { get; set; }
        /// <summary>
        /// 限制库存
        /// </summary>
        public virtual string LimitStock { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        public virtual decimal StockQty { get; set; }




    }

}