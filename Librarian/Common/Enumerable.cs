using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Librarian.Common
{
    public static class Enumerable
    {
        public static ObservableCollection<TSource> ToObservable<TSource>(this IEnumerable<TSource> source)
        {
            return new ObservableCollection<TSource>(source);
        }
    }
}
