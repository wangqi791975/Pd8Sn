using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Payment;

namespace Com.Panduo.ServiceImpl.Payment.Dao
{
    public class RandGcDao : BaseDao<RandGcPo, int>, IRandGcDao
    {
        public RandGcPo GetOneUnUsedGcNo()
        {
            return GetOneObject("from RandGcPo where Status = false");
        }
    }
}
