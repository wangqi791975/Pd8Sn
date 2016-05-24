using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Com.Panduo.Common;
using Com.Panduo.Entity.Shipping;
using Com.Panduo.Service;
using Com.Panduo.Service.Order.ShippingOption;
using Com.Panduo.ServiceImpl.Order.ShippingOption.Dao;
using Com.Panduo.ServiceImpl.Shipping.Dao;
using NHibernate.Linq;

namespace Com.Panduo.ServiceImpl.Order.ShippingOption
{
    public class ShippingService : IShippingService
    {
        #region IOC
        public ICustomsNoDao CustomsNoDao { private get; set; }
        public IShippingDao ShippingDao { private get; set; }
        public IShippingDescDao ShippingDescDao { private get; set; }
        public IShippingDayDao ShippingDayDao { private get; set; }
        #endregion
        /// <summary>
        /// pobox正则
        /// </summary>
        private static string PO_BOX_PATTERN = "^.*(hc|p).*box.*$";
        public string ERROR_SHIPPING_NOT_EXISTS
        {
            get { return "ERROR_SHIPPING_NOT_EXISTS"; }
        }

        public CustomsNo GetCustomsNo(int shippingId, int countryId)
        {
            var customsNoPo = CustomsNoDao.GetCustomsNoPo(shippingId, countryId);
            CustomsNo customsNo = null;
            if (customsNoPo != null)
            {
                customsNo = GetCustomsNoFromPo(customsNoPo);
            }
            return customsNo;
        }

        public ShippingDay GetShippingDay(int shippingId, string countryIsoCode2)
        {
            ShippingDay shippingDay = null;
            var po = ShippingDayDao.GetShippingDay(shippingId, countryIsoCode2);
            if (!po.IsNullOrEmpty())
            {
                shippingDay = new ShippingDay
                {
                    DayHigh = po.DayHigh,
                    DayLow = po.DayLow,
                    CountryIsoCode2 = po.CountryIsoCode2,
                    ShippingId = po.ShippingId,
                    ShippingDayId = po.ShippingDayId
                };
            }
            return shippingDay;
        }

        public IList<ShippingLanguage> GetAllShippingDescs(int languageId)
        {
            var poList = ShippingDescDao.GetAll();
            var list = new List<ShippingLanguage>();
            foreach (var po in poList)
            {
                ShippingLanguage vo = null;
                if (!po.IsNullOrEmpty())
                {
                    vo = new ShippingLanguage();
                    ObjectHelper.CopyProperties(po, vo, new string[] { });
                }
                list.Add(vo);
            }
            return list;
        }

        public ShippingLanguage GetShippingDescById(int languageId, int shippingId)
        {
            var po = ShippingDescDao.GetOneObject("from ShippingDescPo where ShippingId=? and LanguageId=?", new object[] { shippingId, languageId });
            ShippingLanguage vo = null;
            if (po.IsNullOrEmpty()) return vo;
            vo = new ShippingLanguage();
            ObjectHelper.CopyProperties(po, vo, new string[] { });
            return vo;
        }

        public IList<Service.Order.ShippingOption.Shipping> GetAllShippings()
        {
            var poList = ShippingDao.GetAll();
            var voList = new List<Service.Order.ShippingOption.Shipping>();
            foreach (var po in poList)
            {
                Service.Order.ShippingOption.Shipping vo = null;
                if (!po.IsNullOrEmpty())
                {
                    vo = new Service.Order.ShippingOption.Shipping();
                    ObjectHelper.CopyProperties(po, vo, new string[] { });
                }
                voList.Add(vo);
            }
            return voList;
        }

        public bool IsPoBox(string address)
        {
            var isPoBox = address.IsMatch(PO_BOX_PATTERN);
            return isPoBox;
        }

        public ShippingAmount GetShippingAmount(int shippingId, ShipppingCriteria shipppingCriteria)
        {
            ShippingAmount shippingAmount = null;
            var shippingAmounts = GetShippingAmountsCommon(shippingId, shipppingCriteria);
            if (shippingAmounts.IsNullOrEmpty())
                shippingAmounts = GetShippingAmountsCommon(0, shipppingCriteria);
            if (!shippingAmounts.IsNullOrEmpty())
            {
                shippingAmount = shippingAmounts[0];
            }
            return shippingAmount;
        }

