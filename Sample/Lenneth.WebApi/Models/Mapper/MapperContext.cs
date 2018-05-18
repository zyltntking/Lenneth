using System;
using AutoMapper.QueryableExtensions;
using System.Data.Entity;
using System.Linq;

namespace Lenneth.WebApi.Models.Mapper
{
    internal sealed class MapperContext: IDisposable
    {
        private DbContext Context { get; }

        public MapperContext(DbContext context)
        {
            Context = context;
        }

        public MapperContext()
        {
            Context = new BIBOEntities();
        }

        public IQueryable<T> GetMapper<T, TK>() where T : class where TK : class
        {
            return Context.Set<TK>().ProjectTo<T>();
        }

        #region Idisposable

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        private void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                Context?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MapperContext()
        {
            Dispose(false);
        }

        #endregion

    }
}