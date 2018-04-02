namespace Lenneth.Core.Framework.Redis
{
    /// <summary>
    /// 缓存连接接口
    /// </summary>
    interface IRedisConnection
    {
        /// <summary>
        /// 获取缓存服务器地址
        /// </summary>
        string Host { get; }
        /// <summary>
        /// 获取缓存服务器端口号
        /// </summary>
        int Port { get; }
        /// <summary>
        /// 获取缓存服务器访问用户名
        /// </summary>
        string Username { get; }
        /// <summary>
        /// 获取缓存服务器访问密码
        /// </summary>
        string Password { get; }
        /// <summary>
        /// 获取当前缓存集合
        /// </summary>
        string Collection { get; }
        /// <summary>
        /// 缓存前缀
        /// </summary>
        string Prefix { get; }
        /// <summary>
        /// 初始化连接
        /// </summary>
        /// <param name="host">服务器地址</param>
        /// <param name="port">端口号</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="collection">缓存集合</param>
        /// <param name="prefix">前缀</param>
        void InitConnection(string host, int port, string username = null, string password = null, string collection = null, string prefix = null);
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        string ConnectionString { get; }
        /// <summary>
        /// 获取符合当前环境的集合标识
        /// </summary>
        object GetCollection { get; }
    }
}