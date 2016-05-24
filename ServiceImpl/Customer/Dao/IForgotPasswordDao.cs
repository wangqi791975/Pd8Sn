using System;
using Com.Panduo.Entity.Customer;

namespace Com.Panduo.ServiceImpl.Customer.Dao
{
    public interface IForgotPasswordDao:IBaseDao<ForgotPasswordPo,int>
    {
        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="id">忘记密码Id</param>
        /// <param name="modifyDateTime">修改时间</param>
        /// <param name="status">是否有效状态</param>
        void UpdateForgotPassword(int id, DateTime modifyDateTime, bool status);

        /// <summary>
        /// 判断密码串是否存在
        /// </summary>
        /// <param name="encryptCode">加密串</param>
        /// <returns>是否存在</returns>
        ForgotPasswordPo GetValidObjectByCode(string encryptCode);
    }
}