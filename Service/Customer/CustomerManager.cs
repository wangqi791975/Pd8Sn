using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Customer
{
    /// <summary>
    /// 客户经理
    /// </summary>
    [Serializable]
    public class CustomerManager
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public virtual int CustomerManagerId { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 中文名字
        /// </summary>
        public virtual string ChineseName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Telphone { get; set; }
        /// <summary>
        /// skype
        /// </summary>
        public virtual string Skype { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public virtual string Avatar { get; set; }
        /// <summary>
        /// 服务邮箱
        /// </summary>
        public virtual string ServiceEmail { get; set; }
        /// <summary>
        /// 公司邮箱
        /// </summary>
        public virtual string CompanyEmail { get; set; }
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    public enum ManagerSearchCriteria
    {
        /// <summary>
        /// 昵称
        /// </summary>
        Name
    }

    /// <summary>
    /// 搜索条件
    /// </summary>
    public enum ManagerSorterCriteria
    {

    }
}
