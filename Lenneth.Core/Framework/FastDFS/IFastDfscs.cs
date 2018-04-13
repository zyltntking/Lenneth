using System.Collections.Generic;

namespace Lenneth.Core.Framework.FastDFS
{
    public interface IFastDfscs
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="content">二进制文件内容</param>
        /// <param name="fileExt">文件后缀名</param>
        /// <param name="groupName">集合名</param>
        /// <returns>上传文件名</returns>
        string Put(byte[] content, string fileExt, string groupName = null);

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="groupName">集合名</param>
        /// <returns>上传文件名</returns>
        string Put(string filepath, string groupName = null);

        /// <summary>
        /// 批量上传
        /// </summary>
        /// <param name="filelist">文件列表</param>
        /// <param name="groupName">集合名</param>
        /// <returns>结果字典</returns>
        Dictionary<string, string> PutMuti(IEnumerable<string> filelist, string groupName = null);

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="groupName">集合名</param>
        /// <returns></returns>
        byte[] Get(string fileName, string groupName = null);

        /// <summary>
        /// 批量获取
        /// </summary>
        /// <param name="filelist">文件列表</param>
        /// <param name="groupName">集合名</param>
        /// <returns></returns>
        Dictionary<string, byte[]> GetMuti(IEnumerable<string> filelist, string groupName = null);

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="groupName">集合名</param>
        /// <returns></returns>
        string DownLoad(string fileName, string targetDir = null, string groupName = null);

        /// <summary>
        /// 批量下载
        /// </summary>
        /// <param name="filelist">文件列表</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="groupName">集合名</param>
        /// <returns></returns>
        Dictionary<string, string> DownLoadMuti(IEnumerable<string> filelist,string targetDir = null, string groupName = null);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="groupName">集合名</param>
        /// <returns></returns>
        void Delete(string fileName, string groupName = null);
    }
}