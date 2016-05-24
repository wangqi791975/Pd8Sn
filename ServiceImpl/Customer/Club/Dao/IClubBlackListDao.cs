using Com.Panduo.Entity.Customer.Club;

namespace Com.Panduo.ServiceImpl.Customer.Club.Dao
{
    public interface IClubBlackListDao : IBaseDao<ClubBlackListPo,int>
    {
        /// <summary>
        /// 通过邮箱获取club黑名单
        /// </summary>
        /// <param name="customerEmail">客户邮箱</param>
        /// <returns>club黑名单</returns>
        ClubBlackListPo GetClubBlackList(string customerEmail);

        /// <summary>
        /// 删除客户邮箱
        /// </summary>
        /// <param name="customerEmail"></param>
        void DeleteClubBlackList(string customerEmail);
    }
}