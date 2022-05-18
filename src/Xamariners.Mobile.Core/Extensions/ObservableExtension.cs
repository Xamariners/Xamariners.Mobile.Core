using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Xamariners.Mobile.Core.Extensions
{
    public static class ObservableExtension
    {
        /// <summary>
        /// Convert IEnumerable(T) to observable collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            var collection = new ObservableCollection<T>();

            // Technically we can directly use constructor like --> new ObservableCollection<T>(source); which copy the items directly,
            // however, we add per item on purpose so the item property will be raised on every item added & will NOT effect performance.
            // Reference: https://referencesource.microsoft.com/#System/compmod/system/collections/objectmodel/observablecollection.cs,f63ea2601f5edbbb

            if (source != null)
            {
                var array = source as T[] ?? source.ToArray();
                if (array.Any())
                    foreach (var item in array)
                        collection.Add(item);
            }

            return collection;
        }
    }
}
