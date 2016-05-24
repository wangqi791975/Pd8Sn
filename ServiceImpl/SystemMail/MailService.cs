using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Com.Panduo.Common;
using Com.Panduo.Entity.SystemMail;
using Panduo.Com.Email.SaveXml;
using Com.Panduo.Service;
using Com.Panduo.Service.SystemMail;
using Com.Panduo.ServiceImpl.SystemMail.Dao;
using Panduo.Com.Email.SaveXml.Config.Hander;
using Panduo.Com.Email.SaveXml.Entity;

namespace Com.Panduo.ServiceImpl.SystemMail
{
    public class MailService : IMailService
    {
        public IEmailProduceLogDao EmailProduceLogDao { private get; set; }
        public IEmailConfigDao EmailConfigDao { private get; set; }
        public IEmailLogoDao EmailLogoDao { private get; set; }
        private static readonly WebClientHelper WebClientHelper = new WebClientHelper { Timeout = 200 };

        public string AdminReadTemple(MailType emailType, IDictionary<string, string> parm,int languageId=1)
        {

            var mailTypeItemConfig = EmailSaveXmlConfig.MailTypeItemConfigList.FirstOrDefault(x => x.MailType == emailType);
            if (mailTypeItemConfig == null)
            {
                //const string subject = "MailTypeConfig-邮件配置出错-未获取该类型的配置,请检查配置文件MailConfig节点";
                //EmailDao.SaveEmailLogErrMsg(emailLog.Id, subject);
            }
            string mailContent = string.Empty;
            var language = ServiceFactory.ConfigureService.GetAllValidLanguage().FirstOrDefault(x => x.LanguageId == languageId);
            if (language == null)
            {
                //const string subject = "EmailLanguage-邮件语种出错-未匹配该邮件语种对象";
                //EmailDao.SaveEmailLogErrMsg(emailLog.Id, subject);
            }
            else
            {
                var fullUrl = string.Format("{0}/{1}", language.Host.TrimEnd('/'),
                    string.Format(mailTypeItemConfig.MessageActionUrl.TrimEnd('/')));             
                try
                {
                    if (parm.IsNullOrEmpty())
                    {
                        mailContent = WebClientHelper.DownloadString(fullUrl);
                    }
                    else
                    {
                        string postData = parm.GetQueryString();
                        byte[] bytes = Encoding.UTF8.GetBytes(postData);
                        WebClientHelper.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                        WebClientHelper.Headers.Add("ContentLength", postData.Length.ToString());
                        byte[] responseData = WebClientHelper.UploadData(fullUrl, "POST", bytes);
                        mailContent = Encoding.UTF8.GetString(responseData);
                    }
                }
                catch (Exception exception)
                {
                    //var subject = "DownloadMailContent-请求邮件内容出错,邮件Id为[" + emailLog.Id + "]-请求地址为:" + fullUrl + "\n\r,错误信息:" + exception.Message;
                    //EmailDao.SaveEmailLogErrMsg(emailLog.Id, subject);
                    //MailHelper.SendToAdmin(subject, exception.StackTrace);
                    //LoggerHelper.GetLogger(LoggerType.Exception).Error(subject, exception);
                    mailContent = "";
                }
            }
            return mailContent;
        }

        private string ReadTemple(MailType emailType, IDictionary<string, string> parm)
        {

            var mailTypeItemConfig = EmailSaveXmlConfig.MailTypeItemConfigList.FirstOrDefault(x => x.MailType == emailType);
            if (mailTypeItemConfig == null)
            {
                //const string subject = "MailTypeConfig-邮件配置出错-未获取该类型的配置,请检查配置文件MailConfig节点";
                //EmailDao.SaveEmailLogErrMsg(emailLog.Id, subject);
            }

            var language = ServiceFactory.ConfigureService.GetAllValidLanguage().FirstOrDefault(x => x.LanguageId == ServiceFactory.ConfigureService.SiteLanguageId);
            if (language == null)
            {
                //const string subject = "EmailLanguage-邮件语种出错-未匹配该邮件语种对象";
                //EmailDao.SaveEmailLogErrMsg(emailLog.Id, subject);
            }

            var fullUrl = string.Format("{0}/{1}", language.Host.TrimEnd('/'), string.Format(mailTypeItemConfig.MessageActionUrl.TrimEnd('/')));
            string mailContent = string.Empty;
            try
            {
                if (parm.IsNullOrEmpty())
                {
                    mailContent = WebClientHelper.DownloadString(fullUrl);
                }
                else
                {
                    string postData = parm.GetQueryString();
                    byte[] bytes = Encoding.UTF8.GetBytes(postData);
                    WebClientHelper.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    WebClientHelper.Headers.Add("ContentLength", postData.Length.ToString());
                    byte[] responseData = WebClientHelper.UploadData(fullUrl, "POST", bytes);
                    mailContent = Encoding.UTF8.GetString(responseData);
                }
            }
            catch (Exception exception)
            {
                //var subject = "DownloadMailContent-请求邮件内容出错,邮件Id为[" + emailLog.Id + "]-请求地址为:" + fullUrl + "\n\r,错误信息:" + exception.Message;
                //EmailDao.SaveEmailLogErrMsg(emailLog.Id, subject);
                //MailHelper.SendToAdmin(subject, exception.StackTrace);
                //LoggerHelper.GetLogger(LoggerType.Exception).Error(subject, exception);
                mailContent = "";
            }
            return mailContent;
        }

