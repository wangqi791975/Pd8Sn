//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ProductVo.cs
//创 建 人：万天文
//创建时间：2015/01/15 16:05:07 
//功能说明：产品展示VO类
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//----------------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Service;

namespace Com.Panduo.Web.Models.Product
{
    [Serializable]
    public class CategoryTreeVo
    {
        /// <summary>
        /// 类别树使用场景
        /// </summary>
        public enum CategoryTreeType
        {
            /// <summary>
            /// 首页
            /// </summary>
            Home = 0,
            /// <summary>
            /// 产品列表
            /// </summary>
            ProductList = 1,
            /// <summary>
            /// 产品搜索
            /// </summary>
            ProductSearch = 2,
            /// <summary>
            /// 产品详情
            /// </summary>
            ProductDatail = 3
        }

        /// <summary>
        /// 类别树使用场景
        /// </summary>
        public CategoryTreeType TreeType { get; set; }
        /// <summary>
        /// 类别树
        /// </summary>
        public IList<RelatedData<Service.Product.Category.CategoryLanguage>> CategoryRelatedDatas { get; set; }

        /// <summary>
        /// 当前类别ID
        /// </summary>
        public int CurrentCategoryId { get; set; }

        /// <summary>
        /// 当前类另父ID
        /// </summary>
        public int CurrentParentCategoryId { get; set; }
    }
}