using System;
using System.Text.RegularExpressions;
using StackExchange.Redis;

namespace Lenneth.Core.Framework.Redis
{
    /// <summary>
    /// Redis连接
    /// </summary>
    class RedisConnection : IRedisConnection
    {
        #region 公共属性及访问器

        /// <summary>
        /// Redis服务器地址
        /// </summary>
        private string _host = "127.0.0.1";

        /// <summary>
        /// Redis服务器端口号
        /// </summary>
        private int _port = 6379;

        /// <summary>
        /// Redis服务器访问密码
        /// </summary>
        private string _password;

        /// <summary>
        /// 当前Redis服务器访问集合序列
        /// </summary>
        private int _dbNum = 0;

        /// <summary>
        /// 当前Redis服务器访问集合
        /// </summary>
        private string _collection = "0";

        /// <summary>
        /// 自定义key前缀
        /// </summary>
        private string _prefix;

        /// <summary>
        /// Redis服务器地址访问器
        /// </summary>
        public string Host { get => _host; private set => _host = value; }

        /// <summary>
        /// Redis服务器端口号访问器
        /// </summary>
        public int Port { get => _port; private set => _port = value; }

        /// <summary>
        /// Redis缓存服务器访问用户名
        /// </summary>
        public string Username { get => null; private set { } }

        /// <summary>
        /// Redis服务器访问密码访问器
        /// </summary>
        public string Password { get => _password; private set => _password = value; }

        /// <summary>
        /// 当前Redis服务器访问集合序列
        /// </summary>
        public int DbNum { get => _dbNum; set => _dbNum = value; }

        /// <summary>
        /// 当前Redis服务器访问集合
        /// </summary>
        public string Collection { get => _collection; private set => _collection = value; }

        /// <summary>
        /// 自定义Key前缀
        /// </summary>
        public string Prefix { get => _prefix; private set => _prefix = value; }

        #endregion 公共属性及访问器

        /// <summary>
        /// Redis连接配置
        /// </summary>
        private ConfigurationOptions Config => new ConfigurationOptions
        {
            //AbortOnConnectFail = true,
            //AllowAdmin = false,
            //ConnectRetry = 3,
            //ConnectTimeout = 5000,
            //ConfigCheckSeconds = 60,
            DefaultDatabase = DbNum,
            //KeepAlive = 60,
            //DefaultVersion = new Version(3, 2, 1),
            Password = Password,
            EndPoints =
            {
                { Host, Port },
            },
        };

        /// <summary>
        /// 获取Redis缓存序列
        /// </summary>
        public object GetCollection => DbNum;

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public string ConnectionString => Config.ToString(true);


        /// <summary>
        /// 初始化Redis服务器连接
        /// </summary>
        /// <param name="host">地址</param>
        /// <param name="port">端口号</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="collection">集合</param>
        /// <param name="prefix">自定义key前缀</param>
        public void InitConnection(string host, int port, string username = null, string password = null, string collection = "0", string prefix = null)
        {
            Host = host;
            Port = port;
            Username = username;
            Password = password;
            var rex = new Regex(@"^\d+$");
            Collection = rex.IsMatch(collection) ? collection : "0";
            DbNum = Convert.ToInt32(Collection);
            Prefix = prefix;
        }
    }
}