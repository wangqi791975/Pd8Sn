namespace Com.Panduo.Service.Customer.Club
{
    public class ClubBlackList
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 客户邮箱
        /// </summary>
        public virtual string CustomerEmail
        {
            get;
            set;
        }
    }
}