        public void AddEmailProduceLog(EmailProduceLog emailProduceLog)
        {
            EmailProduceLogDao.AddObject(new EmailProduceLogPo
            {
                KeyNo = emailProduceLog.KeyNo,
                EmailType = (int)emailProduceLog.EmailType,
                HasAttachment = emailProduceLog.HasAttachment,
                Attachment = emailProduceLog.Attachment,
                IsCreatedXml = false,
                DateCreatedXml = emailProduceLog.DateCreatedXml,
                DateAdded = emailProduceLog.DateAdded,
                LanguageId = emailProduceLog.LanguageId,
                ErrCount = emailProduceLog.ErrCount,
                ErrMsg = emailProduceLog.ErrMsg
            });
        }

        public MailLogo GetMailLogo(int logoId)
        {
            var po = EmailLogoDao.GetObject(logoId);
            return GetMailLogoVoFromPo(po);
        }

        public MailLogo GetMailLogoByLanguageId(int languageId)
        {
            var po = EmailLogoDao.GetMailLogoByLanguageId(languageId);
            return GetMailLogoVoFromPo(po);
        }

        public int AddMailLogo(MailLogo logo)
        {
            if (!logo.IsNullOrEmpty())
            {
                EmailLogoPo po = new EmailLogoPo()
                {
                    AdminId = logo.Creator,
                    DateCreated = DateTime.Now,
                    LanguageId = logo.LanguageId,
                    LogoImage = logo.LogoImage,
                    LogoLink = logo.LogoUrl,
                };
                return EmailLogoDao.AddObject(po);
            }
            return 0;
        }

        public void UpdateMailLogo(MailLogo logo)
        {
            if (!logo.IsNullOrEmpty())
            {
                var m = EmailLogoDao.GetObject(logo.LogoId);
                if (!m.IsNullOrEmpty())
                {
                    m.LogoLink = logo.LogoUrl;
                    EmailLogoDao.UpdateObject(m);
                }
            }
        }

        public PageData<MailLogo> GetMailLogoList(int currentPage, int pageSize, int languageId, IList<Sorter<MailLogoSorterCriteria>> sorterCriteria)
        {
            var list = new List<MailLogo>();
            var polist = EmailLogoDao.GetMailLogoList(currentPage, pageSize, languageId, sorterCriteria);
            if (!polist.IsNullOrEmpty())
            {
                foreach (var po in polist.Data)
                {
                    list.Add(GetMailLogoVoFromPo(po));
                }
            }
            return new PageData<MailLogo>
            {
                Pager = polist.Pager,
                Data = list,
            };
        }

        public void DeleteMailLogo(int logoId)
        {
            EmailLogoDao.DeleteObjectById(logoId);
        }

        public void UseMailLogo(int logoId)
        {
            EmailLogoDao.UseMailLogo(logoId);
        }


        /// <summary>
        /// 根据邮件类型获取邮件配置
        /// </summary>
        /// <param name="type">邮件类型</param>
        /// <param name="languageId">不区分语种，传英语站点Id</param>
        /// <returns></returns>
        public MailConfig GetMailConfig(MailType type, int languageId)
        {
            var po = EmailConfigDao.GetMailConfig((int)type, languageId);
            if (!po.IsNullOrEmpty())
            {
                return new MailConfig()
                {
                    Receiver = po.Receiver,
                    Cc = po.EmailCc,
                    Creator = po.AdminId,
                    CreateTime = po.DateCreated,
                    LanguageId = po.LanguageId,
                    From = po.EmailForm,
                    FromName = po.FromName,
                    To = po.EmailTo,
                    ToName = po.ToName,
                    Type = po.Type.ToEnum<MailType>()
                };
            }
            return null;
        }

