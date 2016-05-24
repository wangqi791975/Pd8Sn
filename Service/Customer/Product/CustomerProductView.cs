namespace Com.Panduo.Service.Customer.Product
{
    public class CustomerProductView
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 客户Id
        /// </summary>
        public virtual int CustomerId
        {
            get;
            set;
        }
        /// <summary>
        /// 产品id
        /// </summary>
        public virtual int ProductId
        {
            get;
            set;
        }
        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual string ProductModel
        {
            get;
            set;
        }
        /// <summary>
        /// 语言Id
        /// </summary>
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 产品名称
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 图片
        /// </summary>
        public virtual string Image
        {
            get;
            set;
        }
        /// <summary>
        /// 最高单价
        /// </summary>
        public virtual decimal ProfitRate
        {
            get;
            set;
        } 
    }
}