using System;
using System.Collections.Generic;

namespace ObjectViewer.Extensions
{
    internal static class Extensions
    {
        public static void ForAll<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action(item);
            }
        }
    }
}
