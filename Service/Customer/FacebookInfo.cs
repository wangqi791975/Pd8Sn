using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Customer
{
    /// <summary>
    /// FaceBook信息
    /// </summary>
    [Serializable]
    public class FacebookInfo
    {
        /// <summary>
        /// 客户Id
        /// </summary>
        public virtual int CustomerId { get; set; }

        /// <summary>
        /// 创建时间（关联时间）
        /// </summary>
        public virtual DateTime CreateDateTime { get; set; }

        /// <summary>
        /// FaceBook账号
        /// </summary>
        public virtual string FaceBookAccount { get; set; }

        /// <summary>
        /// FaceBookId
        /// </summary>
        public virtual string FaceBookId { get; set; }

        /// <summary>
        /// FaceBookEmail
        /// </summary>
        public virtual string FaceBookEmail { get; set; }

        /// <summary>
        /// FaceBookFName
        /// </summary>
        public virtual string FaceBookFName { get; set; }

        /// <summary>
        /// FaceBookLName
        /// </summary>
        public virtual string FaceBookLName { get; set; }

        /// <summary>
        /// FaceBookName
        /// </summary>
        public virtual string FaceBookName { get; set; }

        /// <summary>
        /// FaceBookLink
        /// </summary>
        public virtual string FaceBookLink { get; set; }

        /// <summary>
        /// FaceBookGender
        /// </summary>
        public virtual string FaceBookGender { get; set; }

    }
}
