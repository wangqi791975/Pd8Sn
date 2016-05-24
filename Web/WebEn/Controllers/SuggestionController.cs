using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Suggestion;
using Com.Panduo.Web.Common;
using Resources;

namespace Com.Panduo.Web.Controllers
{
    public class SuggestionController : Controller
    {
        //
        // GET: /Suggestion/

        [HttpPost]
        public ActionResult PostSuggestion(string objects, string attachments, string fullname, string email, string content)
        {
            var hashtable = new Hashtable { { "emailerror", false }, { "emailmsg", string.Empty }, { "suggerror", false }, { "suggmsg", string.Empty } };
            if (email.IsEmpty())
            {
                hashtable["emailerror"] = true;
                hashtable["emailmsg"] = Lang.ErrorEmailEmpty;
                return Json(hashtable);
            }
            if (!RegexHelper.IsEmail(email))
            {
                hashtable["emailerror"] = true;
                hashtable["emailmsg"] = Lang.ErrorEmailFormat;
                return Json(hashtable);
            }
            if (content.IsEmpty())
            {
                hashtable["suggerror"] = true;
                hashtable["suggmsg"] = Lang.ErrorSuggestionRequired;
                return Json(hashtable);
            }

            var suggestionContent = new SuggestionContent
            {
                LanguageId = ServiceFactory.ConfigureService.SiteLanguageId,
                FullName = fullname,
                Email = email,
                Content = content,
                CreateDateTime = DateTime.Now,
                ReplyDate = DateTime.Now,
                ReplyContent = ""
            };

            var details = objects.Split(';');
            suggestionContent.Details = new List<SuggestionDetail>();
            if (!objects.IsEmpty() && details.Length > 0)
            {
                foreach (var detail in details)
                {
                    int objectId = detail.Split(',')[0].ToInt();
                    int score = detail.Split(',')[1].ToInt();
                    suggestionContent.Details.Add(new SuggestionDetail { Score = score, ObjectId = objectId });
                }
            }
            var attachmentPaths = attachments.Split('|');
            string fileNames = string.Empty;                    //传递给邮件的附件名称集合
            suggestionContent.AttachmentList = new List<SuggestionAttachment>();
            if (attachments != string.Empty && attachmentPaths.Length > 0)
            {
                foreach (var attachmentPath in attachmentPaths)
                {
                    string path = attachmentPath.Split(':')[0];
                    string fileName = attachmentPath.Split(':')[1];
                    fileNames += fileName + ",";
                    suggestionContent.AttachmentList.Add(new SuggestionAttachment { Name = fileName, Path = path });
                }
            }
            if (!fileNames.IsEmpty())
            {
                fileNames = fileNames.Substring(0, fileNames.Length - 1);
            }

            int detailId = ServiceFactory.SuggestionService.AddSuggestionContent(suggestionContent);
            if (detailId > 0)
            {
                ServiceFactory.MailService.SuggestionToCustomerEmail(detailId, fileNames);
                ServiceFactory.MailService.SuggestionToSaleEmail(detailId, fileNames);
            }

            return Json(hashtable);
        }

        [HttpPost]
        public void UploadAttachment()
        {
            HttpPostedFileBase file = Request.Files["FileData"];                //上传文件      
            const string uploadPath = "/Attachment/Suggestion"; //得到上传路径
            if (file != null)
            {
                DateTime time = DateTime.Now;
                var extension = System.IO.Path.GetExtension(file.FileName);
                if (extension != null)
                {
                    string ext = extension.ToLower();   //上传文件的后缀（小写）

                    var uploadFileMapPath = UploadFileHelper.GetUploadFileSavePath(UploadFileType.Suggestion);
                    if (!System.IO.Directory.Exists(uploadFileMapPath))
                    {
                        System.IO.Directory.CreateDirectory(uploadFileMapPath);
                    }
                    file.SaveAs(uploadFileMapPath + "\\" + time.ToFileTime() + ext);
                    string uploadFilePath = UploadFileHelper.GetUploadFileRelatePath(uploadFileMapPath);
                    string fileName = uploadFilePath + ":" + time.ToFileTime() + ext;
                    Response.Write(fileName);
                }
            }
        }

    }
}
