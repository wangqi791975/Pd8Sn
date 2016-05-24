using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Product;
using Com.Panduo.Service.Product.Property;
using Com.Panduo.Service.SiteConfigure;
using Com.Panduo.ServiceImpl;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web.Controllers
{
    public class ProductPriceRiseController : BaseController
    {
        //
        // GET: /Product/

        public ActionResult Index(int? id)
        {
            int remote = id ?? 0;
            ViewBag.Remote = remote;
            return View();
        }

        public ActionResult GetList()
        {
            IList<ProductPriceRise> productPriceRises = ServiceFactory.ProductService.GetAllProductPriceRise();

            ViewData.Model = productPriceRises;
            return View();
        }

        public ActionResult GetInfo(int id)
        {
            return View();
        }

        public JsonResult Submit()
        {
            var hashtable = new Hashtable();
            hashtable.Add("getlist", true);
            hashtable.Add("msg", "添加成功!");
            hashtable.Add("error", false);
            try
            {
                decimal reseValue = Request["rise_value"].Trim().ParseTo(0);
                if (reseValue <= 0)
                {
                    hashtable["msg"] = "上浮比例不合法!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                if (reseValue > 50)
                {
                    hashtable["msg"] = "上浮比例不能超过50!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                int adminId = 1;
                ProductPriceRise productPriceRise = new ProductPriceRise
                {
                    RiseValue = reseValue / 100,
                    DateCreated = DateTime.Now,
                    AdminId = adminId
                };
                var rises = ServiceFactory.ProductService.GetAllProductPriceRise();
                var list = rises.Where(x => x.RiseValue == productPriceRise.RiseValue).ToList();
                if (list.Count > 0)
                {
                    hashtable["msg"] = "该上浮比例已经存在!";
                    hashtable["error"] = true;
                    return Json(hashtable);
                }
                ServiceFactory.ProductService.AddProductPriceRise(productPriceRise);
                
            }
            catch (BussinessException bussinessException)
            {
                hashtable["msg"] = ActionJsonResult.Error;
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
            }
            return Json(hashtable);
        }

        public JsonResult Delete(FormCollection form)
        {
            var hashtable = new Hashtable();
            hashtable.Add("error", false);
            hashtable.Add("msg", string.Empty);
            hashtable.Add("getlist", true);
            hashtable.Add("url", "/ProductPriceRise/GetList");
            try
            {
                int id = form["ids"].ParseTo(0);
                ServiceFactory.ProductService.DeleteProductPriceRiseById(id);
            }
            catch (BussinessException bussinessException)
            {
                hashtable.Add("error", true);
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
            }

            return Json(hashtable);
        }

    }
}
