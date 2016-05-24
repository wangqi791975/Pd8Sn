using System.Collections.Generic;

namespace Com.Panduo.Service.Product
{
   public class ProductInfo
    {
       /// <summary>
       /// 产品信息
       /// </summary>
       public Product Product { get; set; }

       /// <summary>
       /// 产品价格梯度
       /// </summary>
       public ProductPrice ProductPrice { get; set; }

       /// <summary>
       /// 产品当前语种名称
       /// </summary>
       public string ProductName { get; set; }

       /// <summary>
       /// 产品英文名称
       /// </summary>
       public string ProductEnName { get; set; }

       /// <summary>
       /// 产品当前语种描述
       /// </summary>
       public string ProductDesName { get; set; }

       /// <summary>
       /// 产品库存
       /// </summary>
       public ProductStock ProductStock { get; set; }

       /// <summary>
       /// 产品图片列表
       /// </summary>
       public IList<ProductImages> ProductImages { get; set; }

       /// <summary>
       /// 单位名称
       /// </summary>
       public string UnitName { get; set; }

       /// <summary>
       /// 是否热销
       /// </summary>
       public bool IsHot { get; set; }

       /// <summary>
       /// 是否有相似商品
       /// </summary>
       public bool HasSimilarItems { get; set; }

       /// <summary>
       /// 产品属性
       /// </summary>
       public IList<KeyValuePair<Property.Property, Property.PropertyValue>> ProductProperties { get; set; }


    }
}
