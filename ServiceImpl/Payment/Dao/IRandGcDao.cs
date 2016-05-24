using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Payment;

namespace Com.Panduo.ServiceImpl.Payment.Dao
{
    public interface IRandGcDao : IBaseDao<RandGcPo,int>
    {
        RandGcPo GetOneUnUsedGcNo();
    }
}
