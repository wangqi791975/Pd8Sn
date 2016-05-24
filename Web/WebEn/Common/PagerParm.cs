using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Com.Panduo.Common;

namespace Com.Panduo.Web.Common
{
    [Serializable]
    public class PagerParm
    {
        private string _pageFromatUrl;
        private const string _pageFormat = "-page-";//{page} 在问号后面会有编码问题
        //private const string _pageFormatEncode = "%7Bpage%7D";
        public string PageIndexParmKey = UrlParameterKey.Page;
        public UrlHelper Url;

        public string PageFromatUrl
        {
            get { return _pageFromatUrl; }
        }

        public string PageFormat
        {
            get { return _pageFormat; }
        }

        public int RecordCount;
        public int PageSize;
        public int PageIndex { private set; get; }
        public int PrevPageIndex { private set; get; }
        public int NextPageIndex { private set; get; }

        public int PageButtonCount = 7;
        public int StartPageIndex { private set; get; }
        public int EndPageIndex { private set; get; }

        /// <summary>
        /// 锚点
        /// </summary>
        public string Anchor;

        public string CssClass = "BotPage";

        /// <summary>
        /// 总是显示
        /// </summary>
        public bool AlwayDisplay = true;

        /// <summary>
        /// 显示GO按钮
        /// </summary>
        public bool ShowGoButton = false;

        public int PageCount
        {
            private set;
            get;
        }

        public bool ShowInStoreFilter;

        public PagerParm(UrlHelper url, int recordCount, int pageSize)
            : this(url, recordCount, pageSize, false)
        {

        }

        public PagerParm(UrlHelper url, int recordCount, int pageSize, bool showInStoreFiler)
        {
            this.Url = url;
            this.PageSize = pageSize;
            this.RecordCount = recordCount;
            this.ShowInStoreFilter = showInStoreFiler;
            Refresh();
        }

        public void Refresh()
        {
            if (PageSize < 1) throw new ArgumentOutOfRangeException("PageSize");
            if (this.RecordCount < 0) this.RecordCount = 0;
            int pageCount = RecordCount / PageSize;
            if (RecordCount % PageSize > 0) pageCount += 1;
            this.PageCount = pageCount;

            HttpRequest request = HttpContext.Current.Request;
            if (Url.RequestContext.RouteData.Values.ContainsKey(PageIndexParmKey))
            {
                var routeDate = new RouteValueDictionary(Url.RequestContext.RouteData.Values);
                var pageIndex = routeDate.TryGetValue(PageIndexParmKey);
                this.PageIndex = pageIndex.IsNullOrEmpty() ? 1 : pageIndex.ParseTo(1);
                //routeDate.Set(PageIndexParmKey, _pageFormat);
                routeDate[PageIndexParmKey] = _pageFormat;
                _pageFromatUrl = Url.Action(routeDate["action"].ToString(), routeDate);
                var baseUrl = UrlPathHelper.GetHost(true);
                _pageFromatUrl = string.Format("{0}{1}", baseUrl, _pageFromatUrl);
                _pageFromatUrl = _pageFromatUrl.SetParam(request.Url.AbsoluteUri.GetQueryStringMap());
                //_pageFromatUrl = Regex.Replace(_pageFromatUrl, _pageFormatEncode, _pageFormat, RegexOptions.IgnoreCase);
            }
            else
            {
                //this.PageIndex = request.QueryString.TryGetInt(PageIndexParmKey, 1);
                this.PageIndex = request.QueryString[PageIndexParmKey].ParseTo(1);
                _pageFromatUrl = request.Url.AbsoluteUri.SetParam(PageIndexParmKey, _pageFormat);
                //_pageFromatUrl = Regex.Replace(_pageFromatUrl, _pageFormatEncode, _pageFormat, RegexOptions.IgnoreCase);
            }

            if (PageIndex > pageCount) PageIndex = pageCount;
            if (PageIndex < 1) PageIndex = 1;
            PrevPageIndex = PageIndex < 2 ? 1 : (PageIndex - 1);
            NextPageIndex = PageIndex >= pageCount ? pageCount : (PageIndex + 1);

            if (pageCount <= PageButtonCount)
            {
                StartPageIndex = 1;
                EndPageIndex = pageCount;
            }
            else
            {
                int middleButtonIndex = (PageButtonCount + 1) / 2;
                if (PageIndex > pageCount - middleButtonIndex)
                {
                    EndPageIndex = pageCount;
                    StartPageIndex = EndPageIndex - PageButtonCount + 1;
                }
                else if (PageIndex < middleButtonIndex + 1)
                {
                    StartPageIndex = 1;
                    EndPageIndex = PageButtonCount;
                }
                else
                {
                    EndPageIndex = PageIndex + middleButtonIndex - 1;
                    StartPageIndex = EndPageIndex - PageButtonCount + 1;
                    if (StartPageIndex < 1)
                    {
                        StartPageIndex = 1;
                    }
                }
            }
        }

        /// <summary>
        /// 获取分页URL
        /// </summary>
        public string GetPageUrl(int pageIndex)
        {
            return _pageFromatUrl.Replace(_pageFormat, pageIndex.ToString());
        }

        /// <summary>
        /// 获取分页URL
        /// </summary>
        public string GetPageUrl()
        {
            return _pageFromatUrl.Replace(_pageFormat, _pageFormat);
        }

        public static int GetPageSize()
        {
            int pageSize;
            if (!int.TryParse(HttpContext.Current.Request.QueryString[UrlParameterKey.PageSize], out pageSize) || pageSize < 1)
            {
                return CookieHelper.ProductPageSize;
            }
            return pageSize;
        }

    }
}
