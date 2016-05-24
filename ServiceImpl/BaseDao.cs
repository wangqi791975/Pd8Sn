using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Com.Panduo.Common;
using Com.Panduo.Service;
using NHibernate;
using NHibernate.Mapping;
using Spring.Data.NHibernate.Generic.Support; 

namespace Com.Panduo.ServiceImpl
{
	/// <summary>
	/// phc
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="PK"></typeparam>
	public class BaseDao<T, PK> : HibernateDaoSupport, IBaseDao<T, PK>
	{
		public static readonly int BatchSize = 50;

		/// <summary>
		/// 设置Session刷新模式
		/// </summary>
		/// <param name="flushMode"></param>
		private void SetSessionFlushMode(FlushMode flushMode)
		{
			Session.FlushMode = flushMode;
		}

		public PK AddObject(T entity)
		{
			SetSessionFlushMode(FlushMode.Auto);
			var o = HibernateTemplate.Save(entity);
			return (PK)o;
		}
		 
		public IList<PK> AddObjects(IEnumerable<T> list)
		{
			var successList = new List<PK>();
			var session = Session;
			session.FlushMode = FlushMode.Auto;

			for (var i = 0; i < list.Count(); i++)
			{
				var id = (PK)session.Save(list.ElementAt(i));
				successList.Add(id);
				if (((i + 1) % BatchSize) == 0)
				{
					session.Flush();
					session.Clear();
				}
			}

			return successList;
		}

		public void UpdateObject(T entity)
		{
			SetSessionFlushMode(FlushMode.Auto);
			HibernateTemplate.Update(entity);
		}

		public void UpdateObjects(IEnumerable<T> list)
		{
			var session = Session;
			session.FlushMode = FlushMode.Auto;

			for (var i = 0; i < list.Count(); i++)
			{
				session.Update(list.ElementAt(i));
				if (((i + 1) % BatchSize) == 0)
				{
					session.Flush();
					session.Clear();
				}
			}
		}

		public void UpdateObjectByHql(string hql)
		{
			UpdateObjectByHql(hql, new object[] { });
		}

		public void UpdateObjectByHql(string hql, object parameter)
		{
			UpdateObjectByHql(hql, new[] { parameter });
		}

		public void UpdateObjectByHql(string hql, object[] parameters)
		{
			var session = Session;

			var query = session.CreateQuery(hql);
			if (!parameters.IsNullOrEmpty())
			{
				for (var i = 0; i < parameters.Length; i++)
				{
					query.SetParameter(i, parameters[i]);
				}
			}

			query.ExecuteUpdate();

			ReleaseSession(session);
		}

	    public void UpdateObjectByHql(string hql, IList<KeyValuePair<string, object>> parameters)
	    {
            var session = Session;

            var query = session.CreateQuery(hql);
            if (!parameters.IsNullOrEmpty())
            {
                foreach (var item in parameters)
                {
                    if (!(item.Value is string) && item.Value is IEnumerable)
                    {
                        query.SetParameterList(item.Key, item.Value as IList);
                    }
                    else
                    {
                        query.SetParameter(item.Key, item.Value);
                    }
                }
            }

            query.ExecuteUpdate();

            ReleaseSession(session);
	    }

		public void DeleteObjectById(PK pk)
		{
			SetSessionFlushMode(FlushMode.Auto);
			var obj = HibernateTemplate.Load<T>(pk);
			HibernateTemplate.Delete(obj);
		}

		public void DeleteObjectByIds(IEnumerable<PK> pks)
		{
			var session = Session;
			session.FlushMode = FlushMode.Auto;

			for (var i = 0; i < pks.Count(); i++)
			{
				session.Delete(session.Load<T>(pks.ElementAt(i)));
				if (((i + 1) % BatchSize) == 0)
				{
					session.Flush();
					session.Clear();
				}
			}
		}

		public void DeleteObject(T entity)
		{
			SetSessionFlushMode(FlushMode.Auto);
			HibernateTemplate.Delete(entity);
		}

		public void DeleteObjects(string filedName, object value)
		{
			var session = Session;

			var query = session.CreateQuery(string.Format("delete from {0} where {1}=?",typeof(T),filedName));
			query.SetParameter(0, value);
		
			query.ExecuteUpdate();
		
			ReleaseSession(session); 
		}