        public IList<ShippingAmount> GetShippingAmounts(ShipppingCriteria shipppingCriteria)
        {
            return GetShippingAmountsCommon(0, shipppingCriteria);
        }

        public List<ShippingAmount> GetShippingAmounts(ShipppingCriteria shipppingCriteria, List<Sorter<ShoppingAmountSorterCriteria>> sorterCriteria)
        {
            return GetShippingAmountsCommon(0, shipppingCriteria);
        }

        /// <summary>
        /// 获取配送方式的公用方法
        /// </summary>
        /// <param name="shippingId">配送方式ID</param>
        /// <param name="shipppingCriteria">配送方式查询条件</param>
        /// <returns></returns>
        public List<ShippingAmount> GetShippingAmountsCommon(int shippingId, ShipppingCriteria shipppingCriteria)
        {
            var shippingAmounts = new List<ShippingAmount>();
            //设置查询提交
            var parmsList = new List<SqlParameter>
                            {
                                new SqlParameter("@ShippingIdFind", SqlDbType.Int){Value =  shippingId}, 
                                new SqlParameter("@CountryIsoCode2", SqlDbType.VarChar, 2){Value =  shipppingCriteria.CountryIsoCode2},
                                new SqlParameter("@City", SqlDbType.VarChar, 32){Value = shipppingCriteria.City??""},
                                new SqlParameter("@PostCode", SqlDbType.VarChar, 32){Value = shipppingCriteria.PostCode},
                                new SqlParameter("@GrossWeight", SqlDbType.Decimal){Value = shipppingCriteria.GrossWeight}, 
                                new SqlParameter("@VolumeWeight", SqlDbType.Decimal){Value = shipppingCriteria.VolumeWeight},
                                new SqlParameter("@ClubLevel", SqlDbType.Decimal){Value = shipppingCriteria.ClubLevel},
                                new SqlParameter("@TotalAmount", SqlDbType.Decimal){Value = shipppingCriteria.TotalAmount}
                            };
            using (
                var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_shippings_get",
                    parmsList.ToArray()))
            {
                while (reader.Read())
                {
                    var shippingAmount = new ShippingAmount
                    {
                        ShippingId = reader.GetInt32(0),
                        ShippingCode = reader.GetString(1),
                        ShippingName = reader.GetString(2),
                        DayLow = reader.GetInt32(3),
                        DayHigh = reader.GetInt32(4),
                        TrackUrl = reader.GetString(5),
                        IsCalculateRemote = reader.GetBoolean(6),
                        IsCalculateVolume = reader.GetBoolean(7),
                        ShippingBoxNumber = reader.GetInt32(8),
                        ShippingCost = reader.GetDecimal(9),
                        RemoteAmount = reader.GetDecimal(10),
                        HandlingFeeForClub = reader.GetDecimal(11),
                        ClubShippingBalance = reader.GetDecimal(12),
                        IsDefault = false
                    };

                    shippingAmounts.Add(shippingAmount);
                }
                reader.Close();
            }
            if (shippingAmounts.Count > 0)
            {
                shippingAmounts[0].IsDefault = true;
            }

            return shippingAmounts;
        }

        #region 后台

        #region 配送方式

        public PageData<Service.Order.ShippingOption.Shipping> FindAllShippings(int currentPage, int pageSize,
            IDictionary<ShippingSearchCriteria, object> searchCriteria, IList<Sorter<ShippingSorterCriteria>> sorterCriteria)
        {
            var hqlHelper = new HqlHelper("FROM ShippingPo");
            //1.构建查询条件
            if (!searchCriteria.IsNullOrEmpty())
            {
                searchCriteria.ForEach(item =>
                {
                    switch (item.Key)
                    {
                        case ShippingSearchCriteria.Name:
                            hqlHelper.AddWhere("ShippingName", HqlOperator.Like, "ShippingName", item.Value);
                            break;
                    }
                });
            }
            //2.构建排序条件
            if (!sorterCriteria.IsNullOrEmpty())
            {
                sorterCriteria.ForEach(sorter =>
                {
                    switch (sorter.Key)
                    {
                        //case ShippingSorterCriteria.Status:
                        //    hqlHelper.AddSorter("Status", sorter.IsAsc);
                        //    break;
                    }
                });
            }
            else
            {
                hqlHelper.AddSorter("ShippingId", false);
            }
            //3.执行查询并返回数据
            var pageDataPo = ShippingDao.FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
            var pageDataVo = new PageData<Service.Order.ShippingOption.Shipping>();
            var voList = pageDataPo.Data.Select(GetShippingVoFromPo).ToList();

            pageDataVo.Pager = pageDataPo.Pager;
            pageDataVo.Data = voList;
            return pageDataVo;
        }

