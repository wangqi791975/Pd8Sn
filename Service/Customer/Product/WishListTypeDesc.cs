//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2014-2020
//系统名称：8Seasons
//文件名称：WishListTypeDesc.cs
//创 建 人：罗海明
//创建时间：2015/02/01 11:59:50 
//功能说明：心愿单喜爱类型Vo
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//----------------------------------------------------------------- 
using System;

namespace Com.Panduo.Service.Customer.Product
{
    /// <summary>
    /// 心愿单喜爱类型
    /// </summary>
    [Serializable]
    public class WishListTypeDesc
    {
        /// <summary>
        /// 心愿单喜爱类型Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 语种Id
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// 选项名称
        /// </summary>
        public virtual string ItemName { get; set; }

    }
}
