using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Service;

namespace Com.Panduo.Web.Models.Product
{
    public class ProductGirdVo
    {
        /// <summary>
        /// 产品信息
        /// </summary>
        public Service.Product.Product Product { get; set; }



        /// <summary>
        /// 产品信息
        /// </summary>
        public Service.Product.ProductInfo ProductInfo { get; set; }

        /// <summary>
        /// 样式索引后缀
        /// </summary>
        public string ClassIndex { get; set; }
    }
}