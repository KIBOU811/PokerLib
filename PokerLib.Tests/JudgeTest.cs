using PokerLib.Entity;

namespace PokerLib.Tests
{
    public class JudgeTest
    {
        [Theory]
        [ClassData(typeof(StraightTestData))]
        public void HasStraightTest(IEnumerable<Card> cards, bool expected, List<List<Card>> _)
        {
            Assert.Equal(expected, Judge.HasStraight(cards));
        }

        [Theory]
        [ClassData(typeof(StraightTestData))]
        public void GetStraighHandsTest(IEnumerable<Card> cards, bool _, List<List<Card>> expected)
        {
            var actual = Judge.GetStraightHands(cards);
            if (actual is null)
            {
                Assert.Null(actual);
                return;
            }

            foreach (var (expectedElm, actualElm) in expected.Zip(actual))
            {
                foreach (var (e, a) in expectedElm.Zip(actualElm))
                {
                    Assert.Equal(e, a);
                }
            }
        }

        [Fact]
        public void IsDrawOfStraight()
        {
            var cardList = PokerLib.Generate.GenerateCardList([1, 2, 3, 8, 9]);
            var actual = Judge.IsDrawOfStraight(cardList);
            Assert.True(actual);
        }
    }

    public class StraightTestData : TheoryData<IEnumerable<Card>, bool, List<List<Card>>>
    {
        public StraightTestData()
        {
            foreach (var (cards, expected, expectedCards) in CardsData.Zip(ExpectedData, ExptectedCardsData))
            {
                Add(cards, expected, expectedCards);
            }
        }

        private static IEnumerable<List<Card>> CardsData =>
            [
                [
                    new Card(Suit.Hearts, 1),
                    new Card(Suit.Hearts, 2),
                    new Card(Suit.Hearts, 3),
                    new Card(Suit.Hearts, 4),
                    new Card(Suit.Hearts, 5),
                ]
            ];

        private static IEnumerable<bool> ExpectedData =>
            [
                true
            ];

        private static IEnumerable<List<List<Card>>> ExptectedCardsData =>
            [
                [
                    [
                        new Card(Suit.Hearts, 1),
                        new Card(Suit.Hearts, 2),
                        new Card(Suit.Hearts, 3),
                        new Card(Suit.Hearts, 4),
                        new Card(Suit.Hearts, 5),
                    ]
                ]
            ];
    }
}