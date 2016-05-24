using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Order.ShippingOption;
using NUnit.Framework;

namespace Com.Panduo.Tests
{
    /// <summary>
    ///这是 IShippingServiceTest 的测试类
    /// </summary>
    [TestFixture()]
    public class IShippingServiceTest : SpringTest
    {

        /// <summary>
        /// 通过国家二级简码和配送方式ID得到客户填写关税号的信息
        /// </summary>
        /// <returns></returns>
        [Test]
        public void GetCustomsNo()
        {
            var customsNo = ServiceFactory.ShippingService.GetCustomsNo(4, 30);
            if (customsNo != null)
            {
                Console.WriteLine(string.Format(customsNo.CustomsNoType.ToString()));
                Console.WriteLine((int) customsNo.CustomsNoType);
            }
            Console.WriteLine("");
            Hashtable table = new Hashtable();
            table.Add("Material", "Glass");
            table.Add("Hole Size", "1.5mm");
            table.Add("Thickness", "8mm(3/9)");
            table.Add("Color", "Multicolor");
            table.Add("Note", "This product is nicket");
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string json = jss.Serialize(table);
            Console.WriteLine(json);
        }

        /// <summary>
        /// 通过语种ID得到当前配送方式多语种（IIS缓存）
        /// </summary>
        /// <returns></returns>
        [Test]
        public void GetAllShippingDescs()
        {
            var list = ServiceFactory.ShippingService.GetAllShippingDescs(1);
            Console.WriteLine(list.Count);
            foreach (var shipping in list)
            {
                Console.WriteLine(shipping.Name);
            }
        }


        /// <summary>
        /// 获取所有的运送方式信息（前台IIS缓存用）
        /// </summary>
        /// <returns></returns>
        [Test]
        public void GetAllShippings()
        {
            var list = ServiceFactory.ShippingService.GetAllShippings();
            Console.WriteLine(list.Count);
            foreach (var shipping in list)
            {
                Console.WriteLine("Id:" + shipping.ShippingId);
            }
        }


        /// <summary>
        /// 地址信息里是否包含P.O.BOX字符串
        /// </summary>
        /// <returns></returns>
        [Test]
        public void IsPoBox()
        {
            string address = "jofdhcBOxdfsfd";
            bool isBox = ServiceFactory.ShippingService.IsPoBox(address);
            Console.WriteLine(isBox);
        }

        /// <summary>
        /// 获取单个配送方式
        /// </summary>
        [Test]
        public void GetShippingAmount()
        {
            int shippingId = 53;

            ShipppingCriteria shipppingCriteria = new ShipppingCriteria();
            shipppingCriteria.CountryIsoCode2 = "US";
            shipppingCriteria.City = "City";
            shipppingCriteria.PostCode = "433312";
            shipppingCriteria.GrossWeight = 5.9206m;
            shipppingCriteria.VolumeWeight = 54.987076m;
            shipppingCriteria.ClubLevel = 0;
            shipppingCriteria.TotalAmount = 123m;

            var shippingAmount = ServiceFactory.ShippingService.GetShippingAmount(shippingId, shipppingCriteria);
            Console.WriteLine(string.Format("ShippingCode:{0}, ShippingName:{1}, ShippingCost:{2}", shippingAmount.ShippingCode, shippingAmount.ShippingName, shippingAmount.ShippingCost));
        }

        /// <summary>
        /// 获取配送方式列表
        /// </summary>
        [Test]
        public void GetShippingAmounts()
        {
            ShipppingCriteria shipppingCriteria = new ShipppingCriteria();
            shipppingCriteria.CountryIsoCode2 = "RU";
            shipppingCriteria.City = "City";
            shipppingCriteria.PostCode = "433312";
            shipppingCriteria.GrossWeight = 5.9206m;
            shipppingCriteria.VolumeWeight = 54.987076m;
            //shipppingCriteria.ClubLevel = 2;
            shipppingCriteria.ClubLevel = 4;
            shipppingCriteria.TotalAmount = 123m;

            var shippingAmounts = ServiceFactory.ShippingService.GetShippingAmounts(shipppingCriteria);
            foreach (var shippingAmount in shippingAmounts)
            {
                Console.WriteLine(string.Format("ShippingCode:{0}, ShippingName:{1}, ShippingCost:{2}, HandlingFeeForClub:{3}", shippingAmount.ShippingCode, shippingAmount.ShippingName, shippingAmount.ShippingCost, shippingAmount.HandlingFeeForClub));
            }
        }
    }
}
