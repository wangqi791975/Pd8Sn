using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Service;
using NHibernate;

namespace Com.Panduo.ServiceImpl
{
    /// <summary>
    /// 数据访问层基类-phc
    /// </summary>
    /// <typeparam name="T">实体对象类型</typeparam>
    /// <typeparam name="Pk">主键类型</typeparam>
    public interface IBaseDao<T, Pk>
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Pk AddObject(T entity);

        /// <summary>
        /// 批量添加,返回添加的对象主键
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        IList<Pk> AddObjects(IEnumerable<T> list);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        void UpdateObject(T entity);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        void UpdateObjects(IEnumerable<T> list);

        /// <summary>
        /// 通过hql更新对象
        /// </summary> 
        void UpdateObjectByHql(string hql);

        /// <summary>
        /// 通过hql更新对象
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameter"></param>
        void UpdateObjectByHql(string hql, object parameter);

        /// <summary>
        /// 通过hql更新对象
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameters"></param>
        void UpdateObjectByHql(string hql, object[] parameters);

        /// <summary>
        /// 通过hql更新对象
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameters">Key为命名参数名称,Value-对应的参数</param>
        void UpdateObjectByHql(string hql, IList<KeyValuePair<string, object>> parameters);

        /// <summary>
        /// 使用主键删除
        /// </summary>
        /// <param name="pk"></param>
        void DeleteObjectById(Pk pk);

        /// <summary>
        /// 使用主键批量删除
        /// </summary>
        /// <param name="pks"></param>
        void DeleteObjectByIds(IEnumerable<Pk> pks);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        void DeleteObject(T entity);

        /// <summary>
        /// 删除指定字段为指定值的对象
        /// </summary>
        /// <param name="filedName"></param>
        /// <param name="value"></param>
        void DeleteObjects(string filedName, object value);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="objects"></param>
        void DeleteObjects(IEnumerable<T> objects);
        /// <summary>
        /// 通过hql删除对象
        /// </summary> 
        void DeleteObjectByHql(string hql);

        /// <summary>
        /// 通过hql删除对象
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameter"></param>
        void DeleteObjectByHql(string hql, object parameter);

        /// <summary>
        /// 通过hql删除对象
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameters"></param>
        void DeleteObjectByHql(string hql, object[] parameters);

        /// <summary>
        /// 通过hql删除对象
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameters">Key为命名参数名称,Value-对应的参数</param>
        void DeleteObjectByHql(string hql, IList<KeyValuePair<string, object>> parameters);

        /// <summary>
        /// 删除所有对象
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        T GetObject(Pk pk);

        /// <summary>
        /// 指定锁模式获取对象
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="lockMode"></param>
        /// <returns></returns>
        T GetObject(Pk pk, LockMode lockMode);

        /// <summary>
        /// 通过hql获取一个对象
        /// </summary>
        /// <param name="hql"></param>
        /// <returns></returns>
        T GetOneObject(string hql);

        /// <summary>
        /// 通过hql和一个参数获取一个对象
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        T GetOneObject(string hql, object parameter);

        /// <summary>
        /// 通过hql和多个参数获取一个对象
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        T GetOneObject(string hql, object[] parameters);

        /// <summary>
        /// 通过hql和多个参数获取一个对象
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameters">Key为命名参数名称,Value-对应的参数</param>
        /// <returns></returns>
        T GetOneObject(string hql, IList<KeyValuePair<string, object>> parameters);

        /// <summary>
        /// 通过hql获取一个普通对象
        /// </summary>
        /// <param name="hql"></param>
        /// <returns></returns>
        object GetSingleObject(string hql);

        /// <summary>
        /// 通过hql和一个参数获取一个普通对象
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        object GetSingleObject(string hql, object parameter);

        /// <summary>
        /// 通过hql和多个参数获取一个普通对象
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        object GetSingleObject(string hql, object[] parameters); 

        /// <summary>
        /// 获取所有对象
        /// </summary>
        /// <returns></returns>
        IList<T> GetAll();

        /// <summary>
        /// 通过hql获取一个对象
        /// </summary>
        /// <param name="hql">hql查询语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns></returns>
        T FindByHql(string hql, params object[] parameters);
        
        /// <summary>
        /// 通过对象属性和对应的属性值查找一个对象
        /// </summary> 
        /// <param name="fieldValues">属性和属性值键值对</param>
        /// <returns></returns>
        T FindByHql(IList<KeyValuePair<string,object >> fieldValues); 

        /// <summary>
        /// 通过hql获取对象列表
        /// </summary>
        /// <param name="hql">hql查询语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns></returns>
        IList<T> FindDataByHql(string hql, params object[] parameters);

