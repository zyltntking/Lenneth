using System;

namespace Lenneth.Core.Framework.DynamicData.Experimental
{
    /// <summary>
    /// A specialisation of the SourceList which is optimised for watching individual items
    /// 
    /// 
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IWatcher<TObject, TKey> : IDisposable
    {
        /// <summary>
        /// Watches updates which match the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        IObservable<Change<TObject, TKey>> Watch(TKey key);
    }
}
