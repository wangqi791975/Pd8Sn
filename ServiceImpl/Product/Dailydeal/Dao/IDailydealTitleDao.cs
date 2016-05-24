using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using Com.Panduo.Entity.Product;
using Com.Panduo.Entity.Product.Dailydeal;

namespace Com.Panduo.ServiceImpl.Product.Dailydeal.Dao
{ 
    public interface IDailydealTitleDao:IBaseDao<DailydealTitlePo,int>
    {
        IList<DailydealTitlePo> GetAllTitles(int languageId);
    } 
}
   