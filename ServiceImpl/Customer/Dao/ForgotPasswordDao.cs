using System;
using System.Security.Cryptography.X509Certificates;
using Com.Panduo.Entity.Customer;

namespace Com.Panduo.ServiceImpl.Customer.Dao
{
    public class ForgotPasswordDao : BaseDao<ForgotPasswordPo, int>, IForgotPasswordDao
    {
        public void UpdateForgotPassword(int id, DateTime modifyDateTime, bool status)
        {
            UpdateObjectByHql("UPDATE ForgotPasswordPo SET DateUsed = ? , Status = ? WHERE Id = ?", new object[] { modifyDateTime, status, id });
        }

        public ForgotPasswordPo GetValidObjectByCode(string encryptCode)
        {
            return GetOneObject("FROM ForgotPasswordPo WHERE EncryptedString = ? AND Status = ?", new object[] { encryptCode, false });
        }
    }
}