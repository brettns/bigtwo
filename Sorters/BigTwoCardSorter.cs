using System.Linq;
using BigTwo.Types;

namespace BigTwo.Sorters
{
    class BigTwoCardSorter : ICardSorter
    {
        public Card[] Sort(Card[] cards)
        {
            return cards
                .OrderBy(x => x.GameValue)
                .ThenBy(x => x.Suit)
                .ToArray();
        }
    }
}