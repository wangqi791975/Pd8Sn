using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Common
{
    /// <summary>
    ///树辅助类
    /// </summary>
    public static class TreeHelper
    {
        /// <summary>
        /// 转化为一棵树
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">要转化的数据</param>
        /// <param name="idName">主键属性名称</param>
        /// <param name="parentIdName">父属性名称</param>
        /// <param name="sortName">排序属性名称</param>
        /// <returns></returns>
        public static IList<TreeNode<T>> ToTree<T>(this IEnumerable<T> data, string idName = "Id", string parentIdName = "ParentId", string sortName = "Id")
        { 
            var treeProperties = new List<TreeNode<T>>();

            var type = typeof (T);
            var idType = type.GetProperty(idName);
            var parentIdType = type.GetProperty(parentIdName);
            var sortType = type.GetProperty(sortName);

            foreach (var item in data)
            {
                treeProperties.Add(new TreeNode<T>
                                       {
                                           Id = idType.GetValue(item, null),
                                           ParentId = parentIdType.GetValue(item, null),
                                           SortValue = sortType.GetValue(item, null),
                                           Data = item
                                       });
            }
            
            var rootTreeProperties = treeProperties.Where(c => c.ParentId == null);
            foreach (var item in rootTreeProperties)
            {
                item.SetSubTreeNode(treeProperties);
            }

            return rootTreeProperties.OrderBy(c => c.SortValue).ToList();
        }

        /// <summary>
        /// 获取一个数节点及其子孙节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <param name="idName"></param>
        /// <param name="parentIdName"></param>
        /// <param name="sortName"></param>
        /// <returns></returns>
        public static TreeNode<T> GetSubTree<T>(this IEnumerable<T> data,object id ,string idName = "Id", string parentIdName = "ParentId", string sortName = "Id")
        {
            var treeProperties = data.ToTree(idName, parentIdName, sortName);

            var treeNode = treeProperties.FirstOrDefault(c => c.Id.Equals(id));

            if (treeNode == null)
            {
                foreach (var treeProperty in treeProperties)
                {
                    treeNode = GetTreeNode(treeProperty.SubDataList, id);
                    if (treeNode!=null)
                    {
                        break;
                    }
                }
            }

            return treeNode;
        }

        private static TreeNode<T> GetTreeNode<T>(this IEnumerable<TreeNode<T>> data, object id)
        {
            var treeNode = data.FirstOrDefault(c => c.Id.Equals(id));
            if (treeNode == null)
            {
                foreach (var node in data)
                {
                    treeNode = node.SubDataList.GetTreeNode(id);
                    if (treeNode != null)
                    {
                        break;
                    }
                }
            }

            return treeNode;
        }

        /// <summary>
        /// 循环设置子树
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="treeNode">树节点</param>
        /// <param name="allTreeProperties">所有的树节点</param>
        private static void SetSubTreeNode<T>(this TreeNode<T> treeNode, IEnumerable<TreeNode<T>> allTreeProperties)
        {
            treeNode.SubDataList = allTreeProperties.Where(c =>c.ParentId !=null && c.ParentId.Equals(treeNode.Id)).OrderBy(c=>c.SortValue).ToList();
            foreach (var item in treeNode.SubDataList)
            {
                item.SetSubTreeNode(allTreeProperties);
            }
        }
    }

    /// <summary>
    /// 树节点
    /// </summary>
    /// <typeparam name="T">树类型</typeparam>
    public class TreeNode<T>
    {
        /// <summary>
        /// 数据主键Id
        /// </summary>
        public object Id { get; set; }
        /// <summary>
        /// 数据父Id
        /// </summary>
        public object ParentId { get; set; }
        /// <summary>
        /// 排序值
        /// </summary>
        public object SortValue { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 子树节点
        /// </summary>
        public IList<TreeNode<T>> SubDataList { get; set; }
    }
}
