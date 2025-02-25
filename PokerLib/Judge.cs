using PokerLib.Entity;
using PokerLib.Extensions;
using System.Globalization;

namespace PokerLib
{
    public class Judge
    {
        /// <summary>
        /// カードの役の判定
        /// </summary>
        /// <param name="cards">検査対象のカード</param>
        /// <returns>渡されたカードの役</returns>
        public static Hand JudgeHand(IEnumerable<Card> cards)
        {
            var sorted = cards.SortCards();

            if (HasStraightFlush(sorted))
            {
                return Hand.StraightFlush;
            }
            if (HasFourOfAKind(sorted))
            {
                return Hand.FourOfAKind;
            }
            if (HasFullHouse(sorted))
            {
                return Hand.FullHouse;
            }
            if (HasFlush(sorted))
            {
                return Hand.Flush;
            }
            if (HasStraight(sorted))
            {
                return Hand.Straight;
            }
            if (HasThreeOfAKind(sorted))
            {
                return Hand.ThreeOfAKind;
            }
            if (HasTwoPair(sorted))
            {
                return Hand.TwoPair;
            }
            if (HasOnePair(sorted))
            {
                return Hand.OnePair;
            }

            return Hand.None;
        }

        /// <summary>
        /// カードになにかの番号のカードがn枚ちょうど含まれているかの判定
        /// </summary>
        /// <param name="cards">検査対象のカード</param>
        /// <param name="n">枚数</param>
        /// <returns>なにかの数字がn枚含まれるか</returns>
        public static bool AreNCardsWithAnyNumber(IEnumerable<Card> cards, int n)
        {
            foreach (var cardsAnyNumber in cards.GroupBy(c => c.Number))
            {
                if (cardsAnyNumber.Count() == n)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// カードになにかの番号のカードがn枚以上含まれているかの判定
        /// </summary>
        /// <param name="cards">検査対象のカード</param>
        /// <param name="n">枚数</param>
        /// <returns>なにかの数字がn枚含まれるか</returns>
        public static bool AreNOrMoreCardsWithAnyNumber(IEnumerable<Card> cards, int n)
        {
            foreach (var cardsAnyNumber in cards.GroupBy(c => c.Number))
            {
                if (cardsAnyNumber.Count() >= n)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// カードがワンペアになり得るかの判定
        /// </summary>
        /// <param name="cards">検査対象のカード</param>
        /// <returns>カードがワンペアになり得るか</returns>
        public static bool HasOnePair(IEnumerable<Card> cards)
        {
            if (cards.Count() < Define.DrawPokerHands)
            {
                return false;
            }

            return AreNCardsWithAnyNumber(cards, 2);
        }

        /// <summary>
        /// カードがツーペアになり得るかの判定
        /// </summary>
        /// <param name="cards">検査対象のカード</param>
        /// <returns>カードがツーペアになり得るか</returns>
        public static bool HasTwoPair(IEnumerable<Card> cards)
        {
            if (cards.Count() < Define.DrawPokerHands)
            {
                return false;
            }

            var onePairCount = 0;
            foreach (var cardsAnyNumber in cards.GroupBy(c => c.Number))
            {
                if (cardsAnyNumber.Count() == 2)
                {
                    onePairCount++;
                }
            }

            if (onePairCount == 2)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// カードがスリーカードになり得るかの判定
        /// </summary>
        /// <param name="cards">検査対象のカード</param>
        /// <returns>カードがスリーカードになり得るか</returns>
        public static bool HasThreeOfAKind(IEnumerable<Card> cards)
        {
            if (cards.Count() < Define.DrawPokerHands)
            {
                return false;
            }

            return AreNCardsWithAnyNumber(cards, 3);
        }

        /// <summary>
        /// カードがフォーカードになり得るかの判定
        /// </summary>
        /// <param name="cards">検査対象のカード</param>
        /// <returns>カードがフォーカードになり得るか</returns>
        public static bool HasFourOfAKind(IEnumerable<Card> cards)
        {
            if (cards.Count() < Define.DrawPokerHands)
            {
                return false;
            }

            return AreNCardsWithAnyNumber(cards, 4);
        }

        /// <summary>
        /// カードがフラッシュになり得るかの判定
        /// </summary>
        /// <param name="cards">検査対象のカード</param>
        /// <returns>カードがフラッシュになり得るか</returns>
        public static bool HasFlush(IEnumerable<Card> cards)
        {
            if (cards.Count() < Define.DrawPokerHands)
            {
                return false;
            }

            var clubsCount = cards.Where(c => c.Suit == Suit.Clubs).Count();
            var heartsCount = cards.Where(c => c.Suit == Suit.Hearts).Count();
            var spadesCount = cards.Where(c => c.Suit == Suit.Spades).Count();
            var diamondsCount = cards.Where(c => c.Suit == Suit.Diamonds).Count();

            if (clubsCount >= 5
                || heartsCount >= 5
                || spadesCount >= 5
                || diamondsCount >= 5)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// カードがストレートになり得るかの判定
        /// </summary>
        /// <param name="cards">検査対象のカード</param>
        /// <returns>カードがストレートになり得るか</returns>
        public static bool HasStraight(IEnumerable<Card> cards)
        {
            if (cards.Count() != Define.DrawPokerHands)
            {
                return false;
            }

            var numberCount = new int[Define.MaxNumber + 1];
            var straightCount = new int[11];

            // 番号の出現数を数える
            foreach (var card in cards)
            {
                numberCount[card.Number]++;
            }

            for (int i = 1; i < 11; i++)
            {
                if (numberCount[i] != 0)
                {
                    for (int j = 0; j < Define.DrawPokerHands; j++)
                    {
                        // i + j == 14のとき、Aを表すので1に修正
                        var index = i + j > Define.MaxNumber ? 1 : i + j;
                        if (numberCount[index] != 0)
                        {
                            straightCount[i]++;
                        }
                    }

                    if (straightCount[i] >= Define.DrawPokerHands)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// カードがフルハウスになり得るかの判定
        /// </summary>
        /// <param name="cards">検査対象のカード</param>
        /// <returns>カードがフルハウスになり得るか</returns>
        public static bool HasFullHouse(IEnumerable<Card> cards)
        {
            if (cards.Count() < Define.DrawPokerHands)
            {
                return false;
            }

            return AreNCardsWithAnyNumber(cards, 3) && AreNCardsWithAnyNumber(cards, 2);
        }

        /// <summary>
        /// カードがストレートフラッシュになり得るかの判定
        /// </summary>
        /// <param name="cards">検査対象のカード</param>
        /// <returns>カードがストレートフラッシュになり得るか</returns>
        public static bool HasStraightFlush(IEnumerable<Card> cards)
        {
            if (cards.Count() < Define.DrawPokerHands)
            {
                return false;
            }

            return HasFullHouse(cards) && HasStraight(cards);
        }

        #region カードからある組み合わせのカードの一覧を取得する関数群
        /// <summary>
        /// カードからフラッシュの組み合わせの一覧を取得
        /// </summary>
        /// <param name="cards">検査対象のカード</param>
        /// <returns>フラッシュになる組み合わせのカードのリスト</returns>
        public static List<List<Card>>? GetFlushHands(IEnumerable<Card> cards)
        {
            if (!HasFlush(cards))
            {
                return null;
            }

            var flushSuit = cards.Where(c => c.Suit == Suit.Clubs).Count() >= 5 ? Suit.Clubs
                : cards.Where(c => c.Suit == Suit.Hearts).Count() >= 5 ? Suit.Hearts
                : cards.Where(c => c.Suit == Suit.Hearts).Count() >= 5 ? Suit.Spades
                : Suit.Diamonds;

            return cards.Where(c => c.Suit == flushSuit).OrderBy(c => c.Number).ToList().Combination(Define.NumHand).ToList();
        }

        /// <summary>
        /// ストレートになる部分数字のカードから組み合わせの列挙
        /// </summary>
        /// <param name="partialNumbers">ストレートになる部分数字のカード</param>
        /// <returns>ストレートの組み合わせのリスト</returns>
        public static List<List<Card>>? ListUpStraightHands(IEnumerable<IGrouping<uint, Card>> partialNumbers)
        {
            partialNumbers = partialNumbers.OrderBy(c => c.First().Number).ThenBy(c => c.First().Suit);
            var setCount = partialNumbers.Select(x => x.Count()).Aggregate((now, next) => now * next);
            var resultList = new List<List<Card>>();

            for (int i = 0; i < setCount; i++)
            {
                var handsArray = new Card[Define.NumHand];
                for (var j = 0; j < Define.NumHand; j++)
                {
                    var anyNumberList = partialNumbers.Skip(j).Take(1).First().ToList();
                    handsArray[j] = anyNumberList[i % anyNumberList.Count];
                }
                resultList.Add(handsArray.ToList());
            }

            return resultList.Count > 0 ? resultList : null;
        }

        /// <summary>
        /// カードからストレートの組み合わせの一覧を取得
        /// </summary>
        /// <param name="cards">検査対象のカード</param>
        /// <returns>ストレートになる組み合わせのカードのリスト</returns>
        public static List<List<Card>>? GetStraightHands(IEnumerable<Card> cards)
        {
            var cardsGroupByNuber = cards.GroupBy(c => c.Number);
            if (cardsGroupByNuber == null || cardsGroupByNuber.Count() < 5)
            {
                return null;
            }

            var loopCount = cardsGroupByNuber.Count() - Define.NumHand + 1;
            var straightHandsList = new List<List<Card>>();
            for (int i = 0; i < loopCount; i++)
            {
                var partialNumbers = cardsGroupByNuber.Skip(i).Take(Define.NumHand);
                var partialCards = partialNumbers.Select(c => c.First()).ToList();

                var isStraight = partialCards[1].Number - partialCards[0].Number == 1
                    && partialCards[2].Number - partialCards[1].Number == 1
                    && partialCards[3].Number - partialCards[2].Number == 1
                    && partialCards[4].Number - partialCards[3].Number == 1;

                if (isStraight)
                {
                    var straightPartialList = ListUpStraightHands(partialNumbers);

                    if (straightPartialList != null)
                    {
                        straightHandsList.AddRange(straightPartialList);
                    }
                }
            }

            var cardsGroupLast4 = cardsGroupByNuber.Skip(cardsGroupByNuber.Count() - 4).Take(4).ToList();

            if (cardsGroupByNuber.First().First().Number == 1
                && cardsGroupLast4[0].First().Number == 10
                && cardsGroupLast4[1].First().Number == 11
                && cardsGroupLast4[2].First().Number == 12
                && cardsGroupLast4[3].First().Number == 13)
            {
                var partialNumbers = new List<IGrouping<uint, Card>>
                {
                    cardsGroupByNuber.First(),
                    cardsGroupLast4[0],
                    cardsGroupLast4[1],
                    cardsGroupLast4[2],
                    cardsGroupLast4[3],
                };

                var straightPartialList = ListUpStraightHands(partialNumbers);

                if (straightPartialList != null)
                {
                    straightHandsList.AddRange(straightPartialList);
                }
            }

            return straightHandsList.Count > 0 ? straightHandsList : null;
        }

        /// <summary>
        /// カードからストレートフラッシュの組み合わせの一覧を取得
        /// </summary>
        /// <param name="cards">検査対象のカード</param>
        /// <returns>ストレートフラッシュになる組み合わせのカードのリスト</returns>
        public static List<List<Card>>? GetStraightFlushHands(IEnumerable<Card> cards)
        {
            var flushHandsList = GetFlushHands(cards);
            if (flushHandsList == null)
            {
                return null;
            }

            var straightFlushHandsList = new List<List<Card>>();

            foreach (var flushHands in flushHandsList)
            {
                var straightHands = GetStraightHands(flushHands);

                if (straightHands != null)
                {
                    straightFlushHandsList.AddRange(straightHands);
                }
            }

            return straightFlushHandsList.Count > 0 ? straightFlushHandsList : null;
        }
        #endregion

        public static bool IsDrawOfThreeOfAKind(IEnumerable<Card> cards)
        {
            if (cards.Count() < Define.DrawPokerHands)
            {
                return false;
            }

            return AreNCardsWithAnyNumber(cards, 2);
        }

        public static bool IsDrawOfFourOfAKind(IEnumerable<Card> cards)
        {
            if (cards.Count() < Define.DrawPokerHands)
            {
                return false;
            }

            return AreNCardsWithAnyNumber(cards, 3);
        }

        public static bool IsDrawOfStraight(IEnumerable<Card> cards, bool limited5 = true)
        {
            if (cards.Count() < Define.DrawPokerHands)
            {
                return false;
            }

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

                if (straightCount[i] >= Define.Draw.StraightCards
                    && (!limited5
                    || limited5 && straightCount[i] < Define.DrawPokerHands))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsDrawOfFlush(IEnumerable<Card> cards)
        {
            if (cards.Count() < Define.DrawPokerHands && !HasFlush(cards))
            {
                return false;
            }

            var clubsCount = cards.Where(c => c.Suit == Suit.Clubs).Count();
            var heartsCount = cards.Where(c => c.Suit == Suit.Hearts).Count();
            var spadesCount = cards.Where(c => c.Suit == Suit.Spades).Count();
            var diamondsCount = cards.Where(c => c.Suit == Suit.Diamonds).Count();

            if (clubsCount >= Define.Draw.FlushCards
                || heartsCount >= Define.Draw.FlushCards
                || spadesCount >= Define.Draw.FlushCards
                || diamondsCount >= Define.Draw.FlushCards)
            {
                return true;
            }

            return false;
        }

        public static bool IsDrawOfFullHouse(IEnumerable<Card> cards)
        {
            if (cards.Count() < Define.DrawPokerHands)
            {
                return false;
            }

            var onePairCount = 0;
            foreach (var cardsAnyNumber in cards.GroupBy(c => c.Number))
            {
                if (cardsAnyNumber.Count() == 2)
                {
                    onePairCount++;
                }
            }

            if (onePairCount >= 2)
            {
                return true;
            }

            return AreNCardsWithAnyNumber(cards, 3);
        }

        /// <summary>
        /// 最も強いハンドを返す
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static IEnumerable<Card> GetStrongestHand(IEnumerable<Card> cards)
        {
            var sorted = cards.SortCards();

            if (HasStraightFlush(sorted))
            {
                var flushCards = GetFlushHands(sorted) ?? new List<List<Card>>();
                foreach (var fc in flushCards)
                {
                    if (HasStraight(fc))
                    {
                        var straightCards = GetFiveConsecutiveNumberCards(fc.SortCards());
                        if (straightCards != null && straightCards.Count() == Define.NumHand)
                        {
                            return straightCards;
                        }
                    }
                }
            }
            if (HasFourOfAKind(sorted))
            {
                var comboList = GetListOfComboWithNCards(sorted, 4);
                if (comboList != null && comboList.Any())
                {
                    var hands = comboList.Last();
                    var comboNumber = hands.Last().Number;
                    return hands.Append(sorted.Where(c => c.Number != comboNumber).Last());
                }
            }
            if (HasFullHouse(sorted))
            {
                var threeComboList = GetListOfComboWithNCards(sorted, 3);
                var twoComboList = GetListOfComboWithNCards(sorted, 2);
                if (threeComboList != null && threeComboList.Any())
                {
                    if (twoComboList != null && twoComboList.Any())
                    {
                        return threeComboList.Last().Concat(twoComboList.Last());
                    }
                    else
                    {
                        return threeComboList.Last().Concat(threeComboList.SkipLast(1).Last().Take(2));
                    }
                }
            }
            if (HasFlush(sorted))
            {
                var flushCards = GetFlushHands(sorted);
                if (flushCards != null && flushCards.Any())
                {
                    return flushCards.Last().TakeLast(Define.NumHand);
                }
            }
            if (HasStraight(sorted))
            {
                var straightCards = GetFiveConsecutiveNumberCards(sorted);
                if (straightCards != null && straightCards.Count() == Define.NumHand)
                {
                    return straightCards;
                }
            }
            if (HasThreeOfAKind(sorted))
            {
                var comboList = GetListOfComboWithNCards(sorted, 3);
                if (comboList != null && comboList.Any())
                {
                    var hands = comboList.Last();
                    var comboNumber = hands.Last().Number;
                    return hands.Concat(sorted.Where(c => c.Number != comboNumber).TakeLast(2));
                }
            }
            if (HasTwoPair(sorted))
            {
                var comboList = GetListOfComboWithNCards(sorted, 2);
                if (comboList != null && comboList.Count() >= 2)
                {
                    var hands = comboList.TakeLast(2).SelectMany(c => c);
                    var comboNumberList = hands.GroupBy(c => c.Number).Select(g => g.First().Number);;
                    return hands.Append(sorted.Where(c => !comboNumberList.Contains(c.Number)).Last());
                }
            }
            if (HasOnePair(sorted))
            {
                var comboList = GetListOfComboWithNCards(sorted, 2);
                if (comboList != null && comboList.Count() == 1)
                {
                    var hands = comboList.Last();
                    var comboNumber = hands.Last().Number;
                    return hands.Concat(sorted.Where(c => c.Number != comboNumber).TakeLast(3));
                }
            }

            return cards.Count() >= Define.NumHand ? sorted.TakeLast(5) : sorted;
        }

        /// <summary>
        /// ある番号がn枚あるとき、そのn枚ある番号ごとのすべてのカードのリスト
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<Card>> GetListOfComboWithNCards(IEnumerable<Card> cards, int n)
        {
            return cards.GroupBy(c => c.Number).Where(g => g.Count() == n).Select(g => g.Select(c => c));
        }

        /// <summary>
        /// 連続する5つの数字で、最大の役となるものを1つだけ返す
        /// </summary>
        /// <param name="sortedCards"></param>
        /// <returns>nullのとき、存在しなかったと判定される</returns>
        public static IEnumerable<Card>? GetFiveConsecutiveNumberCards(IEnumerable<Card> sortedCards)
        {
            var onlyNumbers = sortedCards.GroupBy(c => c.Number);
            if (onlyNumbers.Count() < Define.NumHand)
            {
                return null;
            }

            IEnumerable<(int start, int end)> startEndSet = Enumerable.Empty<(int start, int end)>();

            for (int i = 0; i <= onlyNumbers.Count() - Define.NumHand; i++)
            {
                var diff = onlyNumbers.Skip(i).First().First().Number - onlyNumbers.Skip(i + 4).First().First().Number;
                if (diff != 4)
                {
                    continue;
                }

                startEndSet = startEndSet.Append((i, i + 4));
            }

            if (!startEndSet.Any())
            {
                return null;
            }

            var retList = new List<Card>();
            for (int i = startEndSet.Last().start; i <= startEndSet.Last().end; i++)
            {
                retList.Add(sortedCards.Where(c => c.Number == i).First());
            }

            return retList;
        }

        /// <summary>
        /// ハンドを比べて、第1引数を基準に勝敗を取得する
        /// </summary>
        /// <param name="playerHand">勝敗の基準となるハンド</param>
        /// <param name="anotherHand">比べるハンド</param>
        /// <returns>第1引数の勝敗</returns>
        public static WinOrLose GetWinOrLose(Hand playerHand, Hand anotherHand)
        {
            return playerHand < anotherHand ? WinOrLose.Win
                : playerHand == anotherHand ? WinOrLose.Draw
                : WinOrLose.Lose;
        }
    }
}