        private Service.Order.ShippingOption.Shipping GetShippingVoFromPo(ShippingPo shippingPo)
        {
            if (shippingPo.IsNullOrEmpty())
                return null;
            var shipping = new Service.Order.ShippingOption.Shipping
            {
                ShippingId = shippingPo.ShippingId,
                ShippingName = shippingPo.ShippingName,
                ExpressDelivery = shippingPo.ExpressDelivery,
                ExtraAmt = shippingPo.ExtraAmt,
                ExtraOil = shippingPo.ExtraOil,
                ExtraTimes = shippingPo.ExtraTimes,
                CalRemote = shippingPo.CalRemote,
                CalVolume = shippingPo.CalVolume,
                ShippingDiscount = shippingPo.ShippingDiscount,
                ShippingCode = shippingPo.ShippingCode,
                ShippingStatus = shippingPo.ShippingStatus,
            };
            return shipping;
        }

        public ShippingBaseInfo GetShippingById(int shippingId)
        {
            var shipping = ShippingDao.GetOneObject("from ShippingPo where ShippingId=?", shippingId);
            var shippingDescs = ShippingDescDao.FindDataByHql("from ShippingDescPo where ShippingId=?", shippingId);
            var shippingdays = ShippingDayDao.FindDataByHql("from ShippingDayPo where ShippingId=?", shippingId);
            var shippingBaseInfo = new ShippingBaseInfo
            {
                Shipping = GetShippingVoFromPo(shipping),
            };
            if (!shippingdays.IsNullOrEmpty())
                shippingBaseInfo.ShippingDay = shippingdays.Select(GetShippingDayVoFromPo).ToList();

            if (!shippingDescs.IsNullOrEmpty())
                shippingBaseInfo.ShippingLanguages = shippingDescs.Select(GetShippingLanguageVoFromPo).ToList();
            return shippingBaseInfo;
        }

        private ShippingLanguage GetShippingLanguageVoFromPo(ShippingDescPo shippingDescPo)
        {
            ShippingLanguage vo = null;
            if (shippingDescPo.IsNullOrEmpty()) return null;
            vo = new ShippingLanguage();
            ObjectHelper.CopyProperties(shippingDescPo, vo, new string[] { });
            return vo;
        }

        private ShippingDay GetShippingDayVoFromPo(ShippingDayPo shippingDayPo)
        {
            ShippingDay vo = null;
            if (shippingDayPo.IsNullOrEmpty()) return null;
            vo = new ShippingDay();
            ObjectHelper.CopyProperties(shippingDayPo, vo, new string[] { });
            return vo;
        }

