using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Com.Panduo.Common;
using Com.Panduo.MailServer;
using Com.Panduo.Service;
using Com.Panduo.Web.Common;
using Resources;

namespace Com.Panduo.Web.Controllers
{
    public class ContactUsController : Controller
    {
        [HttpGet]
        public ActionResult ContactUs()
        {
            ViewBag.Sitemaps = Sitemap.GetContactUsSitemap();
            return View();
        }

        [HttpPost]
        public ActionResult ContactUs(string name, string email, string message, string attachments)
        {
            var hashtable = new Hashtable { { "emailerror", false }, { "emailmsg", string.Empty }, { "messagerror", false }, { "messagemsg", string.Empty } };
            if (email.IsEmpty())
            {
                hashtable["emailerror"] = true;
                hashtable["emailmsg"] = Lang.ErrorEmailEmpty;
            }
            if (!RegexHelper.IsEmail(email))
            {
                hashtable["emailerror"] = true;
                hashtable["emailmsg"] = Lang.ErrorEmailFormat;
            }
            if (email.Length < 5)
            {
                hashtable["emailerror"] = true;
                hashtable["emailmsg"] = Lang.ErrorEmailTooShort;
            }
            if (ServiceFactory.CustomerService.GetCustomerByEmail(email).IsNullOrEmpty())
            {
                hashtable["emailerror"] = true;
                hashtable["emailmsg"] = Lang.ErrorEmailNotExist;
            }
            if (message.IsEmpty())
            {
                hashtable["messagerror"] = true;
                hashtable["messagemsg"] = Lang.ErrorMessageEmpty;
            }

            ServiceFactory.MailService.ContactUsToCustomerEmail(name, email, message, attachments);
            ServiceFactory.MailService.ContactUsToSaleEmail(name, email, message, attachments);

            return Json(hashtable);
        }

        [HttpPost]
        public string UploadAttachment()
        {
            string fileName = string.Empty;
            HttpPostedFileBase file = Request.Files["Filedata"];
            if (file != null)
            {
                string path = UploadFileHelper.GetUploadFileSavePath(UploadFileType.ContactUs);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var extension = Path.GetExtension(file.FileName);
                if (extension != null)
                {
                    string ext = extension.ToLower();
                    fileName = DateTime.Now.ToFileTime() + ext;
                    file.SaveAs(path + "\\" + fileName);
                }
            }
            return fileName;
        }

        [HttpPost]
        public ActionResult CheckedEmail(string email)
        {
            var hashtable = new Hashtable { { "isExist", true }, { "msg", string.Empty } };
            if (ServiceFactory.CustomerService.GetCustomerByEmail(email).IsNullOrEmpty())
            {
                hashtable["isExist"] = false;
                hashtable["msg"] = Lang.ErrorEmailNotExist;
            }
            return Json(hashtable);
        }
    }
}
