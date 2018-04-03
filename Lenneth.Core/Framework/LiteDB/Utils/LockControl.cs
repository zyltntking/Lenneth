using System;

namespace Lenneth.Core.Framework.LiteDB
{
    /// <summary>
    /// A class to control locking disposal. Can be a "new lock" - when a lock is not not coming from other lock state
    /// </summary>
    public class LockControl : IDisposable
    {
        private Action _dispose;

        /// <summary>
        /// Indicate that cache was clear becase has changes on file
        /// </summary>
        public bool Changed { get; private set; }

        internal LockControl(bool changed, Action dispose)
        {
            Changed = changed;
            _dispose = dispose;
        }

        //public void Dispose()
        //{
        //    if (_dispose != null) _dispose();
        //}

        #region IDisposable

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        protected virtual void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                _dispose?.Invoke();
            }
        }

        /// <summary>执行与释放或重置非托管资源关联的应用程序定义的任务。</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>在垃圾回收将某一对象回收前允许该对象尝试释放资源并执行其他清理操作。</summary>
        ~LockControl()
        {
            Dispose(false);
        }

        #endregion
    }
}