        #region 邮件发送
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="type">邮件类型</param>
        /// <param name="languageId">语种（不区分语种，传英语站点Id）</param>
        /// <param name="mail">邮件实体</param>
        private void SendMail(MailType type, int languageId, EmailXmlData mail)
        {
            List<string> clist = new List<string>();
            List<string> tlist = new List<string>();
            List<string> flist = new List<string>();
            var config = GetMailConfig(type, languageId);


            if (config.IsNullOrEmpty())
            {
                throw new BussinessException("缺少邮件数据库配置信息");
            }
            foreach (var c in config.Cc.Split(","))
            {
                clist.Add(c);
            }
            if (config.Receiver == 1)
            {
                foreach (var f in config.From.Split(","))
                {
                    flist.Add(f);
                }
                if (mail.RecipientList.IsNullOrEmpty() || mail.RecipientList.Count < 1)
                {
                    mail.RecipientList = flist;
                }
            }
            else
            {
                foreach (var t in config.To.Split(","))
                {
                    tlist.Add(t);
                }
                if (mail.RecipientList.IsNullOrEmpty() || mail.RecipientList.Count < 1)
                {
                    mail.RecipientList = tlist;
                }
            }

            if (mail.CcList.IsNullOrEmpty() || mail.CcList.Count < 1)
            {
                mail.CcList = clist;
            }
            EmailService.SerializerXml(mail);
        }


        /// <summary>
        /// 构建邮件内容
        /// </summary>
        /// <param name="type">邮件类型</param>
        /// <param name="dic">替换标签字典</param>
        /// <param name="tplmailContent">被替换模板内容</param>
        /// <param name="email">发件人</param>
        /// <param name="displayName">发件人名称</param>
        /// <param name="attachments">文件名列表（全路径）</param>
        /// <returns></returns>
        private EmailXmlData BuildEmailXmlData(MailType type, IDictionary<string, string> dic, string tplmailContent, string email, string displayName = "", List<MailAttachments> attachments = null)
        {
            var mailId = Guid.NewGuid().ToString("N");
            var title = MailTemplateHelper.GetTagValue(tplmailContent, "<!--<tilte>(.*)</tilte>-->");//获取标题
            tplmailContent = MailTemplateHelper.RemoveTag(tplmailContent, "<!--<tilte>(.*)</tilte>-->");//干掉标题
            tplmailContent = MailTemplateHelper.ReplaceTemplateTag(tplmailContent, dic);
            var xmlObj = new EmailXmlData
            {
                Type = type,
                MailId = mailId,
                Title = title,
                MailContent = tplmailContent,
                SendDateTime = DateTime.Now,
                Sender = email,
                FromName = displayName,
                LanguageId = ServiceFactory.ConfigureService.SiteLanguageId,
                Attachments = attachments,
            };
            return xmlObj;
        }


        /// <summary>
        /// 获取邮件附件列表
        /// </summary>
        /// <param name="uploadFileType">上传文件类型（用于目录获取）</param>
        /// <param name="mailType">邮件类型，（生成发邮件目录）</param>
        /// <param name="attachmentfile">附件文件名列表（逗号分隔）</param>
        /// <returns></returns>
        private List<MailAttachments> GetMailAttachments(UploadFileType uploadFileType, MailType mailType, string attachmentfile)
        {
            List<MailAttachments> attachments = new List<MailAttachments>();
            var files = attachmentfile.Split(',', '|');
            var filedir = UploadFileHelper.GetUploadFileSavePath(uploadFileType);
            foreach (var f in files)
            {
                var attachmentfilepath = Path.Combine(filedir, f);
                if (File.Exists(attachmentfilepath))
                {
                    var emailConfig = EmailSaveXmlConfig.MailTypeItemConfigList.FirstOrDefault(
                        x => x.MailType == mailType);
                    if (emailConfig.IsNullOrEmpty())
                    {
                        throw new BussinessException();
                    }
                    else
                    {
                        var savePath = Path.Combine(EmailSaveXmlConfig.EmailAttachmentSavePath,
                            emailConfig.AttachmentFolder);
                        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
                        savePath = Path.Combine(savePath, f);
                        File.Copy(attachmentfilepath, savePath);
                        FileInfo fileInfo = new FileInfo(attachmentfilepath);
                        attachments.Add(new MailAttachments()
                        {
                            AttachmentSize = fileInfo.Length,
                            CreateTime = fileInfo.CreationTime,
                            AttachmentAddress = savePath,
                            AttachmentName = f,
                        });
                    }
                }
            }
            return attachments;
        }
        #endregion

