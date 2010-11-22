using System;
using System.Collections.Generic;
using System.Linq;

namespace LivingRoom
{
    public static class EnumerableExtensions
    {

        public static IEnumerable<TResult> Each<T, TResult>(
            this IEnumerable<T> enumerable, Func<T, TResult> action)
        {
            return enumerable.Select(action);
        }

        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
                action(item);
        }

    }
}
