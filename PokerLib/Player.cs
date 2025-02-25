using PokerLib.Entity;
using PokerLib.Extensions;

namespace PokerLib
{
    public interface IPlayer<T>
    {
        void DrawOwnCards(IEnumerable<T> cardPool, int n);

        void DrawCards(IEnumerable<T> cardPool, int n);

        void DeleteCards(IEnumerable<T> deleteIndices);

        Hand JudgeOwnCards();
    }

    public class Player : IPlayer<int>
    {
        public List<Card> OwnCards { get; set; }

        public Player()
        {
            OwnCards = new List<Card>();
        }

        public void DrawOwnCards(IEnumerable<int> cardPool, int n)
        {
            OwnCards = Generate.GetRandomCards(cardPool, n).SortCards().ToList();
        }

        public void DrawCards(IEnumerable<int> cardPool, int n)
        {
            OwnCards.AddRange(Generate.GetRandomCards(cardPool, n));
            OwnCards = OwnCards.SortCards().ToList();
        }

        public void DeleteCards(IEnumerable<int> deleteIndices)
        {
            OwnCards = OwnCards.Select((value, index) => (index, value)).Where(x => !deleteIndices.Contains(x.index)).Select(x => x.value).SortCards().ToList();
        }

        public void DeleteCards(IEnumerable<Card> deleteCards)
        {
            OwnCards = OwnCards.Where(c => !deleteCards.Contains(c)).ToList();
        }

        public Hand JudgeOwnCards()
        {
            return Judge.JudgeHand(OwnCards);
        }
    }

    public class PlayerBasedCard : IPlayer<Card>
    {
        public List<Card> OwnCards { get; set; }

        public PlayerBasedCard()
        {
            OwnCards = new List<Card>();
        }

        public void DrawOwnCards(IEnumerable<Card> cardPool, int n)
        {
            OwnCards = Generate.GetRandomCards(cardPool, n).SortCards().ToList();
        }

        public void DrawCards(IEnumerable<Card> cardPool, int n)
        {
            OwnCards.AddRange(Generate.GetRandomCards(cardPool, n));
            OwnCards = OwnCards.SortCards().ToList();
        }

        public void DeleteCards(IEnumerable<Card> deleteIndices)
        {
            var indices = deleteIndices.ToList();
            OwnCards = OwnCards.Where(x => indices.Contains(x)).SortCards().ToList();
        }

        public Hand JudgeOwnCards()
        {
            return Judge.JudgeHand(OwnCards);
        }
    }
}
