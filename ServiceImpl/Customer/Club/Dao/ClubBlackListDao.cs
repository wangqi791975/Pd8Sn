using Com.Panduo.Entity.Customer.Club;

namespace Com.Panduo.ServiceImpl.Customer.Club.Dao
{
    public class ClubBlackListDao : BaseDao<ClubBlackListPo, int>, IClubBlackListDao
    {
        public ClubBlackListPo GetClubBlackList(string customerEmail)
        {
            return GetOneObject("FROM ClubBlackListPo WHERE CustomerEmail = ?", customerEmail);
        }

        public void DeleteClubBlackList(string customerEmail)
        {
            DeleteObjectByHql("DELETE FROM ClubBlackListPo WHERE CustomerEmail = ?", customerEmail);
        }
    }
}