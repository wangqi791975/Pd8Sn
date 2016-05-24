//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：EmailLogoDao.cs
//创 建 人：罗海明
//创建时间：2015/01/29 18:08:40 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Com.Panduo.Common;
using Com.Panduo.Entity.SystemMail;
using Com.Panduo.Service;
using Com.Panduo.Service.SystemMail;

namespace Com.Panduo.ServiceImpl.SystemMail.Dao
{ 
    public class EmailLogoDao:BaseDao<EmailLogoPo,int>, IEmailLogoDao
    {
        public PageData<EmailLogoPo> GetMailLogoList(int currentPage, int pageSize, int languageId, IList<Sorter<MailLogoSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("Select E from EmailLogoPo E ");
            hqlHelper.AddWhere("E.LanguageId", HqlOperator.Eq, "LanguageId", languageId);
            if (!sorterCriteria.IsNullOrEmpty())
            {
                foreach (var item in sorterCriteria)
                {
                    switch (item.Key)
                    {
                        case MailLogoSorterCriteria.CreateTime:
                            hqlHelper.AddSorter("E.DateCreated", item.IsAsc);
                            break;
                    }
                }
            }
            return FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
        }


        public EmailLogoPo GetMailLogoByLanguageId(int languageId)
        {
            return GetOneObject("from EmailLogoPo where LanguageId = ? And Status=1", languageId);
        }

        public void UseMailLogo(int logoId)
        {
            var parms = new[]
            {
                new SqlParameter("@id", SqlDbType.Int) {Value = logoId},
            };
            SqlHelper.ExecuteNonQuery(SqlHelper.CONN_STRING, CommandType.StoredProcedure,
                "up_email_logo_use", parms);
        }
    } 
}
   