        #region Sourcing

        public void CustomerSourcingInformation(string productTitle, string content, string fullname, string email, string files)
        {
            var parm = new Dictionary<string, string>();
            parm.Add("title", productTitle);
            parm.Add("content", content);
            parm.Add("fullname", fullname);
            parm.Add("email", email);
            var emailType = MailType.OemAndSourcingToCustomer;
            var mailContent = ReadTemple(emailType, parm);

            IDictionary<string, string> dic = new Dictionary<string, string>
                {
                    {"<!--[$fullname$]-->",fullname},
                };
            var xmlData = BuildEmailXmlData(emailType, dic, mailContent, email, fullname, GetMailAttachments(UploadFileType.AddSource, emailType, files));
            SendMail(emailType, ServiceFactory.ConfigureService.SiteLanguageId, xmlData);
        }

        public void SourcingInformationToSales(string productTitle, string content, string fullname, string email, string files)
        {
            var parm = new Dictionary<string, string>();
            parm.Add("title", productTitle);
            parm.Add("content", content);
            parm.Add("fullname", fullname);
            parm.Add("email", email);
            var emailType = MailType.OemAndSourcingToSales;
            var mailContent = ReadTemple(emailType, parm);
            var xmlData = BuildEmailXmlData(emailType, null, mailContent, email, fullname, GetMailAttachments(UploadFileType.AddSource, emailType, files));
            SendMail(emailType, ServiceFactory.ConfigureService.EnglishLangId, xmlData);
        }

        #endregion

        /// <summary>
        /// Ask A Question Email
        /// </summary>
        /// <param name="fullname">客户全名</param>
        /// <param name="customeremail">客户邮箱</param>
        /// <param name="content">question内容</param>
        /// <param name="files">附件名称</param>
        public void AskQuestionEmail(string fullname, string customeremail, string content, string files)
        {
            var emailType = MailType.CustomerAskQuestion;
            List<MailAttachments> fileList = GetMailAttachments(UploadFileType.CustomerAskQuestion, emailType, files);
            var mailContent = ReadTemple(emailType, null);
            IDictionary<string, string> dic = new Dictionary<string, string>
                {
                    {"<!--[$email$]-->",customeremail},
                    {"<!--[$fullname$]-->",fullname},
                    {"<!--[$emailBody$]-->",content},
                };
            var xmlData = BuildEmailXmlData(emailType, dic, mailContent, customeremail, fullname, fileList);
            SendMail(emailType, ServiceFactory.ConfigureService.EnglishLangId, xmlData);
        }

        public void ReviewEmail(string fullname, string customeremail, DateTime addTime, string productInfo)
        {
            throw new NotImplementedException();
        }

        public void CustomerHeadEmail(string fullname, string customeremail)
        {
            var emailType = MailType.CustomerAvatarSubmitted;
            var mailContent = ReadTemple(emailType, null);
            var xmlData = BuildEmailXmlData(emailType, null, mailContent, customeremail, fullname, null);
            SendMail(emailType, ServiceFactory.ConfigureService.EnglishLangId, xmlData);
        }

        public void TestimonialEmail(string fullname, string customeremail, string testimonial)
        {
            var emailType = MailType.Testimonial;
            var parm = new Dictionary<string, string>();
            parm.Add("email", customeremail);
            parm.Add("testimonial", testimonial);
            parm.Add("customername", fullname);
            var mailContent = ReadTemple(emailType, parm);
            var xmlData = BuildEmailXmlData(emailType, null, mailContent, customeremail, fullname, null);
            SendMail(emailType, ServiceFactory.ConfigureService.EnglishLangId, xmlData);
        }

        public void ForgetPwdEmail(string fullname, string email, string resetUrl, string cancelUrl)
        {
            var emailType = MailType.CustomerPasswordReset;
            var parm = new Dictionary<string, string>();
            parm.Add("name", fullname);
            parm.Add("email", email);
            parm.Add("resetUrl", resetUrl);
            parm.Add("cancelUrl", cancelUrl);
            var mailContent = ReadTemple(emailType, parm);
            var xmlData = BuildEmailXmlData(emailType, null, mailContent, email, fullname, null);
            SendMail(emailType, ServiceFactory.ConfigureService.SiteLanguageId, xmlData);
        }

