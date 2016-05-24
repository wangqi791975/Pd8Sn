using Com.Panduo.Common;
using Com.Panduo.Entity.Customer;

namespace Com.Panduo.ServiceImpl.Customer.Dao
{
    public class SubscribeDao : BaseDao<SubscribePo, int>, ISubscribeDao
    {
        public SubscribePo GetSubscribe(string email)
        {
            return GetOneObject("FROM SubscribePo WHERE Email = ?", email);
        }

        public int GetSubscribeId(int languageId, string email)
        {
            var obj = GetSingleObject("SELECT Id FROM SubscribePo WHERE LanguageId = ? AND Email = ?", new object[] { languageId, email });
            return obj == null ? 0 : int.Parse(obj.ToString());
        }

        public void UpdateSubscribeStatus(string email, bool status)
        {
            UpdateObjectByHql("UPDATE SubscribePo SET Status = ? WHERE Email = ?", new object[] { status, email });
        }
    }
}