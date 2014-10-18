using System;
using System.Collections.Generic;
using System.Linq;

namespace BigTwo.Types
{
    public class PlayerHand : CardCollection
    {
        public PlayerHand() : base()
        {
        }

        public PlayerHand(IEnumerable<Card> cards) : base(cards)
        {
        }

        public bool HasMatchingCards(PokerHands type)
        {
            IEnumerable<IGrouping<int, Card>> pairs = GetMatchingCards(type);
            return pairs.Any();
        }

        public IEnumerable<IGrouping<int, Card>> GetMatchingCards(PokerHands type)
        {
            IEnumerable<IGrouping<int, Card>> matchingCards = this
                .GroupBy(c => c.Value)
                .Where(g => g.Count() == (int)type);

            return matchingCards;
        }

        public bool IsCardPartOfStrongerHand(Card card)
        {
            if (!this.Contains(card))
            {
                throw new ArgumentOutOfRangeException("We don't have a " + card.GetCardName());
            }

            // check if it's part of a pair, this return true for trips and quads as well.
            var isAtleastPair = GetMatchingCards(PokerHands.Double)
                .Any(g => g
                    .Any(c => c.Value == card.Value)
                );


            // todo: check if it's part of a poker hand


            // not part of stronger hand.
            return isAtleastPair;
        }
    }
}
