namespace PokerLib
{
    /// <summary>
    /// スート
    /// </summary>
    public enum Suit
    {
        Clubs,
        Hearts,
        Spades,
        Diamonds
    }

    public enum Hand
    {
        FiveOfAKind,
        StraightFlush,
        FourOfAKind,
        FullHouse,
        Flush,
        Straight,
        ThreeOfAKind,
        TwoPair,
        OnePair,
        None
    }

    public enum WinOrLose
    {
        Win,
        Draw,
        Lose
    }


    public class Define
    {
        /// <summary>
        /// カードの数字の最低値
        /// </summary>
        public const int MinNumber = 1;

        /// <summary>
        /// カードの数字の最大値
        /// </summary>
        public const int MaxNumber = 13;

        /// <summary>
        /// 役の判定に使うカードの枚数
        /// </summary>
        public const int NumHand = 5;

        /// <summary>
        /// ドローポーカーの手札の枚数
        /// </summary>
        public const int DrawPokerHands = 5;

        /// <summary>
        /// セブンカード・ドローで配られるカードの枚数
        /// </summary>
        public const int SevenCardDrawCards = 7;

        /// <summary>
        /// ドロー（リーチ）とする枚数の定義
        /// </summary>
        public class Draw
        {
            /// <summary>
            /// フラッシュのリーチと判定する枚数
            /// </summary>
            public const int FlushCards = 3;

            /// <summary>
            /// ストレートのリーチと判定する枚数
            /// </summary>
            public const int StraightCards = 3;
        }
    }
}
