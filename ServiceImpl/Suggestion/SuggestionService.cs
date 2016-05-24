using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Com.Panduo.Common;
using Com.Panduo.Entity.Suggestion;
using Com.Panduo.Service;
using Com.Panduo.Service.Suggestion;
using Com.Panduo.ServiceImpl.Suggestion.Dao;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.ServiceImpl.Suggestion
{
    public class SuggestionService : ISuggestionService
    {
        public ICustomerSuggestionItemDao CustomerSuggestionItemDao { private get; set; }
        public ICustomerSuggestionObjectsDao CustomerSuggestionObjectsDao { private get; set; }
        public ICustomerSuggestionContentDao CustomerSuggestionContentDao { private get; set; }
        public ICustomerSuggestionAttachmentDao CustomerSuggestionAttachmentDao { private get; set; }
        public ICustomerSuggestionDetailDao CustomerSuggestionDetailDao { private get; set; }


        public string ERROR_SUGGESTION_CONTENT_NOT_EXIST
        {
            get { return "ERROR_SUGGESTION_CONTENT_NOT_EXIST"; }
        }

        public IList<SuggestionItem> GetAllSuggestionItems(int languageId)
        {
            return CustomerSuggestionItemDao.GetSuggestionItemPos(languageId).Select(GetSuggestionItemVoFromPo).ToList();
        }

        public IList<SuggestionObject> GetSuggestionObjectsByItemId(int itemId)
        {
            return
                CustomerSuggestionObjectsDao.GetSuggestionObjectsPos(itemId)
                    .Select(GetSuggestionObjectVoFromPo)
                    .ToList();
        }

        public IList<SuggestionObject> GetAllSuggestionObjects()
        {
            return CustomerSuggestionObjectsDao.GetAll().Select(GetSuggestionObjectVoFromPo).ToList();
        }

        public int AddSuggestionContent(SuggestionContent suggestionContent)
        {
            int detailId = CustomerSuggestionContentDao.AddObject(GetSuggestionContentPoFromVo(suggestionContent));
            if (detailId > 0)
            {
                CustomerSuggestionDetailDao.AddObjects(
                    suggestionContent.Details.Select(m => GetSuggestionDetailPoFromVo(m, detailId)));
                CustomerSuggestionAttachmentDao.AddObjects(
                    suggestionContent.AttachmentList.Select(m => GetSuggestionAttachmentPoFromVo(m, detailId)));
            }
            return detailId;
        }

        public void ReplySuggestionContent(int detailId, string replyContent)
        {
            CustomerSuggestionContentDao.UpdateCustomerSuggestionContent(detailId, replyContent, DateTime.Now);
        }

        public PageData<SuggestionContent> FindAllSuggestionContents(int currentPage, int pageSize, IDictionary<SuggestionContentSearchCriteria, object> searchCriteria, IList<Sorter<SuggestionContentSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("SELECT S FROM CustomerSuggestionContentPo S");

            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (var item in searchCriteria)
                {
                    switch (item.Key)
                    {
                        case SuggestionContentSearchCriteria.LanguageId:
                            hqlHelper.AddWhere("S.LanguageId", HqlOperator.Eq, "LanguageId", item.Value);
                            break;
                        case SuggestionContentSearchCriteria.Name:
                            hqlHelper.AddWhere("S.FullName", HqlOperator.Like, "Name", item.Value);
                            break;
                        case SuggestionContentSearchCriteria.Email:
                            hqlHelper.AddWhere("S.Email", HqlOperator.Like, "Email", item.Value);
                            break;
                        case SuggestionContentSearchCriteria.Keyword:
                            hqlHelper.AddWhere(
                                string.Format("(S.Email Like {0} Or S.FullName Like {0})", ":Keyword"),
                                HqlOperator.Exp, "Keyword", string.Format("%{0}%", item.Value));
                            break;
                    }
                }
            }
            if (!sorterCriteria.IsNullOrEmpty())
            {
                foreach (var sorter in sorterCriteria)
                {
                    switch (sorter.Key)
                    {
                        case SuggestionContentSorterCriteria.Name:
                            hqlHelper.AddSorter("S.FullName", sorter.IsAsc);
                            break;
                        case SuggestionContentSorterCriteria.DateCreated:
                            hqlHelper.AddSorter("S.DateCreated", sorter.IsAsc);
                            break;
                    }
                }
            }
            else
            {
                hqlHelper.AddSorter("S.DetailId", true);
            }

            var pageDataPo = CustomerSuggestionContentDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<SuggestionContent>();
            var voList = pageDataPo.Data.Select(GetSuggestionContentVoFromPo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        public void DeleteSuggestionContent(int id)
        {
            var uggestionContent = CustomerSuggestionContentDao.GetObject(id);
            if (uggestionContent.IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_SUGGESTION_CONTENT_NOT_EXIST);
            }
            CustomerSuggestionContentDao.DeleteObjectById(id);
        }

        public SuggestionContent GetSuggestionContent(int id)
        {
            SuggestionContent suggestionContent = GetSuggestionContentVoFromPo(CustomerSuggestionContentDao.GetObject(id));
            suggestionContent.Details = GetSuggestionDetails(id).ToList();
            suggestionContent.AttachmentList = GetSuggestionAttachments(id).ToList();
            return suggestionContent;
        }

        public IList<SuggestionDetail> GetSuggestionDetails(int suggestionContentId)
        {
            return CustomerSuggestionDetailDao.GetCustomerSuggestionDetails(suggestionContentId).Select(GetSuggestionDetailVoFromPo).ToList();
        }

        public IList<SuggestionAttachment> GetSuggestionAttachments(int suggestionContentId)
        {
            return CustomerSuggestionAttachmentDao.GetCustomerSuggestionAttachments(suggestionContentId).Select(GetSuggestionAttachmentVoFromPo).ToList();
        }

        public int SuggestionFeedback(SuggestionContent suggestionContent)
        {
            var suggetionContentId = CustomerSuggestionContentDao.AddObject(GetSuggestionContentPoFromVo(suggestionContent));
            foreach (var suggestionAttachment in suggestionContent.AttachmentList)
            {
                var suggestionAttachmentPo = GetSuggestionAttachmentPoFromVo(suggestionAttachment);
                suggestionAttachmentPo.DetailId = suggetionContentId;
                CustomerSuggestionAttachmentDao.AddObject(suggestionAttachmentPo);
            }
            foreach (var details in suggestionContent.Details)
            {
                var suggestionDetailPo = GetSuggestionDetailPoFromVo(details);
                suggestionDetailPo.DetailId = suggetionContentId;
                CustomerSuggestionDetailDao.AddObject(suggestionDetailPo);
            }
            return suggetionContentId;
        }

        #region 辅助方法

        internal static SuggestionItem GetSuggestionItemVoFromPo(CustomerSuggestionItemPo customerSuggestionItemPo)
        {
            SuggestionItem suggestionItem = null;
            if (!customerSuggestionItemPo.IsNullOrEmpty())
            {
                suggestionItem = new SuggestionItem
                {
                    Id = customerSuggestionItemPo.ItemId,
                    Name = customerSuggestionItemPo.ItemName,
                    LanguageId = customerSuggestionItemPo.LanguageId,
                    SuggestionObjects = ServiceFactory.SuggestionService.GetSuggestionObjectsByItemId(customerSuggestionItemPo.ItemId).ToList()
                };
            }
            return suggestionItem;
        }

        internal static SuggestionObject GetSuggestionObjectVoFromPo(
            CustomerSuggestionObjectsPo customerSuggestionObjectsPo)
        {
            SuggestionObject suggestionObject = null;
            if (!customerSuggestionObjectsPo.IsNullOrEmpty())
            {
                suggestionObject = new SuggestionObject
                {
                    Id = customerSuggestionObjectsPo.ObjectId,
                    ItemId = customerSuggestionObjectsPo.ItemId,
                    Name = customerSuggestionObjectsPo.Title,
                    TotalScore = customerSuggestionObjectsPo.FullNumber,
                    Sort = customerSuggestionObjectsPo.SortOrder
                };
            }
            return suggestionObject;
        }

        internal static SuggestionContent GetSuggestionContentVoFromPo(CustomerSuggestionContentPo customerSuggestionContentPo)
        {
            SuggestionContent suggestionContent = null;
            if (!customerSuggestionContentPo.IsNullOrEmpty())
            {
                suggestionContent = new SuggestionContent
                {
                    Id = customerSuggestionContentPo.DetailId,
                    LanguageId = customerSuggestionContentPo.LanguageId,
                    FullName = customerSuggestionContentPo.FullName,
                    Email = customerSuggestionContentPo.Email,
                    Content = customerSuggestionContentPo.Content,
                    CreateDateTime = customerSuggestionContentPo.DateCreated,
                    ReplyContent = customerSuggestionContentPo.ReplyContent,
                    ReplyDate = customerSuggestionContentPo.ReplyDate
                };
            }
            return suggestionContent;
        }

        internal static CustomerSuggestionContentPo GetSuggestionContentPoFromVo(SuggestionContent suggestionContent)
        {
            CustomerSuggestionContentPo customerSuggestionContentPo = null;
            if (!suggestionContent.IsNullOrEmpty())
            {
                customerSuggestionContentPo = new CustomerSuggestionContentPo
                {
                    DetailId = suggestionContent.Id,
                    LanguageId = suggestionContent.LanguageId,
                    FullName = suggestionContent.FullName,
                    Email = suggestionContent.Email,
                    Content = suggestionContent.Content,
                    DateCreated = suggestionContent.CreateDateTime,
                    ReplyContent = suggestionContent.ReplyContent,
                    ReplyDate = suggestionContent.ReplyDate
                };
            }
            return customerSuggestionContentPo;
        }

        internal static CustomerSuggestionAttachmentPo GetSuggestionAttachmentPoFromVo(SuggestionAttachment suggestionAttachment, int detailId = 0)
        {
            CustomerSuggestionAttachmentPo customerSuggestionAttachmentPo = null;
            if (!suggestionAttachment.IsNullOrEmpty())
            {
                customerSuggestionAttachmentPo = new CustomerSuggestionAttachmentPo
                {
                    DetailId = detailId,
                    AttachmentName = suggestionAttachment.Name,
                    AttachmentPath = suggestionAttachment.Path
                };
            }
            return customerSuggestionAttachmentPo;
        }

        internal static SuggestionAttachment GetSuggestionAttachmentVoFromPo(CustomerSuggestionAttachmentPo suggestionAttachmentPo)
        {
            SuggestionAttachment suggestionAttachment = null;
            if (!suggestionAttachmentPo.IsNullOrEmpty())
            {
                suggestionAttachment = new SuggestionAttachment
                {
                    SuggestionContentId = suggestionAttachmentPo.DetailId,
                    Name = suggestionAttachmentPo.AttachmentName,
                    Path = suggestionAttachmentPo.AttachmentPath
                };
            }
            return suggestionAttachment;
        }

        internal static CustomerSuggestionDetailPo GetSuggestionDetailPoFromVo(SuggestionDetail suggestionDetail, int detailId = 0)
        {
            CustomerSuggestionDetailPo customerSuggestionDetailPo = null;
            if (!suggestionDetail.IsNullOrEmpty())
            {
                customerSuggestionDetailPo = new CustomerSuggestionDetailPo
                {
                    ObjectId = suggestionDetail.ObjectId,
                    DetailId = detailId,
                    Number = suggestionDetail.Score
                };
            }
            return customerSuggestionDetailPo;
        }

        internal static SuggestionDetail GetSuggestionDetailVoFromPo(CustomerSuggestionDetailPo suggestionDetailPo)
        {
            SuggestionDetail suggestionDetail = null;
            if (!suggestionDetailPo.IsNullOrEmpty())
            {
                suggestionDetail = new SuggestionDetail
                {
                    ObjectId = suggestionDetailPo.ObjectId,
                    SuggestionContentId = suggestionDetailPo.DetailId,
                    Score = suggestionDetailPo.Number
                };
            }
            return suggestionDetail;
        }
        #endregion
    }
}