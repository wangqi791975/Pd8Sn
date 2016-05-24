using System;
using System.Collections.Generic;
using System.Web;

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// 面包屑导航
    /// </summary>
    [Serializable]
    public class SitemapItem
    {
        /// <summary>
        /// 面包屑导航的Text
        /// </summary>
        public string Text { get; set; }

        public string DisplayText
        {
            get
            {
                if (IsHtmlEncode) return HttpUtility.HtmlEncode(Text);
                return Text;
            }
        }

        /// <summary>
        /// 面包屑导航的Link
        /// </summary>
        public string Link { get; set; }

        public string Title { set; get; }

        /// <summary>
        /// 面包屑导航的Css
        /// </summary>
        public string CssClass { get; set; }

        private bool _isHtmlEncode = true;
        /// <summary>
        /// 是否使用HTML编码
        /// </summary>
        public bool IsHtmlEncode
        {
            set { _isHtmlEncode = value; }
            get { return _isHtmlEncode; }
        }

        public SitemapItem()
        {

        }

        public SitemapItem(string text)
        {
            this.Text = text;
        }

        public SitemapItem(string text, string link)
        {
            this.Text = text;
            this.Link = link;
        }

        public SitemapItem(string text, string link, bool isHtmlEncode)
        {
            this.Text = text;
            this.Link = link;
            this.IsHtmlEncode = isHtmlEncode;
        }

        public SitemapItem(string text, string link, string cssClass)
        {
            this.Text = text;
            this.Link = link;
            this.CssClass = cssClass;
        }

        public SitemapItem(string text, string link, string cssClass, bool isHtmlEncode)
        {
            this.Text = text;
            this.Link = link;
            this.CssClass = cssClass;
            this.IsHtmlEncode = isHtmlEncode;
        }
    }
}
