using PokerLib.Entity;

namespace PokerLib
{
    /// <summary>
    /// カード生成に関するクラス
    /// </summary>
    public class Generate
    {
        /// <summary>
        /// カードを表す番号から、カードリストの生成
        /// </summary>
        /// <param name="cardNumbers">カードを表す番号</param>
        /// <returns>カードリスト</returns>
        /// <remarks>
        /// カードを表す番号は、0～51の数字で表される<br/>
        /// 0～12：クラブの1～13<br/>
        /// 13～25：ハートの1～13<br/>
        /// 26～38：スペードの1～13<br/>
        /// 39～51：ダイヤの1～13<br/>
        /// </remarks>
        public static List<Card> GenerateCardList(IEnumerable<int> cardNumbers)
        {
            var cardList = new List<Card>();

            foreach (var card in cardNumbers)
            {
                var suitNumber = (card - (card % 13)) / 13;
                var suit = suitNumber == 0 ? Suit.Clubs
                    : suitNumber == 1 ? Suit.Hearts
                    : suitNumber == 2 ? Suit.Spades
                    : Suit.Diamonds;

                cardList.Add(new Card(suit, (uint)card % 13 + 1));
            }

            return cardList;
        }

        /// <summary>
        /// カードから、カードを表す番号のリストを生成
        /// </summary>
        /// <param name="cards">カードリスト</param>
        /// <returns>カードを表す番号</returns>
        public static List<int> GenerateNumberList(IEnumerable<Card> cards)
        {
            var numberList = new List<int>();

            foreach (var card in cards)
            {
                var suitNumber = card.Suit switch
                {
                    Suit.Clubs => 0,
                    Suit.Hearts => 1,
                    Suit.Spades => 2,
                    Suit.Diamonds => 3,
                    _ => 0,
                };
                numberList.Add(suitNumber * 13 + (int)card.Number - 1);
            }

            return numberList;
        }

        /// <summary>
        /// n枚のランダムなカードを取得する
        /// </summary>
        /// <param name="n">枚数</param>
        /// <returns>n枚のカードリスト</returns>
        public static List<Card> GetRandomCards(IEnumerable<int> cardPool, int n)
        {
            return GenerateCardList(cardPool.OrderBy(i => Guid.NewGuid()).Take(n));
        }

        /// <summary>
        /// n枚のランダムなカードを取得する
        /// </summary>
        /// <param name="n">枚数</param>
        /// <returns>n枚のカードリスト</returns>
        public static List<Card> GetRandomCards(IEnumerable<Card> cardPool, int n)
        {
            return cardPool.OrderBy(i => Guid.NewGuid()).Take(n).ToList();
        }
    }
}