		public void DeleteObjects(IEnumerable<T> objects)
		{
			var session = Session;
			session.FlushMode = FlushMode.Auto;

			for (var i = 0; i < objects.Count(); i++)
			{
				session.Delete(objects.ElementAt(i));
				if (((i + 1) % BatchSize) == 0)
				{
					session.Flush();
					session.Clear();
				}
			}
		}

		public void DeleteObjectByHql(string hql)
		{
			DeleteObjectByHql(hql, new object[] { });
		}

		public void DeleteObjectByHql(string hql, object parameter)
		{
			DeleteObjectByHql(hql, new [] { parameter });
		}

		public void DeleteObjectByHql(string hql, object[] parameters)
		{
			var session = Session;

			var query = session.CreateQuery(hql);
			if (!parameters.IsNullOrEmpty())
			{
				for (var i = 0; i < parameters.Length; i++)
				{ 
					query.SetParameter(i, parameters[i]);
				}
			}

			query.ExecuteUpdate();

			ReleaseSession(session);
		}

        public void DeleteObjectByHql(string hql, IList<KeyValuePair<string, object>> parameters)
        {
            var session = Session;

            var query = session.CreateQuery(hql);
            if (!parameters.IsNullOrEmpty())
            {
                foreach (var item in parameters)
                {
                    if (!(item.Value is string) && item.Value is IEnumerable)
                    {
                        query.SetParameterList(item.Key, item.Value as IList);
                    }
                    else
                    {
                        query.SetParameter(item.Key, item.Value);
                    }
                }
            }

            query.ExecuteUpdate();

            ReleaseSession(session);
        }

		public void DeleteAll()
		{
			HibernateTemplate.Delete(string.Format("from {0}", typeof (T)));
		}

		public T GetObject(PK pk)
		{
			return HibernateTemplate.Get<T>(pk);
		}

		public T GetObject(PK pk, LockMode lockMode)
		{
			return HibernateTemplate.Get<T>(pk,lockMode);
		}

		public T GetOneObject(string hql)
		{
			return GetOneObject(hql, new object[]{});
		}

		public T GetOneObject(string hql, object parameter)
		{
			return GetOneObject(hql, new [] { parameter });
		}

		public T GetOneObject(string hql, object[] parameters)
		{
			var session = Session;
			var query = session.CreateQuery(hql);

			if (!parameters.IsNullOrEmpty())
			{
				for (var i = 0; i < parameters.Length; i++)
				{
					query.SetParameter(i, parameters[i]);
				}
			}

			var data =query.SetFirstResult(0).SetMaxResults(1).List<T>();

			//手动释放
			ReleaseSession(session);

			return data.IsNullOrEmpty()?default(T): data.ElementAt(0);
		}

        public T GetOneObject(string hql, IList<KeyValuePair<string, object>> parameters)
        {
            var session = Session;
            var query = session.CreateQuery(hql);

            if (!parameters.IsNullOrEmpty())
            {
                foreach (var item in parameters)
                {
                    if (!(item.Value is string) && item.Value is IEnumerable)
                    {
                        query.SetParameterList(item.Key, item.Value as IList);
                    }
                    else
                    {
                        query.SetParameter(item.Key, item.Value);
                    }
                }
            }

            var data = query.SetFirstResult(0).SetMaxResults(1).List<T>();

            //手动释放
            ReleaseSession(session);

            return data.IsNullOrEmpty() ? default(T) : data.ElementAt(0);
        } 

		public object GetSingleObject(string hql)
		{
			return GetSingleObject(hql, new object[] {});
		}

		public object GetSingleObject(string hql, object parameter)
		{
			return GetSingleObject(hql, new [] { parameter });
		}

		public object GetSingleObject(string hql, object[] parameters)
		{ 
			var data = HibernateTemplate.Find<object>(hql, parameters);
			
			return data.IsNullOrEmpty() ? null : data[0];
		} 

		public IList<T> GetAll()
		{
			return HibernateTemplate.LoadAll<T>();
		}

