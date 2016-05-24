using Com.Panduo.Entity.Customer.Club;

namespace Com.Panduo.ServiceImpl.Customer.Club.Dao
{
    public class ClubFeeDao : BaseDao<ClubFeePo, int>, IClubFeeDao
    {
        public ClubFeePo GetClubFee(int customerId)
        {
            return GetOneObject("FROM ClubFeePo WHERE CustomerId = ?", customerId);
        }
    }
}