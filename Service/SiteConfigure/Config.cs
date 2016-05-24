namespace Com.Panduo.Service.SiteConfigure
{
    public class Config
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
        /// key
        /// </summary>
        public virtual string Key
        {
            get;
            set;
        }
        /// <summary>
        /// value
        /// </summary>
        public virtual string Value
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Cmmt
        {
            get;
            set;
        }
    }
}