		/// <summary>
		/// 通过hql获取对象
		/// </summary>
		/// <param name="hql"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public T FindByHql(string hql, params object[] parameters)
		{
			if (parameters.Length == 0)
				return HibernateTemplate.Find<T>(hql).FirstOrDefault();

			if (parameters.Length == 1)
			{
				var kvParameters = parameters[0] as IDictionary<string, object>;
				if (kvParameters != null)
				{
					return
						HibernateTemplate.FindByNamedParam<T>(hql, kvParameters.Keys.ToArray(),
															   kvParameters.Values.ToArray()).FirstOrDefault();
				}
			}

			return HibernateTemplate.Find<T>(hql, parameters).FirstOrDefault();
		}

		public T FindByHql(IList<KeyValuePair<string, object>> fieldValues)
		{
			return FindDataByHql(fieldValues).FirstOrDefault();
		}

		/// <summary>
		/// 通过hql获取对象列表
		/// </summary>
		/// <param name="hql"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public IList<T> FindDataByHql(string hql, params object[] parameters)
		{
			return FindDataByHqlLimit(hql, -1, parameters);
		}

		/// <summary>
		/// 通过hql获取对象列表
		/// </summary>
		/// <param name="hql"></param>
		/// <param name="limit"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public IList<T> FindDataByHqlLimit(string hql, int limit = -1, params object[] parameters)
		{ 
			var query = Session.CreateQuery(hql);
			for (var i = 0; i < parameters.Length; i++)
			{
				query.SetParameter(i, parameters[i]);
			}

			IList<T> data;
			if (limit<=0)
			{
				data = query.List<T>();
			}
			else
			{
			   data = query.SetMaxResults(limit).List<T>(); 
			}

			ReleaseSession(Session);

			return data;
		}

        public IList<T> FindDataByHql(string hql, IList<KeyValuePair<string, object>> parameters, int limit = -1)
        {
            var query = Session.CreateQuery(hql);
            if (!parameters.IsNullOrEmpty())
            {
                foreach (var item in parameters)
                {
                    if (!(item.Value is string) && item.Value is IEnumerable)
                    {
                        query.SetParameterList(item.Key, item.Value as IList);
                    }
                    else
                    {
                        query.SetParameter(item.Key, item.Value);
                    }
                }
            }

            IList<T> data;
            if (limit <= 0)
            {
                data = query.List<T>();
            }
            else
            {
                data = query.SetMaxResults(limit).List<T>();
            }

            ReleaseSession(Session);

            return data;
        }

		public IList<T> FindDataByHql(IList<KeyValuePair<string, object>> fieldValues)
		{
			var where = new List<string>();
			if (!fieldValues.IsNullOrEmpty())
			{
				foreach (var item in fieldValues)
				{
					where.Add(string.Format("dataPo.{0} = ?", item.Key));
				}
			}
			return HibernateTemplate.Find<T>(string.Format("from {0} dataPo {1}", typeof(T), where.IsNullOrEmpty() ? string.Empty : "where " + where.Join(" and ")),fieldValues.Select(c => c.Value).ToArray());
		}

		/// <summary>
		/// 通过hql获取对象列表
		/// </summary>
		/// <param name="hql"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public IList<object> FindObjectByHql(string hql, params object[] parameters)
		{
			if (parameters.Length == 0)
				return HibernateTemplate.Find<object>(hql);

			if (parameters.Length == 1)
			{
				var kvParameters = parameters[0] as IDictionary<string, object>;
				if (kvParameters != null)
				{
					return
						HibernateTemplate.FindByNamedParam<object>(hql, kvParameters.Keys.ToArray(),kvParameters.Values.ToArray());
				}
			}

			return HibernateTemplate.Find<object>(hql, parameters);
		}


		public IList<object> FindObjectByHql(IList<KeyValuePair<string, object>> fieldValues)
		{
			var where = new List<string>();
			if (!fieldValues.IsNullOrEmpty())
			{
				foreach (var item in fieldValues)
				{
					where.Add(string.Format("dataPo.{0} = ?", item.Key));
				}
			}
			return HibernateTemplate.Find<object>(string.Format("from {0} dataPo {1}", typeof(T), where.IsNullOrEmpty() ? string.Empty : "where " + where.Join(" and ")), fieldValues.Select(c => c.Value).ToArray());
		} 