        public void SetShipping(ShippingBaseInfo shippingBaseInfo)
        {
            //  1.保存运送方式主表
            var shippingPo = ShippingDao.GetObject(shippingBaseInfo.Shipping.ShippingId);
            if (shippingPo.IsNullOrEmpty())
            {
                shippingPo = GetShipptingPoFromVo(shippingBaseInfo.Shipping);
                shippingPo.ShippingId = ShippingDao.AddObject(shippingPo);
            }
            else
            {
                shippingPo.ShippingName = shippingBaseInfo.Shipping.ShippingName;
                shippingPo.ExpressDelivery = shippingBaseInfo.Shipping.ExpressDelivery;
                shippingPo.ExtraAmt = shippingBaseInfo.Shipping.ExtraAmt;
                shippingPo.ExtraOil = shippingBaseInfo.Shipping.ExtraOil;
                shippingPo.ExtraTimes = shippingBaseInfo.Shipping.ExtraTimes;
                shippingPo.CalRemote = shippingBaseInfo.Shipping.CalRemote;
                shippingPo.CalVolume = shippingBaseInfo.Shipping.CalVolume;
                shippingPo.ShippingDiscount = shippingBaseInfo.Shipping.ShippingDiscount;
                shippingPo.ShippingCode = shippingBaseInfo.Shipping.ShippingCode;
                shippingPo.ShippingStatus = shippingBaseInfo.Shipping.ShippingStatus;
                ShippingDao.UpdateObject(shippingPo);
            }

            //  2.设置运送方式多语种
            foreach (var shippingLanguage in shippingBaseInfo.ShippingLanguages)
            {
                var shippingDescPo = ShippingDescDao.GetOneObject("from ShippingDescPo where ShippingId=? and LanguageId=?", new object[] { shippingPo.ShippingId, shippingLanguage.LanguageId });
                if (shippingDescPo.IsNullOrEmpty())
                {
                    shippingDescPo = new ShippingDescPo
                    {
                        LanguageId = shippingLanguage.LanguageId,
                        Name = shippingLanguage.Name,
                        ShippingDescription = shippingLanguage.ShippingDescription,
                        ShippingId = shippingPo.ShippingId
                    };
                    ShippingDescDao.AddObject(shippingDescPo);
                }
                else
                {
                    shippingDescPo.Name = shippingLanguage.Name;
                    shippingDescPo.ShippingDescription = shippingLanguage.ShippingDescription;
                    ShippingDescDao.UpdateObject(shippingDescPo);
                }
            }

            //  3.设置运送方式国家到达天数
            ShippingDayDao.DeleteObjectByHql("delete from ShippingDayPo where ShippingId=?", new object[] { shippingPo.ShippingId });
            var shippingDayPos = shippingBaseInfo.ShippingDay.Select(day => new ShippingDayPo
            {
                ShippingId = shippingPo.ShippingId,
                DayLow = day.DayLow,
                DayHigh = day.DayHigh,
                CountryIsoCode2 = day.CountryIsoCode2,
            });
            ShippingDayDao.AddObjects(shippingDayPos);
        }

        private ShippingPo GetShipptingPoFromVo(Service.Order.ShippingOption.Shipping shipping)
        {
            ShippingPo shippingPo = null;
            if (shipping.IsNullOrEmpty()) return null;
            shippingPo = new ShippingPo();
            ObjectHelper.CopyProperties(shipping, shippingPo, new string[] { });
            return shippingPo;
        }

        public void DeleteShipping(int shippingId)
        {
            if (ShippingDao.GetObject(shippingId).IsNullOrEmpty())
            {
                throw new BussinessException(ERROR_SHIPPING_NOT_EXISTS);
            }
            ShippingDayDao.DeleteObjectByHql("delete from ShippingDayPo where ShippingId=?", new object[] { shippingId });
            ShippingDescDao.DeleteObjectByHql("delete from ShippingDescPo where ShippingId=?", new object[] { shippingId });
            ShippingDao.DeleteObjectById(shippingId);
        }

        #endregion


        #region 偏远城市和邮编
        public PageData<RemoteCity> FindRemoteCities(int currentPage, int pageSize,
            IDictionary<RemoteCitySearchCriteria, object> searchDictionary,
            IList<Sorter<RemoteCitySorterCriteria>> sorterCriteria)
        {
            var pageData = new PageData<RemoteCity>();
            var dataList = new List<RemoteCity>();
            var rowCount = 0;
            RemoteCity remoteCity = null;
            //设置查询提交
            var parmsList = new List<SqlParameter>
            {
                new SqlParameter("@pageIndex", SqlDbType.Int) {Value = currentPage},
                new SqlParameter("@pageSize", SqlDbType.Int) {Value = pageSize},
                new SqlParameter("@shippingcode", SqlDbType.VarChar, 20)
                {
                    Value = searchDictionary.TryGet(RemoteCitySearchCriteria.ShippingCode).ToSqlString()
                },
                new SqlParameter("@sortField", SqlDbType.VarChar, 100) {Value = string.Empty},
                new SqlParameter("@sortDirecton", SqlDbType.VarChar, 10) {Value = "ASC"}
            };
            //设置排序条件
            if (sorterCriteria != null)
            {
                foreach (var criteria in sorterCriteria)
                {
                    switch (criteria.Key)
                    {
                        case RemoteCitySorterCriteria.Id:
                            parmsList.Where(c => c.ParameterName == "@sortField").First().Value = "Id";
                            parmsList.Where(c => c.ParameterName == "@sortDirecton").First().Value = criteria.IsAsc
                                ? "ASC"
                                : "DESC";
                            break;
                    }
                }
            }
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_admin_remotecity_search", parmsList.ToArray()))
            {
                //数据条数
                if (reader.Read())
                {
                    rowCount = !reader.IsDBNull(0) ? reader.GetInt32(0) : 0;
                }
                reader.NextResult();

                while (reader.Read())
                {
                    remoteCity = new RemoteCity
                    {
                        Id = reader["Id"].ParseTo<int>(),
                        ShippingCode = reader["shipping_code"].ParseTo<string>(),
                        CountryCode = reader["country_code"].ParseTo<string>(),
                        CountryEnglishName = reader["country_name"].ParseTo<string>(),
                        City = reader["city"].ParseTo<string>(),
                    };
                    dataList.Add(remoteCity);
                }
            }
            pageData.Data = dataList;
            pageData.Pager = new Pager(rowCount, currentPage, pageSize);
            return pageData;
        }

