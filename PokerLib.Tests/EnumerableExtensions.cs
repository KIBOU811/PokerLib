namespace PokerLib.Tests
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<(T1, T2)> Zip<T1, T2>(this IEnumerable<T1> t1, IEnumerable<T2> t2)
        {
            using var t1e = t1.GetEnumerator();
            using var t2e = t2.GetEnumerator();
            while (t1e.MoveNext() && t2e.MoveNext())
                yield return (t1e.Current, t2e.Current);
        }

        public static IEnumerable<(T1, T2, T3)> Zip<T1, T2, T3>(this IEnumerable<T1> t1, IEnumerable<T2> t2, IEnumerable<T3> t3)
        {
            using var t1e = t1.GetEnumerator();
            using var t2e = t2.GetEnumerator();
            using var t3e = t3.GetEnumerator();
            while (t1e.MoveNext() && t2e.MoveNext() && t3e.MoveNext())
                yield return (t1e.Current, t2e.Current, t3e.Current);
        }
    }
}
