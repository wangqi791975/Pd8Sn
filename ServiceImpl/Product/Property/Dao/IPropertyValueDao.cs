//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IPropertyValueDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 13:49:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using Com.Panduo.Entity.Product.Property;
using Com.Panduo.Service;

namespace Com.Panduo.ServiceImpl.Product.Property.Dao
{ 
    public interface IPropertyValueDao:IBaseDao<PropertyValuePo,int>
    {
        /// <summary>
        /// 直接查询数据库得到属性值数据
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="keyWrod">关键词</param>
        /// <returns></returns>
        PageData<PropertyValuePo> FindPropertyValuesForAdminList(int propertyId, int currentPage, int pageSize, string keyWrod);
    } 
}
   