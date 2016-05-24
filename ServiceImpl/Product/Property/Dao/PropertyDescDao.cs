//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：PropertyDescDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 14:40:40 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using Com.Panduo.Entity.Product.Property;

namespace Com.Panduo.ServiceImpl.Product.Property.Dao
{
    public class PropertyDescDao : BaseDao<PropertyDescPo, int>, IPropertyDescDao
    {
        /// <summary>
        /// 通过属性ID和语言ID得到属性语种信息
        /// </summary>
        /// <returns>PropertyDescPo</returns>
        public PropertyDescPo GetPropertyDescByPropertyIdAndLanguageId(int propertyId, int languageId)
        {
            object propertyDescDao = GetSingleObject("from PropertyDescPo where PropertyId = ? and LanguageId = ?", new string[] { propertyId.ToString(), languageId.ToString() });

            return (PropertyDescPo)propertyDescDao;
        }

    }
}
