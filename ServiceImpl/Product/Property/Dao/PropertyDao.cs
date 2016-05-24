//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：PropertyDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 14:40:40 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using Com.Panduo.Common;
using Com.Panduo.Entity.Product.Property;
using Com.Panduo.Service;

namespace Com.Panduo.ServiceImpl.Product.Property.Dao
{ 
    public class PropertyDao:BaseDao<PropertyPo,int>, IPropertyDao
    {
        public PageData<PropertyPo> FindPropertiesForAdminList(int currentPage, int pageSize, string keyWord)
        {
            var hqlHelper = new HqlHelper("Select p from PropertyPo p");
            if (!keyWord.IsNullOrEmpty())
            {
                hqlHelper.AddWhere(string.Format("(p.Code Like {0}  or p.ChineseName Like {0})", ":keyWord"), HqlOperator.Exp, "keyWord", string.Format("%{0}%", keyWord));
            }

            return FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
        }
 
    } 
}
   