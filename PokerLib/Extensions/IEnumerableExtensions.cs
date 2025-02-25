using PokerLib.Entity;

namespace PokerLib.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> SortCards<T>(this IEnumerable<T> self) where T : Card
        {
            return self.OrderBy(c => c.Number).ThenBy(c => c.Suit);
        }
    }
}
