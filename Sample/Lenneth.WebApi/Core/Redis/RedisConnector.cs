using Lenneth.WebApi.Core.Log;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace Lenneth.WebApi.Core.Redis
{
    /// <summary>
    /// Redis连接器
    /// </summary>
    internal class RedisConnector : IRedisConnector
    {
        #region 属性

        /// <summary>
        /// 连接缓存
        /// </summary>
        private readonly ConcurrentDictionary<string, ConnectionMultiplexer> _connectionCache = new ConcurrentDictionary<string, ConnectionMultiplexer>();

        /// <summary>
        /// 线程锁
        /// </summary>
        private readonly object _locker = new object();

        /// <summary>
        /// 连接实例
        /// </summary>
        private ConnectionMultiplexer _instance;

        /// <summary>
        /// 自定义key标记
        /// </summary>
        private string CustomKey => Connection.Prefix;

        /// <summary>
        /// Redis连接
        /// </summary>
        private IRedisConnection Connection { get; set; }

        /// <summary>
        /// Redis连接日志
        /// </summary>
        private ILogWapper Logger => UnityConfig.Container.Resolve<ILogWapper>(Resources.AppResource.RedisLogName);

        #endregion 属性

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connection">缓存连接</param>
        public RedisConnector(IRedisConnection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// 获取单例实例
        /// </summary>
        private ConnectionMultiplexer Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_locker)
                {
                    if (_instance == null || !_instance.IsConnected)
                    {
                        _instance = GetManager();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 初始化连接器
        /// </summary>
        /// <param name="connection">缓存连接</param>
        public void InitRedisConnector(IRedisConnection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// 缓存获取
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private ConnectionMultiplexer GetConnectionMultiplexer(string connectionString)
        {
            if (!_connectionCache.ContainsKey(connectionString))
            {
                _connectionCache[connectionString] = GetManager(connectionString);
            }
            return _connectionCache[connectionString];
        }

        /// <summary>
        /// 连接管理器
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private ConnectionMultiplexer GetManager(string connectionString = null)
        {
            connectionString = connectionString ?? Connection.ConnectionString;
            try
            {
                var connect = ConnectionMultiplexer.Connect(connectionString);//注册如下事件
                connect.ConnectionFailed += MuxerConnectionFailed;
                connect.ConnectionRestored += MuxerConnectionRestored;
                connect.ErrorMessage += MuxerErrorMessage;
                connect.ConfigurationChanged += MuxerConfigurationChanged;
                connect.HashSlotMoved += MuxerHashSlotMoved;
                connect.InternalError += MuxerInternalError;
                return connect;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Redis连接发生错误");
                return null;
            }
        }

        #region 事件

        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            Logger.Info("Redis配置已更改： " + e.EndPoint);
        }

        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            Logger.Warn("Redis重新连接：Endpoint failed: " + e.EndPoint + ", " + e.FailureType);
            if (e.Exception != null)
            {
                Logger.Error(e.Exception, "连接失败！" + e.EndPoint);
            }
        }

        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            Logger.Info("Redis连接已成功重建: " + e.EndPoint);
        }

        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
            Logger.Error(e.Message, "Redis连接发生错误");
        }

        /// <summary>
        /// 更改集群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            Logger.Info("Redis集群已变更:NewEndPoint " + e.NewEndPoint + ", OldEndPoint" + e.OldEndPoint);
        }

        /// <summary>
        /// redis类库错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            Logger.Error(e.Exception, "Redis内部错误");
        }

        #endregion 事件

        #region RedisMethod

        #region String

        #region Sync

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public double StringDecrement(string key, double val = 1)
        {
            key = AddCustomKey(key);
            return Do(db => db.StringDecrement(key, val));
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public string StringGet(string key)
        {
            key = AddCustomKey(key);
            return Do(db => db.StringGet(key));
        }

        /// <summary>
        /// 获取多个Key
        /// </summary>
        /// <param name="keyList">Redis Key集合</param>
        /// <returns></returns>
        public IEnumerable<string> StringGet(IEnumerable<string> keyList)
        {
            var newKeys = keyList.Select(AddCustomKey).ToList();
            return Do(db => db.StringGet(ConvertRedisKeys(newKeys).ToArray())).Select(p => p.ToString()).ToList();
        }

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T StringGet<T>(string key)
        {
            key = AddCustomKey(key);
            return Do(db => ConvertObj<T>(db.StringGet(key).ToString()));
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public double StringIncrement(string key, double val = 1)
        {
            key = AddCustomKey(key);
            return Do(db => db.StringIncrement(key, val));
        }

        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool StringSet(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            key = AddCustomKey(key);
            return Do(db => db.StringSet(key, value, expiry));
        }

        /// <summary>
        /// 保存多个key value
        /// </summary>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public bool StringSet(IEnumerable<KeyValuePair<string, string>> keyValues)
        {
            var array = keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(AddCustomKey(p.Key), p.Value)).ToArray();
            return Do(db => db.StringSet(array));
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool StringSet<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            key = AddCustomKey(key);
            var json = ConvertJson(obj);
            return Do(db => db.StringSet(key, json, expiry));
        }

        #endregion Sync

        #region Async

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public async Task<double> StringDecrementAsync(string key, double val = 1)
        {
            key = AddCustomKey(key);
            return await Do(db => db.StringDecrementAsync(key, val));
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public async Task<string> StringGetAsync(string key)
        {
            key = AddCustomKey(key);
            return await Do(db => db.StringGetAsync(key));
        }

        /// <summary>
        /// 获取多个Key的值
        /// </summary>
        /// <param name="keyList">Redis Key集合</param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> StringGetAsync(List<string> keyList)
        {
            var newKeys = keyList.Select(AddCustomKey).ToList();
            var result = await Do(db => db.StringGetAsync(ConvertRedisKeys(newKeys).ToArray()));
            return result.Select(p => p.ToString()).ToList();
        }

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> StringGetAsync<T>(string key)
        {
            key = AddCustomKey(key);
            string result = await Do(db => db.StringGetAsync(key));
            return ConvertObj<T>(result);
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public async Task<double> StringIncrementAsync(string key, double val = 1)
        {
            key = AddCustomKey(key);
            return await Do(db => db.StringIncrementAsync(key, val));
        }

        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            key = AddCustomKey(key);
            return await Do(db => db.StringSetAsync(key, value, expiry));
        }

        /// <summary>
        /// 保存多个key value
        /// </summary>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync(IEnumerable<KeyValuePair<string, string>> keyValues)
        {
            var array = keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(AddCustomKey(p.Key), p.Value)).ToArray();
            return await Do(db => db.StringSetAsync(array));
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            key = AddCustomKey(key);
            var json = ConvertJson(key);
            return await Do(db => db.StringSetAsync(key, json, expiry));
        }

        #endregion Async

        #endregion String

        #region Hash

        #region Sync

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public double HashDecrement(string key, string dataKey, double val = 1)
        {
            key = AddCustomKey(key);
            return Do(db => db.HashDecrement(key, dataKey, val));
        }

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public bool HashDelete(string key, string dataKey)
        {
            key = AddCustomKey(key);
            return Do(db => db.HashDelete(key, dataKey));
        }

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        public long HashDelete(string key, IEnumerable<string> dataKeys)
        {
            key = AddCustomKey(key);
            return Do(db => db.HashDelete(key, dataKeys.Select(val => (RedisValue)val).ToArray()));
        }

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public bool HashExists(string key, string dataKey)
        {
            key = AddCustomKey(key);
            return Do(db => db.HashExists(key, dataKey));
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public T HashGet<T>(string key, string dataKey)
        {
            key = AddCustomKey(key);
            return Do(db =>
            {
                string value = db.HashGet(key, dataKey);
                return ConvertObj<T>(value);
            });
        }

        /// <summary>
        /// 从hash表获取string
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public string HashGetStr(string key, string dataKey)
        {
            key = AddCustomKey(key);
            return Do(db =>
            {
                string value = db.HashGet(key, dataKey);
                return value;
            });
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public double HashIncrement(string key, string dataKey, double val = 1)
        {
            key = AddCustomKey(key);
            return Do(db => db.HashIncrement(key, dataKey, val));
        }

        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<T> HashKeys<T>(string key)
        {
            key = AddCustomKey(key);
            return Do(db =>
            {
                var values = db.HashKeys(key);
                return ConvetList<T>(values.Select(p => p.ToString()));
            });
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool HashSet<T>(string key, string dataKey, T t)
        {
            key = AddCustomKey(key);
            return Do(db =>
            {
                var json = ConvertJson(t);
                return db.HashSet(key, dataKey, json);
            });
        }

        /// <summary>
        /// 存储string到hash
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool HashSetStr(string key, string dataKey, string t)
        {
            key = AddCustomKey(key);
            return Do(db => db.HashSet(key, dataKey, t));
        }

        #endregion Sync

        #region Async

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public async Task<double> HashDecrementAsync(string key, string dataKey, double val = 1)
        {
            key = AddCustomKey(key);
            return await Do(db => db.HashDecrementAsync(key, dataKey, val));
        }

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<bool> HashDeleteAsync(string key, string dataKey)
        {
            key = AddCustomKey(key);
            return await Do(db => db.HashDeleteAsync(key, dataKey));
        }

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        public async Task<long> HashDeleteAsync(string key, List<string> dataKeys)
        {
            key = AddCustomKey(key);
            return await Do(db => db.HashDeleteAsync(key, dataKeys.Select(val => (RedisValue)val).ToArray()));
        }

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<bool> HashExistsAsync(string key, string dataKey)
        {
            key = AddCustomKey(key);
            return await Do(db => db.HashExistsAsync(key, dataKey));
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<T> HashGeAsync<T>(string key, string dataKey)
        {
            key = AddCustomKey(key);
            string value = await Do(db => db.HashGetAsync(key, dataKey));
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public async Task<double> HashIncrementAsync(string key, string dataKey, double val = 1)
        {
            key = AddCustomKey(key);
            return await Do(db => db.HashIncrementAsync(key, dataKey, val));
        }

        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> HashKeysAsync<T>(string key)
        {
            key = AddCustomKey(key);
            var result = await Do(db => db.HashKeysAsync(key));
            return ConvetList<T>(result.Select(p => p.ToString()));
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<bool> HashSetAsync<T>(string key, string dataKey, T t)
        {
            key = AddCustomKey(key);
            return await Do(db =>
            {
                var json = ConvertJson(t);
                return db.HashSetAsync(key, dataKey, json);
            });
        }

        #endregion Async

        #endregion Hash

        #region List

        #region Sync

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T ListLeftPop<T>(string key)
        {
            key = AddCustomKey(key);
            return Do(db =>
            {
                var value = db.ListLeftPop(key);
                return ConvertObj<T>(value);
            });
        }

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void ListLeftPush<T>(string key, T value)
        {
            key = AddCustomKey(key);
            Do(db => db.ListLeftPush(key, ConvertJson(value)));
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long ListLength(string key)
        {
            key = AddCustomKey(key);
            return Do(redis => redis.ListLength(key));
        }

        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<T> ListRange<T>(string key)
        {
            key = AddCustomKey(key);
            return Do(redis =>
            {
                var values = redis.ListRange(key);
                return ConvetList<T>(values.Select(p => p.ToString()));
            });
        }

        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void ListRemove<T>(string key, T value)
        {
            key = AddCustomKey(key);
            Do(db => db.ListRemove(key, ConvertJson(value)));
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T ListRightPop<T>(string key)
        {
            key = AddCustomKey(key);
            return Do(db =>
            {
                var value = db.ListRightPop(key);
                return ConvertObj<T>(value);
            });
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void ListRightPush<T>(string key, T value)
        {
            key = AddCustomKey(key);
            Do(db => db.ListRightPush(key, ConvertJson(value)));
        }

        #endregion Sync

        #region Async

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> ListLeftPopAsync<T>(string key)
        {
            key = AddCustomKey(key);
            var value = await Do(db => db.ListLeftPopAsync(key));
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<long> ListLeftPushAsync<T>(string key, T value)
        {
            key = AddCustomKey(key);
            return await Do(db => db.ListLeftPushAsync(key, ConvertJson(value)));
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> ListLengthAsync(string key)
        {
            key = AddCustomKey(key);
            return await Do(redis => redis.ListLengthAsync(key));
        }

        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ListRangeAsync<T>(string key)
        {
            key = AddCustomKey(key);
            var values = await Do(redis => redis.ListRangeAsync(key));
            return ConvetList<T>(values.Select(p => p.ToString()));
        }

        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<long> ListRemoveAsync<T>(string key, T value)
        {
            key = AddCustomKey(key);
            return await Do(db => db.ListRemoveAsync(key, ConvertJson(value)));
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> ListRightPopAsync<T>(string key)
        {
            key = AddCustomKey(key);
            var value = await Do(db => db.ListRightPopAsync(key));
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<long> ListRightPushAsync<T>(string key, T value)
        {
            key = AddCustomKey(key);
            return await Do(db => db.ListRightPushAsync(key, ConvertJson(value)));
        }

        #endregion Async

        #endregion List

        #region SortSet

        #region Sync

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        public bool SortedSetAdd<T>(string key, T value, double score)
        {
            key = AddCustomKey(key);
            return Do(redis => redis.SortedSetAdd(key, ConvertJson(value), score));
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long SortedSetLength(string key)
        {
            key = AddCustomKey(key);
            return Do(redis => redis.SortedSetLength(key));
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<T> SortedSetRangeByRank<T>(string key)
        {
            key = AddCustomKey(key);
            return Do(redis =>
            {
                var values = redis.SortedSetRangeByRank(key);
                return ConvetList<T>(values.Select(p => p.ToString()));
            });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public bool SortedSetRemove<T>(string key, T value)
        {
            key = AddCustomKey(key);
            return Do(redis => redis.SortedSetRemove(key, ConvertJson(value)));
        }

        #endregion Sync

        #region Async

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        public async Task<bool> SortedSetAddAsync<T>(string key, T value, double score)
        {
            key = AddCustomKey(key);
            return await Do(redis => redis.SortedSetAddAsync(key, ConvertJson(value), score));
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> SortedSetLengthAsync(string key)
        {
            key = AddCustomKey(key);
            return await Do(redis => redis.SortedSetLengthAsync(key));
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> SortedSetRangeByRankAsync<T>(string key)
        {
            key = AddCustomKey(key);
            var values = await Do(redis => redis.SortedSetRangeByRankAsync(key));
            return ConvetList<T>(values.Select(p => p.ToString()));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<bool> SortedSetRemoveAsync<T>(string key, T value)
        {
            key = AddCustomKey(key);
            return await Do(redis => redis.SortedSetRemoveAsync(key, ConvertJson(value)));
        }

        #endregion Async

        #endregion SortSet

        #region Key

        /// <summary>
        /// 删除单个key
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns>是否删除成功</returns>
        public bool KeyDelete(string key)
        {
            key = AddCustomKey(key);
            return Do(db => db.KeyDelete(key));
        }

        /// <summary>
        /// 删除多个key
        /// </summary>
        /// <param name="keys">rediskey</param>
        /// <returns>成功删除的个数</returns>
        public long KeyDelete(IEnumerable<string> keys)
        {
            var newKeys = keys.Select(AddCustomKey).ToList();
            return Do(db => db.KeyDelete(ConvertRedisKeys(newKeys).ToArray()));
        }

        /// <summary>
        /// 判断key是否存储
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public bool KeyExists(string key)
        {
            key = AddCustomKey(key);
            return Do(db => db.KeyExists(key));
        }

        /// <summary>
        /// 设置Key的时间
        /// </summary>
        /// <param name="key">redis key</param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool KeyExpire(string key, TimeSpan? expiry = default(TimeSpan?))
        {
            key = AddCustomKey(key);
            return Do(db => db.KeyExpire(key, expiry));
        }

        /// <summary>
        /// 重新命名key
        /// </summary>
        /// <param name="key">就的redis key</param>
        /// <param name="newKey">新的redis key</param>
        /// <returns></returns>
        public bool KeyRename(string key, string newKey)
        {
            key = AddCustomKey(key);
            return Do(db => db.KeyRename(key, newKey));
        }

        #endregion Key

        #region Subscribe&Publish

        /// <summary>
        /// Redis发布订阅  发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private long Publish<T>(string channel, T msg)
        {
            var sub = Instance.GetSubscriber();
            return sub.Publish(channel, ConvertJson(msg));
        }

        /// <summary>
        /// Redis发布订阅  订阅
        /// </summary>
        /// <param name="subChannel"></param>
        /// <param name="handler"></param>
        private void Subscribe(string subChannel, Action<RedisChannel, RedisValue> handler = null)
        {
            var sub = Instance.GetSubscriber();
            sub.Subscribe(subChannel, (channel, message) =>
            {
                if (handler == null)
                {
                    Console.WriteLine(subChannel + "订阅收到消息：" + message);
                }
                else
                {
                    handler(channel, message);
                }
            });
        }

        /// <summary>
        /// Redis发布订阅  取消订阅
        /// </summary>
        /// <param name="channel"></param>
        private void Unsubscribe(string channel)
        {
            var sub = Instance.GetSubscriber();
            sub.Unsubscribe(channel);
        }

        /// <summary>
        /// Redis发布订阅  取消全部订阅
        /// </summary>
        private void UnsubscribeAll()
        {
            var sub = Instance.GetSubscriber();
            sub.UnsubscribeAll();
        }

        #endregion Subscribe&Publish

        #endregion RedisMethod

        #region Other

        /// <summary>
        /// 建立事务
        /// </summary>
        /// <returns>ITransaction</returns>
        private ITransaction CreateTransaction => Database.CreateTransaction();

        /// <summary>
        /// 获取缓存集合实例
        /// </summary>
        private IDatabase Database => Instance.GetDatabase((int)Connection.GetCollection);

        /// <summary>
        /// 获取缓存服务器实例
        /// </summary>
        /// <param name="hostAndPort">地址和端口字符串</param>
        /// <returns>缓存服务器实例</returns>
        private IServer GetServer(string hostAndPort) => Instance.GetServer(hostAndPort);

        #endregion Other

        #region Util

        /// <summary>
        /// 添加自定义键
        /// </summary>
        /// <param name="dataKey">数据键</param>
        /// <returns></returns>
        private string AddCustomKey(string dataKey) => string.IsNullOrWhiteSpace(CustomKey) ? dataKey : $@"{CustomKey}-{dataKey}";

        /// <summary>
        /// 对象转化为json字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="value">对象实体</param>
        /// <returns>转换结果</returns>
        private string ConvertJson<T>(T value) => value is string ? value.ToString() : JsonConvert.SerializeObject(value);

        /// <summary>
        /// json字符串转化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="value">字符串值</param>
        /// <returns></returns>
        private T ConvertObj<T>(string value) => JsonConvert.DeserializeObject<T>(value);

        /// <summary>
        /// 转换为RedisKey列表
        /// </summary>
        /// <param name="redisKeys">字符串列表</param>
        /// <returns>RedisKey列表</returns>
        private List<RedisKey> ConvertRedisKeys(IEnumerable<string> redisKeys) => redisKeys.Select(redisKey => (RedisKey)redisKey).ToList();

        /// <summary>
        /// 转化为对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        private List<T> ConvetList<T>(IEnumerable<string> values) => values.Select(ConvertObj<T>).ToList();

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="func">命令函数</param>
        /// <returns>直径结果</returns>
        private T Do<T>(Func<IDatabase, T> func) => func(Database);

        #endregion Util

        #region Dispose

        /// <inheritdoc />
        ~RedisConnector()
        {
            Dispose(false);
        }

        /// <summary>
        /// 释放connection
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放connection
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                _instance?.Dispose();
            }
        }

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        #endregion Dispose
    }
}