using Com.Panduo.Entity.Customer.Club;

namespace Com.Panduo.ServiceImpl.Customer.Club.Dao
{
    public interface IClubFeeDao : IBaseDao<ClubFeePo,int>
    {
        /// <summary>
        /// 获取客户Club会费
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        ClubFeePo GetClubFee(int customerId);
    }
}