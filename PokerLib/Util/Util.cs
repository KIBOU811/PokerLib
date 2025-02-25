using PokerLib.Entity;
using static PokerLib.Judge;

namespace PokerLib.Util
{
    public class Util
    {
        public static IEnumerable<Card> GetSuggestedKeepCards(IEnumerable<Card> cards)
        {
            IEnumerable<Card> suggestCards;

            if (IsDrawOfFullHouse(cards))
            {
                var groupedCards = cards.GroupBy(c => c.Number).Select(x => (x.First().Number, NumberCards: x.ToList()));
                suggestCards = groupedCards.Where(x => x.NumberCards.Count >= 2).SelectMany(x => x.NumberCards);
            }
            else if (AreNOrMoreCardsWithAnyNumber(cards, 4))
            {
                var groupedCards = cards.GroupBy(c => c.Number).Select(x => (x.First().Number, NumberCards: x.ToList()));
                suggestCards = groupedCards.Where(x => x.NumberCards.Count >= 4).SelectMany(x => x.NumberCards);
            }
            else if (IsDrawOfFlush(cards) || HasFlush(cards))
            {
                var groupedCards = cards.GroupBy(c => c.Suit).Select(x => (x.First().Suit, SuitCards: x.ToList()));
                var maxSuitCounts = groupedCards.First().SuitCards.Count;
                suggestCards = groupedCards.Where(x => x.SuitCards.Count == maxSuitCounts).SelectMany(x => x.SuitCards);
            }
            else if (AreNOrMoreCardsWithAnyNumber(cards, 3))
            {
                var groupedCards = cards.GroupBy(c => c.Number).Select(x => (x.First().Number, NumberCards: x.ToList()));
                suggestCards = groupedCards.Where(x => x.NumberCards.Count >= 3).SelectMany(x => x.NumberCards);
            }
            else if (AreNOrMoreCardsWithAnyNumber(cards, 2))
            {
                var groupedCards = cards.GroupBy(c => c.Number).Select(x => (x.First().Number, NumberCards: x.ToList()));
                suggestCards = groupedCards.Where(x => x.NumberCards.Count >= 2).SelectMany(x => x.NumberCards);
            }
            else if (IsDrawOfStraight(cards, false))
            {
                var cardsPickedByNumber = cards.GroupBy(c => c.Number).Select(x => x.First());
                var numberCount = new int[Define.MaxNumber + 1];
                var straightCount = new int[11];

                // 番号の出現数を数える
                foreach (var card in cards)
                {
                    numberCount[card.Number]++;
                }

                for (int i = 1; i < 11; i++)
                {
                    if (numberCount[i] == 0)
                    {
                        continue;
                    }

                    for (int j = 0; j < Define.DrawPokerHands; j++)
                    {
                        // i + j == 14のとき、Aを表すので1に修正
                        var index = i + j > Define.MaxNumber ? 1 : i + j;
                        if (numberCount[index] != 0)
                        {
                            straightCount[i]++;
                        }
                    }
                }

                suggestCards = Enumerable.Empty<Card>();
                foreach (var (index, count) in straightCount.Skip(1).Select((value, index) => (index + 1, value)))
                {
                    if (count < Define.Draw.StraightCards)
                    {
                        continue;
                    }

                    for (int i = 0; i < Define.DrawPokerHands; i++)
                    {
                        // i + j == 14のとき、Aを表すので1に修正
                        var number = index + i > Define.MaxNumber ? 1 : index + i;
                        if (cardsPickedByNumber.Where(c => c.Number == number).Any())
                        {
                            suggestCards = suggestCards.Append(cardsPickedByNumber.Where(c => c.Number == number).First());
                        }
                    }
                }
                suggestCards = suggestCards.Distinct();
            }
            else
            {
                suggestCards = Enumerable.Empty<Card>();
            }

            return suggestCards;
        }
    }
}
