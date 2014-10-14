using BigTwo.Types;

namespace BigTwo.Sorters
{
    public interface ICardSorter
    {
        Card[] Sort(Card[] cards);
    }
}