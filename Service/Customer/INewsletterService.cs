using System.Collections.Generic;

namespace Com.Panduo.Service.Customer
{
    /// <summary>
    /// 订阅服务
    /// </summary>
    public interface INewsletterService
    {
        #region 常量

        /// <summary>
        /// 没有订阅信息
        /// </summary>
        string ERROR_CUSTOMER_NOT_EXIST { get; }

        #endregion

        #region 方法

        /// <summary>
        /// 订阅（对订阅邮箱做检验，判断是否已经订阅，如已订阅不insert。最终都提示订阅成功）
        /// </summary>
        /// <param name="newsletter">订阅实体</param>
        void Subscribe(Newsletter newsletter);

        /// <summary>
        /// 批量订阅（对订阅邮箱做检验，判断是否已经订阅，如已订阅不insert。最终都提示订阅成功）
        /// </summary>
        /// <param name="newsletters">订阅实体</param>
        void Subscribe(List<Newsletter> newsletters);

        /// <summary>
        /// 退订（目前只对注册用户做退订功能）
        /// </summary>
        /// <param name="email">客户email</param>
        /// <exception cref="BussinessException">
        ///     <value>ERROR_CUSTOMER_NOT_EXIST:没有订阅信息</value>
        /// </exception>
        void UnSubscribe(string email);

        /// <summary>
        /// 通过客户Id返回订阅信息
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Newsletter GetNewsletter(int customerId);

        #endregion
    }
}