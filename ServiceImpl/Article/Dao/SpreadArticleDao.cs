//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：SpreadArticleDao.cs
//创 建 人：罗海明
//创建时间：2015/01/29 18:08:40 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq; 
using Com.Panduo.Common;
using Com.Panduo.Entity.Article;
using Com.Panduo.Service;
using Com.Panduo.Service.Article;

namespace Com.Panduo.ServiceImpl.Article.Dao
{ 
    public class SpreadArticleDao:BaseDao<SpreadArticlePo,int>, ISpreadArticleDao
    {

        public PageData<SomeArticle> FindSomeArticle(int currentPage, int pageSize,
    IDictionary<SomeArticleCriteria, object> searchCriteria, IList<Sorter<SomeArticleSorterCriteria>> sorterCriteria)
        {
            var pageData = new PageData<SomeArticle>();
            var dataList = new List<SomeArticle>();
            var rowCount = 0;

            //设置查询提交
            var parmsList = new List<SqlParameter>
            {
                new SqlParameter("@pageIndex", SqlDbType.Int) {Value = currentPage},
                new SqlParameter("@pageSize", SqlDbType.Int) {Value = pageSize},
                new SqlParameter("@keyword", SqlDbType.VarChar, 20)
                {
                    Value = searchCriteria.TryGet(SomeArticleCriteria.Title).ToSqlString()
                },
                new SqlParameter("@sortField", SqlDbType.VarChar, 100) {Value = string.Empty},
                new SqlParameter("@sortDirecton", SqlDbType.VarChar, 10) {Value = "ASC"}
            };

            //设置排序条件(暂时不需要提供)
            if (sorterCriteria != null)
            {
                foreach (var criteria in sorterCriteria)
                {
                    switch (criteria.Key)
                    {
                        case SomeArticleSorterCriteria.CreateTime:
                            parmsList.Where(c => c.ParameterName == "@sortField").First().Value = "AddDate";
                            parmsList.Where(c => c.ParameterName == "@sortDirecton").First().Value = criteria.IsAsc
                                ? "ASC"
                                : "DESC";
                            break;
                    }
                }
            }

            SomeArticle someArticle;
            using (
                var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure,
                    "up_admin_article_search", parmsList.ToArray()))
            {
                //数据条数
                if (reader.Read())
                {
                    rowCount = !reader.IsDBNull(0) ? reader.GetInt32(0) : 0;
                }
                reader.NextResult();

                //分页数据 
                while (reader.Read())
                {
                    someArticle = new SomeArticle
                    {
                        ArticleId = reader["article_id"].ParseTo<int>(),
                        ChineseTitle = reader["chinese_title"].ParseTo<string>(),
                        EnglishTitle = reader["english_title"].ParseTo<string>(),
                        CreateTime = reader["date_created"].ParseTo<DateTime>(),
                        CreateId = reader["admin_id"].ParseTo<int>(),
                        Creater = reader["account_email"].ParseTo<string>(),
                    };
                    dataList.Add(someArticle);
                }
            }

            pageData.Data = dataList;
            pageData.Pager = new Pager(rowCount, currentPage, pageSize);
            return pageData;
        }
    } 
}
   