        public PageData<RemoteZip> FindRemoteZips(int currentPage, int pageSize,
            IDictionary<RemoteZipSearchCriteria, object> searchDictionary,
            IList<Sorter<RemoteZipSorterCriteria>> sorterCriteria)
        {
            var pageData = new PageData<RemoteZip>();
            var dataList = new List<RemoteZip>();
            var rowCount = 0;
            RemoteZip remoteZip = null;
            //设置查询提交
            var parmsList = new List<SqlParameter>
            {
                new SqlParameter("@pageIndex", SqlDbType.Int) {Value = currentPage},
                new SqlParameter("@pageSize", SqlDbType.Int) {Value = pageSize},
                new SqlParameter("@shippingcode", SqlDbType.VarChar, 20)
                {
                    Value = searchDictionary.TryGet(RemoteZipSearchCriteria.ShippingCode).ToSqlString()
                },
                new SqlParameter("@sortField", SqlDbType.VarChar, 100) {Value = string.Empty},
                new SqlParameter("@sortDirecton", SqlDbType.VarChar, 10) {Value = "ASC"}
            };
            //设置排序条件
            if (sorterCriteria != null)
            {
                foreach (var criteria in sorterCriteria)
                {
                    switch (criteria.Key)
                    {
                        case RemoteZipSorterCriteria.Id:
                            parmsList.Where(c => c.ParameterName == "@sortField").First().Value = "Id";
                            parmsList.Where(c => c.ParameterName == "@sortDirecton").First().Value = criteria.IsAsc
                                ? "ASC"
                                : "DESC";
                            break;
                    }
                }
            }
            using (var reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING, CommandType.StoredProcedure, "up_admin_remotezip_search", parmsList.ToArray()))
            {
                //数据条数
                if (reader.Read())
                {
                    rowCount = !reader.IsDBNull(0) ? reader.GetInt32(0) : 0;
                }
                reader.NextResult();

                while (reader.Read())
                {
                    remoteZip = new RemoteZip
                    {
                        Id = reader["Id"].ParseTo<int>(),
                        ShippingCode = reader["shipping_code"].ParseTo<string>(),
                        CountryCode = reader["country_code"].ParseTo<string>(),
                        CountryEnglishName = reader["country_name"].ParseTo<string>(),
                        PostcodeStart = reader["postcode_start"].ParseTo<string>(),
                        PostcodeEnd = reader["postcode_end"].ParseTo<string>(),
                    }; 
                    dataList.Add(remoteZip);
                }
            }
            pageData.Data = dataList;
            pageData.Pager = new Pager(rowCount, currentPage, pageSize);
            return pageData;
        }

        public void SyncRemoteData()
        {
            throw new NotImplementedException();
        }
        #endregion

        #endregion

        #region 公用方法

        private CustomsNo GetCustomsNoFromPo(CustomsNoPo customsNoPo)
        {
            CustomsNo customsNo = null;
            if (customsNoPo != null)
            {
                customsNo = new CustomsNo
                {
                    CustomsNoType = (CustomsNoType)customsNoPo.Type,
                    IsRequired = customsNoPo.IsRequired,
                    Note = customsNoPo.Note

                };
            }
            return customsNo;
        }

        #endregion
    }
}
