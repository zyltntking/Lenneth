using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Lenneth.Core.Framework.FastDFS.Common;
using Lenneth.Core.Framework.FastDFS.Utility;

namespace Lenneth.Core.Framework.FastDFS
{
    public sealed class FastDfsWapper : IFastDfscs
    {
        /// <summary>
        /// 访问器
        /// </summary>
        private FastDfsClient Client { get; }
        /// <summary>
        /// 存储节点
        /// </summary>
        private StorageNode Node { get; }
        /// <summary>
        /// 加密器
        /// </summary>
        private IAes Cryptor { get; }

        public FastDfsWapper(FastDfsOpinions opinion)
        {
            var endPointsList = new List<IPEndPoint>();

            foreach (var e in opinion.EndPoints)
            {
                endPointsList.Add(e);
            }

            Client = new FastDfsClient(endPointsList);

            Node = Client.GetStorageNode(opinion.GroupName);

            Cryptor = new Aes(opinion.PassWord);
        }

        #region Implementation of IFastDfscs

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="content">二进制文件内容</param>
        /// <param name="fileExt">文件后缀名</param>
        /// <param name="groupName">集合名</param>
        /// <returns></returns>
        public string Put(byte[] content, string fileExt, string groupName = null)
        {
            try
            {
                var node = string.IsNullOrWhiteSpace(groupName) ? Node : Client.GetStorageNode(groupName);
                var cryptedContent = Cryptor.Encrypt(content);
                return Client.UploadFile(node, cryptedContent, fileExt);
            }
            catch (Exception e)
            {
                return $"Error 上传失败，原因：{e}";
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="groupName">集合名</param>
        /// <returns>上传文件名</returns>
        public string Put(string filepath, string groupName = null)
        {
            try
            {
                var extension = Path.GetExtension(filepath);
                var content = File.ReadAllBytes(filepath);
                return Put(content, extension, groupName);
            }
            catch (Exception e)
            {
                return $"Error 上传失败，原因：{e}";
            }
        }

        /// <summary>
        /// 批量上传
        /// </summary>
        /// <param name="filelist">文件列表</param>
        /// <param name="groupName">集合名</param>
        /// <returns>结果字典</returns>
        public Dictionary<string, string> PutMuti(IEnumerable<string> filelist, string groupName = null)
        {
            var result = new Dictionary<string, string>();
            foreach (var file in filelist)
            {
                var mc = new CallMutiPutWork(Put);
                var invoke = mc.BeginInvoke(file, groupName, null, null);
                var filename = mc.EndInvoke(invoke);
                result.Add(file, filename);
            }
            return result;
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="groupName">集合名</param>
        /// <returns>二进制内容</returns>
        public byte[] Get(string fileName, string groupName = null)
        {
            try
            {
                var node = string.IsNullOrWhiteSpace(groupName) ? Node : Client.GetStorageNode(groupName);
                var content = Client.DownloadFile(node, fileName);
                return Cryptor.Decrypt(content);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 批量获取
        /// </summary>
        /// <param name="filelist">文件列表</param>
        /// <param name="groupName">集合名</param>
        /// <returns></returns>
        public Dictionary<string, byte[]> GetMuti(IEnumerable<string> filelist, string groupName = null)
        {
            var result = new Dictionary<string, byte[]>();
            foreach (var file in filelist)
            {
                var mc = new CallMutiGetWork(Get);
                var invoke = mc.BeginInvoke(file, groupName, null, null);
                var content = mc.EndInvoke(invoke);
                result.Add(file, content);
            }
            return result;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="groupName">集合名</param>
        /// <returns></returns>
        public string DownLoad(string fileName, string targetDir = null, string groupName = null)
        {
            try
            {
                var content = Get(fileName, groupName);
                var name = Path.GetFileName(fileName);
                fileName = !string.IsNullOrWhiteSpace(targetDir) ? $"{targetDir}{name}" : $"{Directory.GetCurrentDirectory()}/{fileName}";
                var dir = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(dir))
                {
                    if (dir != null)
                        Directory.CreateDirectory(dir);
                }
                using (var bw = new BinaryWriter(File.Open(fileName, FileMode.OpenOrCreate)))
                {
                    bw.Write(content);
                }
                return fileName;
            }
            catch (Exception e)
            {
                return $"Error 下载失败，原因：{e}";
            }
        }

        /// <summary>
        /// 批量下载
        /// </summary>
        /// <param name="filelist">文件列表</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="groupName">集合名</param>
        /// <returns></returns>
        public Dictionary<string, string> DownLoadMuti(IEnumerable<string> filelist, string targetDir = null, string groupName = null)
        {
            var result = new Dictionary<string, string>();

            foreach (var file in filelist)
            {
                var mc = new CallMutiDownLoadWork(DownLoad);
                var invoke = mc.BeginInvoke(file, targetDir, groupName, null, null);
                var filename = mc.EndInvoke(invoke);
                result.Add(file, filename);
            }

            return result;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="groupName">集合名</param>
        /// <returns></returns>
        public void Delete(string fileName, string groupName = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(groupName))
                {
                    groupName = Node.GroupName;
                }
                Client.RemoveFile(groupName, fileName);
            }
            catch
            {
                // ignored
            }
        }

        #endregion Implementation of IFastDfscs

        #region Muti Utility

        /// <summary>
        /// 上传代理
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="groupName">集合名</param>
        /// <returns></returns>
        private delegate string CallMutiPutWork(string filePath, string groupName = null);

        /// <summary>
        /// 获取代理
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="groupName">集合名</param>
        /// <returns></returns>
        private delegate byte[] CallMutiGetWork(string fileName, string groupName = null);

        /// <summary>
        /// 下载代理
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="groupName">集合名</param>
        /// <returns></returns>
        private delegate string CallMutiDownLoadWork(string fileName, string targetDir = null, string groupName = null);

        #endregion Muti Utility
    }
}