        /// <summary>
        /// 通过hql获取对象列表
        /// </summary>
        /// <param name="hql">hql查询语句</param>
        /// <param name="limit">个数限制</param>
        /// <param name="parameters">查询参数</param>
        /// <returns></returns>
        IList<T> FindDataByHqlLimit(string hql, int limit=-1, params object[] parameters);

        /// <summary>
        /// 通过hql获取对象列表
        /// </summary>
        /// <param name="hql">hql查询语句</param>
        /// <param name="parameters">查询参数,key-命名参数,value-参数值</param>
        /// <param name="limit">返回个数限制</param>
        /// <returns></returns>
        IList<T> FindDataByHql(string hql, IList<KeyValuePair<string, object>> parameters, int limit = -1);

        /// <summary>
        /// 通过对象属性和对应的属性值查找符合条件的所有对象
        /// </summary>
        /// <param name="fieldValues">属性和属性值键值对</param>
        /// <returns></returns>
        IList<object> FindObjectByHql(IList<KeyValuePair<string, object>> fieldValues);

        /// <summary>
        /// 通过hql获取对象列表
        /// </summary>
        /// <param name="hql">hql查询语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns></returns>
        IList<object> FindObjectByHql(string hql, params object[] parameters);

        /// <summary>
        /// 通过对象属性和对应的属性值查找符合条件的所有对象
        /// </summary>
        /// <param name="fieldValues">属性和属性值键值对</param>
        /// <returns></returns>
        IList<T> FindDataByHql(IList<KeyValuePair<string, object>> fieldValues); 
        /// <summary>
        /// 分页查询对象
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="hql">查询hql</param>
        /// <param name="parmas">查询参数</param>
        /// <returns></returns>
        PageData<T> FindPageDataByHql(int currentPage, int pageSize, string hql, params object[] parmas);

        /// <summary>
        /// 分页查询对象
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="hql">查询hql</param>
        /// <param name="keyValueMap">查询参数键值对</param>
        /// <returns></returns>
        PageData<T> FindPageDataByHql(int currentPage, int pageSize, string hql, IDictionary<string,object> keyValueMap);

        /// <summary>
        /// 分页查询对象
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="sql">查询hql</param>
        /// <param name="keyValueMap">查询参数</param>
        /// <returns></returns>
        PageData<T> FindPageDataBySql(int currentPage, int pageSize, string sql, IDictionary<string, object> keyValueMap);

        /// <summary>
        /// 根据属性字段查找数据
        /// </summary>
        /// <param name="fieldName">要查找的字段</param>
        /// <param name="fieldValue">要查找的值</param>
        /// <returns></returns>
        IList<T> GetListWithFieldValue(string fieldName, object fieldValue);

        /// <summary>
        /// 根据属性字段查找数据
        /// </summary>
        /// <param name="fieldName">要查找的字段</param>
        /// <param name="fieldValue">要查找的值</param>
        /// <param name="idName">Id对应的属性名</param>
        /// <param name="id">要排除的对象Id</param>
        /// <returns></returns>
        IList<T> GetListWithFieldValueExceptId(string fieldName, object fieldValue, string idName, Pk id);

        /// <summary>
        /// 查找是否存在值为fieldValue的字段fieldName
        /// </summary>
        /// <param name="fieldName">要查找的字段</param>
        /// <param name="fieldValue">要查找的值</param>
        /// <returns>是否存在</returns>
        bool ExistObject(string fieldName, object fieldValue);

        /// <summary>
        /// 查找是否存在值为fieldValue的字段fieldName,要排除指定对象
        /// </summary>
        /// <param name="fieldName">要查找的字段</param>
        /// <param name="fieldValue">要查找的值</param>
        /// <param name="idName">Id对应的属性名</param>
        /// <param name="id">要排除的对象Id</param>
        /// <returns></returns>
        bool ExistObjectExceptId(string fieldName, object fieldValue, string idName, Pk id);

        /// <summary>
        /// 获取这种类型的对象个数
        /// </summary>
        /// <param name="whereHql">过滤的hql</param>
        /// <returns></returns>
        int GetObjectCount(string whereHql);

        /// <summary>
        /// 执行Hql
        /// </summary>
        /// <param name="hql"></param>
        void ExecuteByHql(string hql);

        /// <summary>
        /// 执行Hql，带一个参数
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameter"></param>
        void ExecuteByHql(string hql, object parameter);

        /// <summary>
        /// 执行Hql，带多个参数
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameters"></param>
        void ExecuteByHql(string hql, object[] parameters);

        /// <summary>
        ///  执行Hql，带多个参数
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameters">Key为命名参数名称,Value-对应的参数</param>
        void ExecuteByHql(string hql, IList<KeyValuePair<string, object>> parameters);
    }
}
