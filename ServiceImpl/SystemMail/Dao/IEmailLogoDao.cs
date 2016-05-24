//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IEmailLogoDao.cs
//创 建 人：罗海明
//创建时间：2015/01/29 17:59:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections.Generic;
using Com.Panduo.Entity.SystemMail;
using Com.Panduo.Service;
using Com.Panduo.Service.SystemMail;

namespace Com.Panduo.ServiceImpl.SystemMail.Dao
{ 
    public interface IEmailLogoDao:IBaseDao<EmailLogoPo,int>
    {
        PageData<EmailLogoPo> GetMailLogoList(int currentPage, int pageSize, int languageId,
            IList<Sorter<MailLogoSorterCriteria>> sorterCriteria);

        EmailLogoPo GetMailLogoByLanguageId(int languageId);

        void UseMailLogo(int logoId);
    } 
}
   