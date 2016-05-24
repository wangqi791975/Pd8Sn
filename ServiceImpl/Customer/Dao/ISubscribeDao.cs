using Com.Panduo.Entity.Customer;

namespace Com.Panduo.ServiceImpl.Customer.Dao
{
    public interface ISubscribeDao:IBaseDao<SubscribePo,int>
    {
        /// <summary>
        /// 获取订阅对象
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <returns>订阅对象</returns>
        SubscribePo GetSubscribe(string email);

        /// <summary>
        /// 通过邮箱获取订阅Id
        /// </summary>
        /// <param name="languageId">语种Id</param>
        /// <param name="email">邮箱</param>
        /// <returns></returns>
        int GetSubscribeId(int languageId,string email);

        /// <summary>
        /// 通过邮箱修改订阅状态
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="status">订阅状态</param>
        void UpdateSubscribeStatus(string email, bool status);

    }
}