        #region Suggestion
        public void SuggestionToCustomerEmail(int detailId, string files)
        {
            var parm = new Dictionary<string, string>();
            parm.Add("contentid", detailId.ToString());
            var emailType = MailType.SuggestionToCustomer;
            var mailContent = ReadTemple(emailType, parm);
            var xmlData = BuildEmailXmlData(emailType, null, mailContent, "", "", GetMailAttachments(UploadFileType.Suggestion, emailType, files));
            SendMail(emailType, ServiceFactory.ConfigureService.SiteLanguageId, xmlData);
        }

        public void SuggestionToSaleEmail(int detailId, string files)
        {
            var parm = new Dictionary<string, string>();
            parm.Add("contentid", detailId.ToString());
            var emailType = MailType.SuggestionToSales;
            var mailContent = ReadTemple(emailType, parm);
            var xmlData = BuildEmailXmlData(emailType, null, mailContent, "", "", GetMailAttachments(UploadFileType.Suggestion, emailType, files));
            SendMail(emailType, ServiceFactory.ConfigureService.SiteLanguageId, xmlData);
        }
        #endregion

        #region Contact Us

        public void ContactUsToCustomerEmail(string fullname, string customeremail, string message, string files)
        {
            var emailType = MailType.ContactUsToCustomer;
            var parm = new Dictionary<string, string>();
            parm.Add("message", message);
            parm.Add("fullname", fullname);
            parm.Add("email", customeremail);
            var mailContent = ReadTemple(emailType, parm);
            var xmlData = BuildEmailXmlData(emailType, null, mailContent, customeremail, fullname, GetMailAttachments(UploadFileType.ContactUs, emailType, files));
            SendMail(emailType, ServiceFactory.ConfigureService.EnglishLangId, xmlData);
        }

        public void ContactUsToSaleEmail(string fullname, string customeremail, string message, string files)
        {
            var emailType = MailType.ContactUsToSales;
            var parm = new Dictionary<string, string>();
            parm.Add("message", message);
            parm.Add("fullname", fullname);
            parm.Add("email", customeremail);
            var mailContent = ReadTemple(emailType, parm);
            var xmlData = BuildEmailXmlData(emailType, null, mailContent, customeremail, fullname, GetMailAttachments(UploadFileType.ContactUs, emailType, files));
            SendMail(emailType, ServiceFactory.ConfigureService.EnglishLangId, xmlData);
        }
        #endregion

        #region MakePayment
        /// <summary>
        ///提交线下支付信息的邮件（销售）
        /// </summary>
        /// <returns></returns>
        public void MakePaymentToSalesEmail(string fullname, string email, string orderNo, string currency, string amount, string datetime, string controlNo, string remitter, string country, string files)
        {
            var parm = new Dictionary<string, string>();
            var emailType = MailType.MakePaymentToSales;
            var mailContent = ReadTemple(emailType, parm);
            var xmlData = BuildEmailXmlData(emailType, null, mailContent, email, fullname, GetMailAttachments(UploadFileType.MakePayment, emailType, files));
            SendMail(emailType, ServiceFactory.ConfigureService.EnglishLangId, xmlData);
        }

        /// <summary>
        ///提交线下支付信息的邮件（客户）
        /// </summary>
        /// <returns></returns>
        public void MakePaymentToCustomerEmail(string fullname, string email, string orderNo, string currency, string amount, string datetime, string controlNo, string remitter, string country, string files)
        {
            var parm = new Dictionary<string, string>();
            var emailType = MailType.MakePaymentToCustomer;
            var mailContent = ReadTemple(emailType, parm);
            var xmlData = BuildEmailXmlData(emailType, null, mailContent, email, fullname, GetMailAttachments(UploadFileType.MakePayment, emailType, files));
            SendMail(emailType, ServiceFactory.ConfigureService.SiteLanguageId, xmlData);
        }

        #endregion
        internal static MailLogo GetMailLogoVoFromPo(EmailLogoPo emailLogoPo)
        {
            MailLogo mailLogo = null;
            if (!emailLogoPo.IsNullOrEmpty())
            {
                mailLogo = new MailLogo
                {
                    CreateTime = emailLogoPo.DateCreated,
                    Creator = emailLogoPo.AdminId,
                    IsUse = emailLogoPo.Status,
                    LanguageId = emailLogoPo.LanguageId,
                    LogoImage = emailLogoPo.LogoImage,
                    LogoUrl = emailLogoPo.LogoLink,
                    LogoId = emailLogoPo.LogoId,
                };
            }
            return mailLogo;
        }

    }
}