		/// <summary>
		///  通过hql获取对象分页，列表要求hql符合一定格式...
		/// <para>期望的hql是 "[select ...] from ... where ... order by ...", 不符合格式的hql可能会报错</para>
		/// </summary>
		/// <param name="currentPage"></param>
		/// <param name="pageSize"></param>
		/// <param name="hql"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public PageData<T> FindPageDataByHql(int currentPage, int pageSize, string hql, params object[] parameters)
		{
			// 期望的hql是 "[select ...] from ... where ... order by ...", 不符合格式的hql可能会报错
			// TODO 字符串查找并不保险 
			var pos = hql.ToLower().IndexOf("from ", StringComparison.InvariantCultureIgnoreCase);
			var countHql = hql.Substring(pos);

			pos = countHql.ToLower().IndexOf(" order by ", StringComparison.InvariantCultureIgnoreCase);
			if (pos != -1)
			{
				countHql = countHql.Substring(0, pos);
			}

			countHql = " select count (*) " + countHql;
			
			var totalCount = Convert.ToInt32(ExecOneValue<object>(countHql, parameters));

			var page = new Pager(totalCount, currentPage, pageSize);

			// TODO 实际查询返回分页对象
			var query = Session.CreateQuery(hql);
			if (parameters.Length == 1)
			{
				var kvParameters = parameters[0] as IDictionary<string, object>;
				if (kvParameters != null)
				{
					foreach (var kv in kvParameters)
					{
                        if (!(kv.Value is string) && kv.Value is IEnumerable)
                        {
                            query.SetParameterList(kv.Key, kv.Value as IList);
                        }
                        else
                        {
                            query.SetParameter(kv.Key, kv.Value);
                        }  
					}
				}
				else
				{
					query.SetParameter(0, parameters[0]);
				}
			}
			else
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					query.SetParameter(i, parameters[i]);
				}
			}
			var data = query.SetFirstResult(page.StartRowNumber - 1).SetMaxResults(pageSize).List<T>();

