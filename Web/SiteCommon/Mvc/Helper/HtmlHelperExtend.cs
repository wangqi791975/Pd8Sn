using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Web.Common;
using Com.Panduo.Common;

namespace System.Web.Mvc
{
    /// <summary>
    /// HtmlHelper扩展累
    /// </summary>
    public static class HtmlHelperExtend
    {
        /// <summary>
        /// 验证码
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="url">验证码地址</param>
        /// <param name="htmlAttributes">html属性</param>
        /// <returns></returns>
        public static MvcHtmlString ValidateCode(this HtmlHelper htmlHelper,string url,object htmlAttributes=null)
        {
            return htmlHelper.Image(url, htmlAttributes);
        }

        /// <summary>
        /// 图片
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="url">验证码地址</param>
        /// <param name="htmlAttributes">html属性</param>
        /// <returns></returns>
        public static MvcHtmlString Image(this HtmlHelper htmlHelper, string url, object htmlAttributes = null)
        {
            IDictionary<string, object> htmlAttributeMap = new Dictionary<string, object>
                {
                    {"src",url}
                };


            if (htmlAttributes != null)
            {
                var properties = htmlAttributes.GetType().GetProperties();
                foreach (var propertyInfo in properties)
                {
                    htmlAttributeMap.Add(propertyInfo.Name, (string)propertyInfo.GetValue(htmlAttributes, null)); 
                }
            }

            return htmlHelper.HtmlTag("img", htmlAttributeMap);
        }

        /// <summary>
        /// Html标签
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="tag">标签，eg:input,img</param>
        /// <param name="htmlAttributes">标签属性,eg:title,alt</param>
        /// <returns></returns>
        public static MvcHtmlString HtmlTag(this HtmlHelper htmlHelper, string tag, object htmlAttributes = null)
        {
            IDictionary<string, object> htmlAttributeMap = null;

            if (htmlAttributes != null)
            {
                htmlAttributeMap = new Dictionary<string, object>();
                var properties = htmlAttributes.GetType().GetProperties();
                foreach (var propertyInfo in properties)
                {
                    htmlAttributeMap.Add(propertyInfo.Name, (string)propertyInfo.GetValue(htmlAttributes, null)); 
                }
            }

            return htmlHelper.HtmlTag(tag, htmlAttributeMap);
        }

