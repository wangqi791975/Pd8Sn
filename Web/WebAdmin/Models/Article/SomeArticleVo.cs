using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Service.Article;

namespace Com.Panduo.Web.Models.Article
{
    public class SomeArticleVo
    {
        public SomeArticle SomeArticle { get; set; }

        public IList<SomeArticleLanguage> SomeArticleLanguage { get; set; }


    }
}