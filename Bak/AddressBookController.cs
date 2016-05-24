using Com.Panduo.Service.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Panduo.Service;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web.Controllers
{
    public class AddressBookController : Controller
    {
        //
        // GET: /AddressBook/
        public ActionResult Index()
        {
            var customer = ServiceFactory.CustomerService.GetAddressesByCustomerId(SessionHelper.CurrentCustomer.CustomerId);
            return View(customer);
        }


    }
}
