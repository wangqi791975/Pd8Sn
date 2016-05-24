namespace Com.Panduo.Web.Common
{
    public static class CommonConst
    {
        public static readonly string CheckAccountSettingKey = "8SeasonsAdmin";

        #region 公用
        public static readonly string ChineseLanguage = "zh-cn";
        public static readonly string EnglishLanguage = "en-us";
        public static readonly string HongKLanguage = "zh-tw";
        public static readonly string Asc = "ASC";
        public static readonly string Desc = "DESC";
        public static readonly string Hidden = "Hidden"; 
        public static readonly string PageIndex = "page";
        public static readonly string PageSize = "size"; 
        public static readonly int DefaultPageSize = 20; 
        public static readonly string Comma = ",";
        public static readonly string BackSlash = "/";
        public static readonly string Slash = @"\";
        public static readonly string DoubleSlash = @"\\";
        public static readonly string HtmlSpace = "&nbsp;";
        public static readonly string EmptyString = "";
        public static readonly char EmptyChar = ' ';
        public static readonly string NewLineCode = "\n";
        public static readonly string NewLineCode2 = "\r\n";
        public static readonly string HtmlNewLineCode = "<br/>";
        public static readonly string One = "1";
        public static readonly string Two = "2";
        public static readonly string TwoThousand = "2000";
        public static readonly string Yes = "Yes";
        public static readonly string GreaterThan = ">";
        public static readonly string GreaterThanCode = "&gt;";
        public static readonly string LessThan = "<";
        public static readonly string LessThanCode = "&lt;";
        public static readonly string[] WordSplitArray = { " " };//",", "!", ".", ":" 
        public static readonly string Colon = "：";
        public static readonly string PleaseChoose = "";//--请选择--
        public static readonly string EnglishAndCode = "&";
        public static readonly string EqualCode = "="; 
        public static readonly string Id = "Id";
        public static readonly string Post = "Post";
        public static readonly string Get = "Get";
        public static readonly string RedirectUrl = "redirectUrl";
        #endregion 

        #region 页面需求
        public static readonly string AddPrefix = "Add_";
        public static readonly string UpdatePrefix = "Update_";
        public static readonly string DeletePrefix = "Delete_";

        public static readonly string ChinesePrefix = "Chinese_";
        public static readonly string EnglistPrefix = "English_";

        public static readonly string DefualtSiteTheme = "Default";
        #endregion

        #region 正则表达式

        public static readonly string ChineseRegex = @"[\u4e00-\u9fff]";//中文字符

        #endregion  

        /// <summary>
        /// 图片缓存路径
        /// </summary> 

        public static readonly string CooliesDomain = "8seasons.com"; 
    }
}

