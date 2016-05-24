using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.SystemMail;
using Com.Panduo.Web.Common;

namespace Com.Panduo.Web.Controllers
{
    public class MailLogoController : Controller
    {
        public ActionResult Index()
        {
            var languages = ServiceFactory.ConfigureService.GetAllValidLanguage();
            ViewBag.Languages = languages;
            return View();
        }

        public ActionResult GetList()
        {
            int page = Request["page"].ParseTo(1);
            int pageSize = Request["pageSize"].ParseTo(20);
            var languageId = Request["languageId"].ParseTo(1);
            var sorterCriteria = new List<Sorter<MailLogoSorterCriteria>>()
            {
                new Sorter<MailLogoSorterCriteria>()
                {Key=MailLogoSorterCriteria.CreateTime,IsAsc = false}
            };
            var model = ServiceFactory.MailService.GetMailLogoList(page, pageSize, languageId, sorterCriteria);
            return View(model);
        }

        public JsonResult Delete()
        {
            var hashtable = new Hashtable();
            hashtable.Add("error", false);
            hashtable.Add("msg", string.Empty);
            var cId = Request["ids"].ParseTo(-1);//logo id
            try
            {
                ServiceFactory.MailService.DeleteMailLogo(cId);
                hashtable.Add("getlist", true);
                hashtable.Add("url", "/MailLogo/Index");
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
            }
            return Json(hashtable);
        }

        public JsonResult UploadLogo()
        {
            var hashtable = new Hashtable();
            hashtable.Add("msg", "上传成功!");
            hashtable.Add("error", false);
            int languageId= Request["HID_languageId"].ParseTo(1);
            try
            {
                foreach (string upload in Request.Files)
                {               
                    var file = Request.Files[upload];
                    var filename = Guid.NewGuid() + Path.GetExtension(file.FileName);//保存文件名

                    string filePath = Path.Combine(UploadFileHelper.GetUploadFileSavePath(UploadFileType.MailLogo), filename);
                    file.SaveAs(filePath);
                   int i= ServiceFactory.MailService.AddMailLogo(new MailLogo()
                    {
                        Creator = SessionHelper.CurrentAdminUser.AdminId,
                        LogoImage =UploadFileHelper.GetUploadFileRelatePath(filePath),
                        LanguageId = languageId,
                        CreateTime=DateTime.Now,
                        LogoUrl = string.Empty,
                        IsUse=false,
                    });
                    if (i > 0)
                    {
                        ServiceFactory.MailService.UseMailLogo(i);
                    }
                    hashtable.Add("getlist", true);
                    hashtable.Add("url", "/MailLogo/Index");
                }
            }
            catch (BussinessException bussinessException)
            {
                hashtable["msg"] = BaseController.ActionJsonResult.Error;
                hashtable["error"] = true;
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
                return Json(hashtable);
            }
            return Json(hashtable);
        }

        public JsonResult Use()
        {
            var hashtable = new Hashtable();
            hashtable.Add("error", false);
            hashtable.Add("msg", string.Empty);
            var cId = Request["logoid"].ParseTo(-1);//
            var linkurl = Request["linkurl"] ?? string.Empty;
            try
            {
                MailLogo l = new MailLogo()
                {
                    LogoUrl = linkurl,
                    LogoId = cId,
                };
                ServiceFactory.MailService.UpdateMailLogo(l);
                ServiceFactory.MailService.UseMailLogo(cId);
                hashtable.Add("getlist", true);
                hashtable.Add("url", "/MailLogo/Index");
            }
            catch (BussinessException bussinessException)
            {
                switch (bussinessException.GetError())
                {
                    case "":
                        break;
                }
            }
            return Json(hashtable);
        }

        public JsonResult UpdateMailLogo()
        {
            var hashtable = new Hashtable();
            hashtable.Add("error", false);
            hashtable.Add("msg", string.Empty);
            var id = Request["logoid"].ParseTo(-1);//
            var linkurl = Request["linkurl"] ?? string.Empty;
            try
            {
                MailLogo l = new MailLogo()
                {
                    LogoUrl = linkurl,
                    LogoId = id,
                };
                ServiceFactory.MailService.UpdateMailLogo(l);
                hashtable.Add("getlist", true);
                hashtable.Add("url", "/MailLogo/Index");
            }
            catch (BussinessException bussinessException)
            {
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
