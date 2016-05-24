//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：PropertyValueDao.cs
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
    public class PropertyValueDao:BaseDao<PropertyValuePo,int>, IPropertyValueDao
    {
        public PageData<PropertyValuePo> FindPropertyValuesForAdminList(int propertyId, int currentPage, int pageSize, string keyWrod)
        {
            var hqlHelpder = new HqlHelper("Select p from PropertyValuePo p");
            if (propertyId > 0)
            {
                hqlHelpder.AddWhere("p.PropertyId", HqlOperator.Eq, "PropertyId", propertyId);
            }
            if (!keyWrod.IsNullOrEmpty())
            {
                hqlHelpder.AddWhere(string.Format("p.Code like {0} or p.ChineseName like {0}", ":keyWord"), HqlOperator.Exp, "keyWord", keyWrod);
            }
            return FindPageDataByHql(currentPage, pageSize, hqlHelpder.Hql, hqlHelpder.ParamMap);

        }
    } 
}
   