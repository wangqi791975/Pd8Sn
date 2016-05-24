//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IPropertyValueGroupDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 13:49:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections.Generic;
using Com.Panduo.Entity.Product.Property;

namespace Com.Panduo.ServiceImpl.Product.Property.Dao
{ 
    public interface IPropertyValueGroupDao:IBaseDao<PropertyValueGroupPo,int>
    {
        /// <summary>
        /// 根据属性ID得到属性值组
        /// </summary>
        /// <param name="propertyId">属性ID</param>
        /// <returns>属性值组PO列表</returns>
        IList<PropertyValueGroupPo> GetPropertyValueGroupByProertytyId(int propertyId);
    } 
}
   