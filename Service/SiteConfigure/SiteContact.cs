using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.SiteConfigure
{
   /// <summary>
   /// 网站联系方式
   /// </summary>
    [Serializable]
    public class SiteContact
    {
       /// <summary>
        /// Skype
       /// </summary>
       public virtual string Skype { set; get; }


       /// <summary>
       /// 邮箱
       /// </summary>
       public virtual string MailBox { set; get; }


       /// <summary>
       /// 电话
       /// </summary>
       public virtual string Telephone { set; get; }

    }
}
