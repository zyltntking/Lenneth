using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Lenneth.Core.Framework.DynamicData.Binding
{
    /// <summary>
    /// Extensions to convert an observable collection into a dynamic stream
    /// </summary>
    public static class ObservableCollectionEx
    {
        /// <summary>
        /// Convert an observable collection into an observable change set
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public static IObservable<IChangeSet<T>> ToObservableChangeSet<T>(this ObservableCollection<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return ToObservableChangeSet<ObservableCollection<T>,T>(source);
        }


        /// <summary>
        /// Convert an observable collection into an observable change set
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">source
        /// or
        /// keySelector</exception>
        public static IObservable<IChangeSet<TObject, TKey>> ToObservableChangeSet<TObject, TKey>(this ObservableCollection<TObject> source, Func<TObject, TKey> keySelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            return ToObservableChangeSet<ObservableCollection<TObject>, TObject>(source).AddKey(keySelector);
        }

        /// <summary>
        /// Convert the readonly observable collection into an observable change set
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public static IObservable<IChangeSet<T>> ToObservableChangeSet<T>(this ReadOnlyObservableCollection<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return ToObservableChangeSet<ReadOnlyObservableCollection<T>, T>(source);
        }

        /// <summary>
        /// Convert the readonly observable collection into an observable change set
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">source
        /// or
        /// keySelector</exception>
        public static IObservable<IChangeSet<TObject, TKey>> ToObservableChangeSet<TObject, TKey>(this ReadOnlyObservableCollection<TObject> source, Func<TObject, TKey> keySelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            return ToObservableChangeSet<ReadOnlyObservableCollection<TObject>, TObject>(source).AddKey(keySelector);
        }

        /// <summary>
        /// Convert an observable collection into an observable change set
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <typeparam name="TCollection"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public static IObservable<IChangeSet<T>> ToObservableChangeSet<TCollection, T>(this TCollection source)
            where TCollection : INotifyCollectionChanged, IEnumerable<T>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return Observable.Create<IChangeSet<T>>(observer =>
            {
                var data = new ChangeAwareList<T>(source);

                if (data.Count > 0)
                    observer.OnNext(data.CaptureChanges());

                return source.ObserveCollectionChanges()
                    .Scan(data, (list, args) =>
                    {
                        var changes = args.EventArgs;

                        switch (changes.Action)
                        {
                            case NotifyCollectionChangedAction.Add:
                            {
                                if (changes.NewItems.Count == 1)
                                {
                                    list.Insert(changes.NewStartingIndex, (T) changes.NewItems[0]);
                                }
                                else
                                {
                                    list.InsertRange(changes.NewItems.Cast<T>(), changes.NewStartingIndex);
                                }
                                break;
                            }
                            case NotifyCollectionChangedAction.Remove:
                            {
                                if (changes.OldItems.Count == 1)
                                {
                                    list.RemoveAt(changes.OldStartingIndex);
                                }
                                else
                                {
                                    list.RemoveRange(changes.OldStartingIndex, changes.OldItems.Count);
                                }
                                break;
                            }
                            case NotifyCollectionChangedAction.Replace:
                            {
                                list[changes.NewStartingIndex] = (T) changes.NewItems[0];
                                break;
                            }
                            case NotifyCollectionChangedAction.Reset:
                            {
                                list.Clear();
                                list.AddRange(source);
                                break;
                            }

                            case NotifyCollectionChangedAction.Move:
                            {
                                list.Move(changes.OldStartingIndex, changes.NewStartingIndex);
                                break;
                            }
                        }
                        return list;
                    })
                    .Select(list => list.CaptureChanges())
                    .SubscribeSafe(observer);
            });
        }

        /// <summary>
        /// Observes notify collection changed args
        /// </summary>
        public static IObservable<EventPattern<NotifyCollectionChangedEventArgs>> ObserveCollectionChanges(this INotifyCollectionChanged source)
        {
            return Observable
                .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                    h => source.CollectionChanged += h,
                    h => source.CollectionChanged -= h);
        }
    }
}
