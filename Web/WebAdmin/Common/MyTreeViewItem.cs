using System.Collections.Generic;
using System.Linq;
using Com.Panduo.Common;
using Com.Panduo.Service;
using Com.Panduo.Service.Product.Category;

namespace Com.Panduo.Web.Common
{
    public class MyTreeViewItem<T>
    {
        public T Node { get; set; }
        public bool Checked { get; set; }
        public string Index { get; set; }
        public string HtmlDomName { get; set; }
        public MyTreeViewItem<T> Parent { get; set; }
        public IList<MyTreeViewItem<T>> Items { get; set; }
    }

    public class CategoryTreeViewItem
    {
        public static MyTreeViewItem<Category> GetMyTreeViewItem(Category category)
        {
            var parentCategory = ServiceFactory.CategoryService.GetParentCategoryById(category.CategoryId);
            var childCategorys = ServiceFactory.CategoryService.GetAllSubCategories(category.CategoryId);
            var node = new MyTreeViewItem<Category>
            {
                Node = category,
                Checked = false,
                Index = "",
                HtmlDomName = "",
                Parent = parentCategory.IsNullOrEmpty() ? null : GetMyTreeViewItem(parentCategory),
                Items = childCategorys.IsNullOrEmpty() ? null : childCategorys.Select(GetMyTreeViewItem).ToList()
            };
            return node;
        }
    }
}