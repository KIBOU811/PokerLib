// 参照元： https://baba-s.hatenablog.com/entry/2019/12/04/125000

namespace PokerLib.Extensions
{
    internal static class IListExtensions
    {
        public static IEnumerable<List<T>> Combination<T>(this IList<T> self, int n)
        {
            return Enumerable.Range(0, n - 1)
                .Aggregate(
                    Enumerable.Range(0, self.Count - n + 1)
                        .Select(num => new List<int> { num }),
                    (list, _) => list.SelectMany(
                        c =>
                            Enumerable.Range(c.Max() + 1, self.Count - c.Max() - 1)
                                .Select(num => new List<int>(c) { num })
                    )
                )
                .Select(
                    c => c
                        .Select(num => self[num])
                        .ToList()
                );
        }
    }
}
