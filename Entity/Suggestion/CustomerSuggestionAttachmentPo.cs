
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Suggestion
{
    /// <summary>
    ///描述：客户评分附件表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:49
    /// </summary>
    [Class(Table = "t_customer_suggestion_attachment", Lazy = false, NameType = typeof(CustomerSuggestionAttachmentPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CustomerSuggestionAttachmentPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>

        [Id(1, Name = "Id", Column = "id")]
        [Generator(2, Class = "native")]

        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 客户评分内容ID
        /// </summary>
        [Property(Column = "detail_id")]
        public virtual int DetailId
        {
            get;
            set;
        }
        /// <summary>
        /// 附件名称
        /// </summary>
        [Property(Column = "attachment_name")]
        public virtual string AttachmentName
        {
            get;
            set;
        }
        /// <summary>
        /// 附件路径
        /// </summary>
        [Property(Column = "attachment_path")]
        public virtual string AttachmentPath
        {
            get;
            set;
        }
    }
}

