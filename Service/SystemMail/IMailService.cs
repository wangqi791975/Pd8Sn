//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：IMailService.cs
//创 建 人：罗海明
//创建时间：2015/04/08 09:40:40 
//功能说明：mail相关接口
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System;
using System.Collections.Generic;
using Panduo.Com.Email.SaveXml.Entity;

namespace Com.Panduo.Service.SystemMail
{
    public interface IMailService
    {
        #region 记录服务发邮件日志
        /// <summary>
        /// 添加邮件发送日志，服务根据日志生成邮件
        /// </summary>
        /// <param name="emailProduceLog"></param>
        void AddEmailProduceLog(EmailProduceLog emailProduceLog);
        #endregion


        #region MailLogo管理
        /// <summary>
        ///获取MailLogo
        /// </summary>
        /// <param name="logoId">logoId</param>
        MailLogo GetMailLogo(int logoId);

        /// <summary>
        /// 通过语种Id获取MailLogo
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        MailLogo GetMailLogoByLanguageId(int languageId);

        /// <summary>
        /// 添加MailLogo
        /// </summary>
        /// <param name="logo">实体</param>
        int AddMailLogo(MailLogo logo);

        /// <summary>
        /// 编辑mailLogo
        /// </summary>
        /// <param name="logo">实体</param>
        void UpdateMailLogo(MailLogo logo);

        /// <summary>
        /// 查询MailLogo
        /// </summary>
        /// <returns></returns>
        PageData<MailLogo> GetMailLogoList(int currentPage, int pageSize, int languageId, IList<Sorter<MailLogoSorterCriteria>> sorterCriteria);

        /// <summary>
        /// 删除MailLogo
        /// </summary>
        /// <param name="logoId">logoId</param>
        void DeleteMailLogo(int logoId);

        /// <summary>
        /// 使用（当前只能用一个）
        /// </summary>
        /// <param name="logoId"></param>
        void UseMailLogo(int logoId);
        #endregion

        /// <summary>
        /// 邮件模板读取，后台发邮件有需要显示编辑
        /// </summary>
        /// <param name="emailType">邮件类型</param>
        /// <param name="parm">参数(不需求传Null)</param>
        /// <param name="languageId">语种Id</param>
        /// <returns></returns>
        string AdminReadTemple(MailType emailType, IDictionary<string, string> parm, int languageId = 1);

        /// <summary>
        /// 根据邮件类型获取邮件配置
        /// </summary>
        /// <param name="type">邮件类型</param>
        /// <param name="languageId">不区分语种传0</param>
        /// <returns></returns>
        MailConfig GetMailConfig(MailType type, int languageId);

        /// <summary>
        ///1.11商品详情页面Ask a question邮件
        /// </summary>
        /// <param name="fullname">客户名称</param>
        /// <param name="customeremail">客户邮箱</param>
        /// <param name="content">内容</param>
        /// <param name="files">附件</param>
        void AskQuestionEmail(string fullname, string customeremail, string content, string files);


        /// <summary>
        /// 1.10商品详情页面-Review邮件
        /// </summary>
        /// <param name="fullname">客户名称</param>
        /// <param name="customeremail">客户邮箱</param>
        /// <param name="addTime">添加时间</param>
        /// <param name="productInfo">产品信息</param>
        void ReviewEmail(string fullname, string customeremail, DateTime addTime, string productInfo);

        /// <summary>
        /// 1.9	客户提交头像邮件
        /// </summary>
        /// <param name="fullname">客户名称</param>
        /// <param name="customeremail">客户邮箱</param>
        void CustomerHeadEmail(string fullname, string customeremail);

        /// <summary>
        /// 1.8客户提交Suggestion发给客户的邮件
        /// </summary>
        void SuggestionToCustomerEmail(int detailId, string files);

        /// <summary>
        /// 1.8客户提交Suggestion发给销售的邮件
        /// </summary>
        void SuggestionToSaleEmail(int detailId, string files);

        /// <summary>
        /// 1.7 Contact Us发给客户的邮件
        /// </summary>
        /// <param name="fullname"></param>
        /// <param name="customeremail"></param>
        /// <param name="message"></param>
        void ContactUsToCustomerEmail(string fullname, string customeremail, string message,string files);

        /// <summary>
        /// 1.7 Contact Us发给销售的邮件
        /// </summary>
        /// <param name="fullname">客户名称</param>
        /// <param name="customeremail">客户邮箱</param>
        /// <param name="message"></param>
        void ContactUsToSaleEmail(string fullname, string customeremail, string message,string files);


        /// <summary>
        /// 1.19 OEM & Sourcing发给客户的邮件
        /// </summary>
        /// <param name="productTitle"></param>
        /// <param name="content"></param>
        /// <param name="fullname"></param>
        /// <param name="email"></param>
        /// <param name="files"></param>
        void CustomerSourcingInformation(string productTitle, string content, string fullname, string email,
            string files);

        /// <summary>
        /// 1.19 OEM & Sourcing发给销售的邮件
        /// </summary>
        /// <param name="productTitle"></param>
        /// <param name="content"></param>
        /// <param name="fullname"></param>
        /// <param name="email"></param>
        /// <param name="files"></param>
        void SourcingInformationToSales(string productTitle, string content, string fullname, string email, string files);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullname"></param>
        /// <param name="customeremail"></param>
        /// <param name="testimonial"></param>
        void TestimonialEmail(string fullname, string customeremail, string testimonial);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullname"></param>
        /// <param name="email"></param>
        /// <param name="resetUrl"></param>
        /// <param name="cancelUrl"></param>
        void ForgetPwdEmail(string fullname, string email, string resetUrl, string cancelUrl);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullname"></param>
        /// <param name="email"></param>
        /// <param name="orderNo"></param>
        /// <param name="currency"></param>
        /// <param name="amount"></param>
        /// <param name="datetime"></param>
        /// <param name="controlNo"></param>
        /// <param name="remitter"></param>
        /// <param name="country"></param>
        /// <param name="files"></param>
        void MakePaymentToSalesEmail(string fullname, string email, string orderNo, string currency, string amount,
            string datetime, string controlNo, string remitter, string country, string files);

        /// <summary>
        ///提交线下支付信息的邮件（客户）
        /// </summary>
        /// <returns></returns>
        void MakePaymentToCustomerEmail(string fullname, string email, string orderNo, string currency, string amount,
            string datetime, string controlNo, string remitter, string country, string files);
    }

    /// <summary>
    /// 排序原则
    /// </summary>
    public enum MailLogoSorterCriteria
    {
        //创建时间排序
        CreateTime
    }

}
