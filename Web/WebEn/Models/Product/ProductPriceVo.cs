using Com.Panduo.Service.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.Models.Product
{
    [Serializable]
    public class ProductPriceVo
    {
        public ProductPrice ProductPrice { set; get; }

        public decimal Discount { set; get; }

        public bool IsDispalyOriginalPrice { set; get; }
    }
}