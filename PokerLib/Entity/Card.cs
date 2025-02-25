namespace PokerLib.Entity
{
    public class Card : IEquatable<Card>
    {
        /// <summary>
        /// スート
        /// </summary>
        public Suit Suit { get; set; }

        /// <summary>
        /// 数字
        /// </summary>
        public uint Number { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="card">複製元のカード</param>
        public Card(Card card)
        {
            Suit = card.Suit;
            Number = card.Number;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="suit">スート</param>
        /// <param name="number">数字</param>
        public Card(Suit suit, uint number)
        {
            Suit = suit;
            Number = number;
        }

        /// <summary>
        /// Equals実装
        /// </summary>
        /// <param name="other">比較対象オブジェクト</param>
        /// <returns>オブジェクトが等しいか</returns>
        public bool Equals(Card? other)
        {
            if (other is null)
            {
                return false;
            }

            return Suit == other.Suit && Number == other.Number;
        }

        /// <summary>
        /// Equalsオーバーライド
        /// </summary>ｓ
        /// <param name="obj">比較対象オブジェクト</param>
        /// <returns>オブジェクトが等しいか</returns>
        public override bool Equals(object? obj) => Equals(obj as Card);

        /// <summary>
        /// GetHashCodeオーバーライド
        /// </summary>
        /// <returns>ハッシュ値</returns>
        public override int GetHashCode() => (Suit, Number).GetHashCode();
    }
}