			var pl = new PageData<T> { Data = data, Pager = page };
			return pl;
		}
		
		public PageData<T> FindPageDataByHql(int currentPage, int pageSize, string hql, IDictionary<string, object> keyValueMap)
		{
			keyValueMap = keyValueMap.IsNullOrEmpty() ? new Dictionary<string, object>():keyValueMap;
			var pos = hql.ToLower().IndexOf("from ", StringComparison.InvariantCultureIgnoreCase);
			var countHql = hql.Substring(pos); // from ... where ... order by ...
			pos = countHql.ToLower().IndexOf(" order by ",StringComparison.InvariantCultureIgnoreCase);
			if (pos != -1)
			{
				countHql = countHql.Substring(0, pos); // from ... where ... 
			}
			countHql = " select count(*) " + countHql;

			//获取分页信息 
			var countlist =HibernateTemplate.FindByNamedParam<object>(countHql, keyValueMap.Keys.ToArray(), keyValueMap.Values.ToArray());
			var totalCount = 0;
			int.TryParse(countlist.ElementAt(0).ToString(), out totalCount); 
			var page = new Pager(totalCount, currentPage, pageSize); 

			//获取分页对象 
			var query = Session.CreateQuery(hql);
			foreach (var item in keyValueMap)
			{
                if (!(item.Value is string) && item.Value is IEnumerable)
                {
                    query.SetParameterList(item.Key, item.Value as IList);
                }
                else
                {
                    query.SetParameter(item.Key, item.Value);
                } 
			}

			var data = query.SetFirstResult(page.StartRowNumber - 1).SetMaxResults(pageSize).List<T>();
			var pageData = new PageData<T>()
				{
					Data = data,
					Pager = page
				};

			ReleaseSession(Session);

			return pageData;
		}

		public PageData<T> FindPageDataBySql(int currentPage, int pageSize, string sql, IDictionary<string, object> keyValueMap)
		{
			// 期望的hql是 "[select ...] from ... where ... order by ...", 不符合格式的hql可能会报错
			// TODO 字符串查找并不保险
			var pos = sql.ToLower().IndexOf("from ", StringComparison.InvariantCultureIgnoreCase);
			var countHql = sql.Substring(pos); // from ... where ... order by ...
			pos = countHql.ToLower().IndexOf(" order by ", StringComparison.InvariantCultureIgnoreCase);
			if (pos != -1)
			{
				countHql = countHql.Substring(0, pos); // from ... where ... 
			}
			countHql = " select count(*) " + countHql;

			var sqlQuery = Session.CreateSQLQuery(countHql);
			sqlQuery.SetProperties(keyValueMap);

			var totalCount = 0;
			int.TryParse(sqlQuery.UniqueResult().ToString(), out totalCount);
			Pager page = new Pager(totalCount, currentPage, pageSize);

			// 实际查询返回分页对象
			var query = Session.CreateSQLQuery(sql);
			query.SetProperties(keyValueMap);
			query.AddEntity(typeof(T));

			var data = (IList<T>)query.SetFirstResult(page.StartRowNumber - 1).SetMaxResults(pageSize).List();
			var pageData = new PageData<T>();
			pageData.Data = data;
			pageData.Pager = page;

			ReleaseSession(Session);

			return pageData;
		}
        
		public TX ExecOneValue<TX>(string hsql, params object[] parameters)
		{
			if (parameters.Length == 0)
				return (TX)HibernateTemplate.Find<object>(hsql).FirstOrDefault();

			if (parameters.Length == 1)
			{
				var kvParameters = parameters[0] as IDictionary<string, object>;
				if (kvParameters != null)
				{
					return
						(TX)HibernateTemplate.FindByNamedParam<object>(hsql, kvParameters.Keys.ToArray(),
															   kvParameters.Values.ToArray()).FirstOrDefault();
				}
			}

			return (TX)HibernateTemplate.Find<object>(hsql, parameters).FirstOrDefault();
		}  

		public IList<T> GetListWithFieldValue(string fieldName, object fieldValue)
		{
			return HibernateTemplate.FindByNamedParam<T>(string.Format("from {0} dataPo where dataPo.{1}=:{1}", typeof(T), fieldName), new[] { fieldName }, new [] { fieldValue });
		}

		public IList<T> GetListWithFieldValueExceptId(string fieldName, object fieldValue, string idName, PK id)
		{
			return HibernateTemplate.FindByNamedParam<T>(string.Format("from {0} dataPo where dataPo.{1} !=:{1} and dataPo.{2} =:{2}", typeof(T), idName,fieldName), new[] { fieldName, idName }, new [] { fieldValue, id });
		}

		public bool ExistObject(string fieldName, object fieldValue)
		{
			return !GetListWithFieldValue(fieldName, fieldValue).IsNullOrEmpty();
		}

		public bool ExistObjectExceptId(string fieldName, object fieldValue, string idName, PK id)
		{
			return !GetListWithFieldValueExceptId(fieldName, fieldValue, idName, id).IsNullOrEmpty();
		}

		public int GetObjectCount(string whereHql)
		{
			var count = HibernateTemplate.Find<object>(string.Format("select count(*) from {0} dataPo {1}", typeof(T), string.IsNullOrEmpty(whereHql) ? string.Empty : "where " + whereHql)).FirstOrDefault();
			return Convert.ToInt32(count);
		}

		public void ExecuteByHql(string hql)
		{
			ExecuteByHql(hql, new object[] { });
		}

		public void ExecuteByHql(string hql, object parameter)
		{
			ExecuteByHql(hql, new[] { parameter });
		}

		public void ExecuteByHql(string hql, object[] parameters)
		{
			var session = Session;

			var query = session.CreateQuery(hql);
			if (!parameters.IsNullOrEmpty())
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					query.SetParameter(i, parameters[i]);
				}
			}

			query.ExecuteUpdate();

			ReleaseSession(session);
		}

        public void ExecuteByHql(string hql, IList<KeyValuePair<string, object>> parameters)
        {
            var session = Session;

            var query = session.CreateQuery(hql);
            if (!parameters.IsNullOrEmpty())
            {
                foreach (var item in parameters)
                {
                    if (!(item.Value is string) && item.Value is IEnumerable)
                    {
                        query.SetParameterList(item.Key, item.Value as IList);
                    }
                    else
                    {
                        query.SetParameter(item.Key, item.Value);
                    }
                }
            }

            query.ExecuteUpdate();

            ReleaseSession(session);
        }
	}
}
