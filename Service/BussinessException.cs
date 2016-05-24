using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Panduo.Service
{
    /// <summary>
    /// 业务异常类
    /// </summary> 
    public class BussinessException : Exception
    {
        private readonly IList<string> _errorCodeList;

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public BussinessException():base()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorCode">异常代码</param>
        public BussinessException(string errorCode): base(errorCode)
        {
            this._errorCodeList = new List<string> { errorCode }; 
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errors"></param>
        public BussinessException(IList<string> errors): base()
        {
            this._errorCodeList = errors;
        }

        /// <summary>
        /// 获取一个异常代码
        /// </summary>
        /// <returns></returns>
        public string GetError()
        {
            return this._errorCodeList == null ? null : this._errorCodeList[0];
        }

        /// <summary>
        /// 获取所有异常代码
        /// </summary>
        /// <returns></returns>
        public IList<string> GetAllErrors()
        {
            return this._errorCodeList;
        }
    }
}