        /// <summary>
        /// Html标签
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="tag">标签，eg:input,img</param>
        /// <param name="htmlAttributes">标签属性,eg:title,alt</param>
        /// <returns></returns>
        public static MvcHtmlString HtmlTag(this HtmlHelper htmlHelper, string tag, IDictionary<string,object> htmlAttributes = null)
        {
            var tagBuilder = new TagBuilder(tag); 

            if (htmlAttributes != null)
            {
                foreach (var htmlAttribute in htmlAttributes)
                {
                    if (htmlAttribute.Key.Equals("@class", StringComparison.InvariantCultureIgnoreCase))
                    {
                        tagBuilder.AddCssClass((string)htmlAttribute.Value);
                    }
                    else
                    {
                        tagBuilder.MergeAttribute(htmlAttribute.Key, (string)htmlAttribute.Value, true);
                    }
                }
            }

            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        /// <summary>
        /// 复选框列表
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name">单选框的名称</param>
        /// <param name="selectList">选项列表</param>
        /// <param name="disabledValueList">需要禁用的值列表</param>
        /// <param name="htmlAttributes">Html标签属性,eg:title,alt</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, IEnumerable<string> disabledValueList = null, object htmlAttributes = null)
        {
            if (selectList != null && selectList.Any())
            {
                var htmlAttributeMap = new Dictionary<string, string>();
                if (htmlAttributes != null)
                {
                    var properties = htmlAttributes.GetType().GetProperties();
                    foreach (var propertyInfo in properties)
                    {
                        if (htmlAttributeMap.ContainsKey(propertyInfo.Name))
                        {
                            htmlAttributeMap[propertyInfo.Name] = (string)propertyInfo.GetValue(htmlAttributes, null);
                        }
                        else
                        {
                            htmlAttributeMap.Add(propertyInfo.Name, (string)propertyInfo.GetValue(htmlAttributes, null));
                        }
                    }
                }


                IList<TagBuilder> list = new List<TagBuilder>();

                foreach (var item in selectList)
                {
                    var id = name + item.Value;
                    var tag = new TagBuilder("input");
                    tag.GenerateId(id);
                    tag.MergeAttribute("name", name);
                    tag.MergeAttribute("type", "checkbox");
                    tag.MergeAttribute("value", item.Value);

                    //选中
                    if (item.Selected)
                    {
                        tag.MergeAttribute("checked", "checked");
                    }

                    //禁用
                    if (disabledValueList != null && disabledValueList.Any(c => c.Equals(item.Value)))
                    {
                        tag.MergeAttribute("disabled", "disabled");
                    }

                    //html属性
                    foreach (var attributeItem in htmlAttributeMap)
                    {
                        if (attributeItem.Key.Equals("@class", StringComparison.InvariantCultureIgnoreCase))
                        {
                            tag.AddCssClass(attributeItem.Value);
                        }
                        else
                        {
                            tag.MergeAttribute(attributeItem.Key, attributeItem.Value, true);
                        }
                    }

                    var labelTag = new TagBuilder("label");
                    labelTag.MergeAttribute("id", "lbl" + id);
                    labelTag.MergeAttribute("name", "lbl" + id);
                    labelTag.MergeAttribute("for", id);
                    labelTag.SetInnerText(item.Text);

                    list.Add(tag);
                    list.Add(labelTag);
                }

                return MvcHtmlString.Create(list.Join("&nbsp;"));
            }

            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// 单选框列表
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name">单选框的名称</param>
        /// <param name="selectList">选项列表</param>
        /// <param name="disabledValueList">需要禁用的值列表</param>
        /// <param name="htmlAttributes">Html标签属性,eg:title,alt</param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, IEnumerable<string> disabledValueList = null, object htmlAttributes = null)
        {
            if (selectList != null && selectList.Any())
            {
                var htmlAttributeMap = new Dictionary<string, string>();
                if (htmlAttributes != null)
                {
                    var properties = htmlAttributes.GetType().GetProperties();
                    foreach (var propertyInfo in properties)
                    {
                        if (htmlAttributeMap.ContainsKey(propertyInfo.Name))
                        {
                            htmlAttributeMap[propertyInfo.Name] = (string)propertyInfo.GetValue(htmlAttributes, null);
                        }
                        else
                        {
                            htmlAttributeMap.Add(propertyInfo.Name, (string)propertyInfo.GetValue(htmlAttributes, null));
                        }
                    }
                }


                IList<TagBuilder> list = new List<TagBuilder>();

                foreach (var item in selectList)
                {
                    var id = name + item.Value;
                    var tag = new TagBuilder("input");
                    tag.GenerateId(id);
                    tag.MergeAttribute("name", name);
                    tag.MergeAttribute("type", "radio");
                    tag.MergeAttribute("value", item.Value);

                    //选中
                    if (item.Selected)
                    {
                        tag.MergeAttribute("checked", "checked");
                    }

                    //禁用
                    if (disabledValueList != null && disabledValueList.Any(c => c.Equals(item.Value)))
                    {
                        tag.MergeAttribute("disabled", "disabled");
                    }

                    //html属性
                    foreach (var attributeItem in htmlAttributeMap)
                    {
                        if (attributeItem.Key.Equals("@class", StringComparison.InvariantCultureIgnoreCase))
                        {
                            tag.AddCssClass(attributeItem.Value);
                        }
                        else
                        {
                            tag.MergeAttribute(attributeItem.Key, attributeItem.Value, true);
                        }
                    }

                    var labelTag = new TagBuilder("label");
                    labelTag.MergeAttribute("id", "lbl" + id);
                    labelTag.MergeAttribute("name", "lbl" + id);
                    labelTag.MergeAttribute("for", id);
                    labelTag.SetInnerText(item.Text);

                    list.Add(tag);
                    list.Add(labelTag);
                }

                return MvcHtmlString.Create(list.Join("&nbsp;"));
            }

            return MvcHtmlString.Empty;
        }

        #region Suggest
        /// <summary>
        /// 普通Suggest
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="source">数据源</param>
        /// <param name="choosedValue">选择项的值</param>
        /// <param name="defaultValue">默认显示值</param>
        /// <param name="isDisabled">控件是否禁用</param>
        /// <param name="maxDisplayCount">每次最多显示下拉选项个数</param>
        /// <param name="htmlAttributes">Html属性，比如class、style等</param>
        /// <returns></returns>
        public static MvcHtmlString Suggest(string name, IEnumerable<SelectListItem> source, string choosedValue = "", string defaultValue = "", bool isDisabled = false, int maxDisplayCount = 10, object htmlAttributes = null)
        {
            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// Ajax Suggest
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="ajaxUrl">Ajax请求的Url地址</param>
        /// <param name="extraJsonData">提交的额外Json个数数据</param>
        /// <param name="choosedValue">选择项的值</param>
        /// <param name="defaultValue">默认显示值</param>
        /// <param name="isDisabled">控件是否禁用</param>
        /// <param name="maxDisplayCount">每次最多显示下拉选项个数</param>
        /// <param name="htmlAttributes">Html属性，比如class、style等</param>
        /// <returns></returns>
        public static MvcHtmlString Suggest(string name, string ajaxUrl, string extraJsonData = "", string choosedValue = "", string defaultValue = "", bool isDisabled = false, int maxDisplayCount = 10, object htmlAttributes = null)
        {
            return MvcHtmlString.Empty;
        }
        #endregion
    }
}