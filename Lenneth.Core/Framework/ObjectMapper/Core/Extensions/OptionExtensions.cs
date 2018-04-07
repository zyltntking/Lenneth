using System;
using Lenneth.Core.Framework.ObjectMapper.Core.DataStructures;

namespace Lenneth.Core.Framework.ObjectMapper.Core.Extensions
{
    /// <summary>
    ///     https://github.com/Nelibur/Nelibur
    /// </summary>
    internal static class OptionExtensions
    {
        public static Option<T> Do<T>(this Option<T> value, Action<T> action)
        {
            if (value.HasValue)
            {
                action(value.Value);
            }
            return value;
        }

        public static Option<T> DoOnEmpty<T>(this Option<T> value, Action action)
        {
            if (value.HasNoValue)
            {
                action();
            }
            return value;
        }

        public static Option<T> Finally<T>(this Option<T> value, Action<T> action)
        {
            action(value.Value);
            return value;
        }

        public static Option<TResult> Map<TInput, TResult>(this Option<TInput> value, Func<TInput, Option<TResult>> func)
        {
            if (value.HasNoValue)
            {
                return Option<TResult>.Empty;
            }
            return func(value.Value);
        }

        public static Option<TResult> Map<TInput, TResult>(this Option<TInput> value, Func<TInput, TResult> func)
        {
            if (value.HasNoValue)
            {
                return Option<TResult>.Empty;
            }
            return func(value.Value).ToOption();
        }

        public static Option<TResult> Map<TInput, TResult>(this Option<TInput> value, Func<TInput, bool> predicate, Func<TInput, TResult> func)
        {
            if (value.HasNoValue)
            {
                return Option<TResult>.Empty;
            }
            return !predicate(value.Value) ? Option<TResult>.Empty : func(value.Value).ToOption();
        }

        public static Option<T> MapOnEmpty<T>(this Option<T> value, Func<T> func)
        {
            return value.HasNoValue ? func().ToOption() : value;
        }

        public static Option<TV> SelectMany<T, TU, TV>(this Option<T> value, Func<T, Option<TU>> func, Func<T, TU, TV> selector)
        {
            return value.Map(x => func(x).Map(y => selector(x, y).ToOption()));
        }

        public static Option<T> Where<T>(this Option<T> value, Func<T, bool> predicate)
        {
            if (value.HasNoValue)
            {
                return Option<T>.Empty;
            }
            return predicate(value.Value) ? value : Option<T>.Empty;
        }
    }
}
