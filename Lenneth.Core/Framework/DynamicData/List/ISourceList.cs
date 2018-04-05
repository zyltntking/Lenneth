using System;

namespace Lenneth.Core.Framework.DynamicData
{
    /// <summary>
    /// An editable observable list, providing  observable methods
    /// as well as data access methods
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISourceList<T> : IObservableList<T>, ICollectionSubject
    {
        /// <summary>
        /// Edit the inner list within the list's internal locking mechanism
        /// </summary>
        /// <param name="updateAction">The update action.</param>
        void Edit(Action<IExtendedList<T>> updateAction);

    }
}
