using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Memcached.ClientLibrary;

namespace Com.Panduo.Common
{
    /// <summary>
    /// Memcached工具类
    /// </summary>
    public class MemcachedHelper
    {
        private static readonly int HASH_CODE = 32;
        private readonly static MemcachedClient _mcc = new MemcachedClient();
        private readonly static MemcachedHelper _memcachedHelper = new MemcachedHelper();
        private readonly static SockIOPool _sockIOPool = null;

        static MemcachedHelper()
        {
            // 服务器列表和其权重
            string[] servers = ConfigurationManager.AppSettings["memcached.services"].Split(";").ToArray();
            int[] weights = { 3 };

            // 获取socke连接池的实例对象
            _sockIOPool = SockIOPool.GetInstance();

            // 设置服务器信息
            _sockIOPool.SetServers(servers);
            _sockIOPool.SetWeights(weights);

            // 设置初始连接数、最小和最大连接数以及最大处理时间
            _sockIOPool.InitConnections = 5;
            _sockIOPool.MinConnections = 5;
            _sockIOPool.MaxConnections = 250;
            _sockIOPool.MaxIdle = 1000 * 60 * 60 * 6;

            // 设置主线程的睡眠时间
            _sockIOPool.MaintenanceSleep = 30;

            // 设置TCP的参数，连接超时等
            _sockIOPool.Nagle = false;
            _sockIOPool.SocketTimeout = 3000;
            _sockIOPool.SocketConnectTimeout = 0;

            // 初始化连接池
            _sockIOPool.Initialize();

            // 压缩设置，超过指定大小（单位为K）的数据都会被压缩 
            _mcc.EnableCompression = true;
            _mcc.CompressionThreshold = 64 * 1024;
        }

        /// <summary>
        /// 私有构造函数--单例
        /// </summary>
        private MemcachedHelper()
        {
        }

        /// <summary>
        /// MemcachedHelper实例
        /// </summary>
        /// <returns></returns>
        public static MemcachedHelper Instance
        {
            get
            {
                return _memcachedHelper;
            }
        }

        /// <summary>
        /// MemcachedClient实例
        /// </summary>
        /// <returns></returns>
        public static MemcachedClient Client
        {
            get
            {
                return _mcc;
            }
        }

        /// <summary>
        /// 添加缓存数据 ，如果以及存在键对应的数据则添加失败
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>成功返回true,添加失败返回false</returns>
        public bool Add(string key, object value)
        {
            return Add(key, value, DateTime.Now.AddDays(30));
        }

        /// <summary>
        /// 添加缓存数据 ，如果以及存在键对应的数据则添加失败
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">失效时间</param>
        /// <returns>成功返回true,添加失败返回false</returns>
        public bool Add(string key, object value, DateTime expiry)
        {
            return _mcc.Add(key, value, expiry, HASH_CODE);
        }

        /// <summary>
        /// 替换缓存数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>成功返回true,添加失败返回false</returns>
        public bool Replace(string key, object value)
        {
            return Replace(key, value, DateTime.Now.AddDays(30));
        }

        /// <summary>
        /// 替换缓存数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">失效时间</param>
        /// <returns>成功返回true,添加失败返回false</returns>
        public bool Replace(string key, object value, DateTime expiry)
        {
            return _mcc.Replace(key, value, expiry, HASH_CODE);
        }

        /// <summary>
        /// 删除缓存数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>成功返回true,添加失败返回false</returns>
        public bool Delete(string key)
        {
            return _mcc.Delete(key, HASH_CODE, DateTime.Now);
        }

        /// <summary>
        /// 设置缓存数据,如果键对应的数据以及存在则替换，否则添加
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>成功返回true,添加失败返回false</returns>
        public bool Set(string key, object value)
        {
            return Set(key, value, DateTime.Now.AddDays(30), HASH_CODE);
        }

        /// <summary>
        /// 设置缓存数据,如果键对应的数据以及存在则替换，否则添加
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="hashCode">Hash值</param>
        /// <returns>成功返回true,添加失败返回false</returns>
        public bool Set(string key, object value, int hashCode)
        {
            return Set(key, value, hashCode);
        }

        /// <summary>
        /// 置缓存数据,如果键对应的数据以及存在则替换，否则添加
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">失效时间</param>
        /// <returns>成功返回true,添加失败返回false</returns>
        public bool Set(string key, object value, DateTime expiry)
        {
            return Set(key, value, expiry, HASH_CODE);
        }

        /// <summary>
        /// 置缓存数据,如果键对应的数据以及存在则替换，否则添加
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">失效时间</param>
        /// <param name="hashCode">HashCode</param>
        /// <returns>成功返回true,添加失败返回false</returns>
        public bool Set(string key, object value, DateTime expiry, int hashCode)
        {
            return _mcc.Set(key, value, expiry, hashCode);
        }

        /// <summary>
        /// 获取缓存数据 
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回数据,如果不存在返回Null</returns>
        public object Get(string key)
        {
            return Get(key, HASH_CODE);
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashCode">Hash值</param>
        /// <returns>返回数据,如果不存在返回Null</returns>
        public object Get(string key, int hashCode)
        {
            return _mcc.Get(key, hashCode);
        }

        /// <summary>
        /// 获取缓存数据, 
        /// </summary>
        /// <param name="key">键</param> 
        /// <returns>返回数据,如果不存在返回指定默认值</returns>
        public T Get<T>(string key)
        {
            return Get<T>(key, default(T));
        }

        /// <summary>
        /// 获取缓存数据, 
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回数据,如果不存在返回指定默认值</returns>
        public T Get<T>(string key, T defaultValue)
        {
            return Get<T>(key, default(T), HASH_CODE);
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="hashCode"></param>
        /// <returns>返回数据,如果不存在返回指定默认值</returns>
        public T Get<T>(string key, T defaultValue, int hashCode)
        {
            var obj = default(T);
            try
            {
                obj = (T)_mcc.Get(key, hashCode);
            }
            catch (Exception)
            {
                obj = defaultValue;
            }

            return obj;
        }

        /// <summary>
        /// 缓存中是否存在指定键的数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>如果存在指定键的缓存返回true，否则返回false</returns>
        public bool IsExists(string key)
        {
            return _mcc.KeyExists(key);
        }

        /// <summary>
        /// 刷新所有缓存
        /// </summary>
        /// <returns>成功返回true,添加失败返回false</returns>
        public bool FlushAll()
        {
            return _mcc.FlushAll();
        }

        /// <summary>
        /// 刷新指定缓存服务器的缓存
        /// </summary>
        /// <param name="servers">服务器地址和端口列表，eg:127.0.0.1:11211</param>
        /// <returns></returns>
        public bool FlushAll(string[] servers)
        {
            var arrayList = new ArrayList(servers.Count());
            arrayList.AddRange(servers);

            return _mcc.FlushAll(arrayList);
        }

        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <returns></returns>
        public Hashtable Stats()
        {
            return _mcc.Stats();
        }

        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <param name="servers"></param>
        /// <returns></returns>
        public Hashtable Stats(string[] servers)
        {
            var arrayList = new ArrayList(servers.Count());
            arrayList.AddRange(servers);

            return _mcc.Stats(arrayList);
        }

        /// <summary>
        /// 计数器加1
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回当前总数</returns>
        public long Increment(string key)
        {
            return Increment(key, 1L);
        }

        /// <summary>
        /// 计数器加指定数值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">要加的数值</param>
        /// <returns>返回当前总数</returns>
        public long Increment(string key, long value)
        {
            return Increment(key, value, HASH_CODE);
        }

        /// <summary>
        /// 计数器加指定数值,指定Hash值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">要加的数值</param>
        /// <param name="hashCode">Hash值</param>
        /// <returns>返回当前总数</returns>
        public long Increment(string key, long value, int hashCode)
        {
            return _mcc.Increment(key, value, hashCode);
        }

        /// <summary>
        /// 计数器减1
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回当前总数</returns>
        public long Decrement(string key)
        {
            return Decrement(key, 1L);
        }

        /// <summary>
        /// 计数器减指定数值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">要加的数值</param>
        /// <returns>返回当前总数</returns>
        public long Decrement(string key, long value)
        {
            return Decrement(key, value, HASH_CODE);
        }

        /// <summary>
        /// 计数器减指定数值,指定Hash值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">要加的数值</param>
        /// <param name="hashCode">Hash值</param>
        /// <returns>返回当前总数</returns>
        public long Decrement(string key, long value, int hashCode)
        {
            return _mcc.Decrement(key, value, hashCode);
        }

        /// <summary>
        /// 获取区块
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, IList<string>> GetAllSlabIds()
        {
            var map = new Dictionary<string, IList<string>>();

            var services = _sockIOPool.Servers;
            foreach (string service in services)
            {
                var serviceArr = service.Split(':');

                var ipString = "127.0.0.1";
                var port = 11211;

                if (serviceArr.Length == 2)
                {
                    ipString = serviceArr[0];
                    port = serviceArr[1].ParseTo(11211);
                }
                else if (serviceArr.Length == 1)
                {
                    ipString = serviceArr[0];
                }

                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(new IPEndPoint(IPAddress.Parse(ipString), port));
                var slabIdIter = QuerySlabId(socket);
                var ids = slabIdIter.Select(c => Regex.Replace(c, @".*:(\d+):.*", "$1")).Distinct().ToList();

                socket.Close();

                map.Add(service, ids);
            }

            return map;
        }

        /// <summary>
        /// 获取服务器对应的缓存Key
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, IList<string>> GetAllCacheKeys()
        {
            var map = new Dictionary<string, IList<string>>();

            var services = _sockIOPool.Servers;
            foreach (string service in services)
            {
                var serviceArr = service.Split(':');

                var ipString = "127.0.0.1";
                var port = 11211;

                if (serviceArr.Length == 2)
                {
                    ipString = serviceArr[0];
                    port = serviceArr[1].ParseTo(11211);
                }
                else if (serviceArr.Length == 1)
                {
                    ipString = serviceArr[0];
                }

                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(new IPEndPoint(IPAddress.Parse(ipString), port));
                var slabIdIter = QuerySlabId(socket);
                var ids = slabIdIter.Select(c => Regex.Replace(c, @".*:(\d+):.*", "$1")).Distinct().ToList();

                var keyIter = QueryKeys(socket, ids);
                socket.Close();

                map.Add(service, keyIter.ToList());
            }

            return map;
        }


        #region 内部解析
        private const string END = "END"; // end of data from server
        private const string ERROR = "ERROR"; // invalid command name from client

        /// <summary>
        /// 执行返回字符串标量
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="command">命令</param>
        /// <returns>执行结果</returns>
        private static string ExecuteScalarAsString(Socket socket, String command)
        {
            var sendNumOfBytes = socket.Send(Encoding.UTF8.GetBytes(command));
            var bufferSize = 0x10000;
            var buffer = new Byte[bufferSize];
            var sb = new StringBuilder();

            while (true)
            {
                socket.Receive(buffer);
                var str = Encoding.UTF8.GetString(buffer);
                sb.Append(str);

                if (str.Contains(END) || str.EndsWith(END) || str.EndsWith(ERROR))
                {
                    break;
                }
            }


            return sb.ToString();
        }

        /// <summary>
        /// 查询slabId
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <returns>slabId遍历器</returns>
        private static IEnumerable<string> QuerySlabId(Socket socket)
        {
            var command = "stats items \r\n";//STAT items:0:number 0 \r\n
            var contentAsString = ExecuteScalarAsString(socket, command);

            return ParseStatsItems(contentAsString);
        }

        /// <summary>
        /// 解析STAT items返回slabId
        /// </summary>
        /// <param name="contentAsString">解析内容</param>
        /// <returns>slabId遍历器</returns>
        private static IEnumerable<string> ParseStatsItems(String contentAsString)
        {
            var slabIds = new List<String>();
            var separator = new[] { "\r\n" };
            var separator2 = new[] { ' ' };
            var items = contentAsString.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            for (Int32 i = 0; i < items.Length; i += 4)
            {
                var itemParts = items[i].Split(separator2, StringSplitOptions.RemoveEmptyEntries);

                if (itemParts.Length < 3)
                    continue;

                slabIds.Add(itemParts[1]);
            }

            return slabIds;
        }

        /// <summary>
        /// 查询键
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="slabIdIter">被查询slabId</param>
        /// <returns>键遍历器</returns>
        private static IEnumerable<string> QueryKeys(Socket socket, IEnumerable<string> slabIdIter)
        {
            var keys = new List<String>();
            //var cmdFmt = "stats cachedump {0} 200000 ITEM views.decorators.cache.cache_header..cc7d9 [6 b; 1256056128 s] \r\n";
            var cmdFmt = "stats cachedump {0} 0 \r\n";
            var contentAsString = String.Empty;

            foreach (string slabId in slabIdIter)
            {
                contentAsString = ExecuteScalarAsString(socket, string.Format(cmdFmt, slabId));
                keys.AddRange(ParseKeys(contentAsString));
            }

            return keys;
        }

        /// <summary>
        /// 解析stats cachedump返回键
        /// </summary>
        /// <param name="contentAsString">解析内容</param>
        /// <returns>键遍历器</returns>
        private static IEnumerable<string> ParseKeys(String contentAsString)
        {
            var keys = new List<String>();
            var separator = new[] { "\r\n" };
            var separator2 = new[] { ' ' };
            var prefix = "ITEM";
            var items = contentAsString.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in items)
            {
                var itemParts = item.Split(separator2, StringSplitOptions.RemoveEmptyEntries);

                if ((itemParts.Length < 3) || !String.Equals(itemParts.FirstOrDefault(), prefix, StringComparison.OrdinalIgnoreCase))
                    continue;

                keys.Add(itemParts[1]);
            }

            return keys;
        }
        #endregion
    }
}
