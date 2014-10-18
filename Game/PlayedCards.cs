using System;
using System.Collections.Generic;
using BigTwo.Players;
using BigTwo.Types;

namespace BigTwo.Game
{
    public class PlayedCards : CardCollection
    {
        public PlayedCards(IPlayer player)
        {
            Player = player;
        }

        public PlayedCards(IPlayer player, IEnumerable<Card> cards) : base(cards)
        {
            Player = player;
        }

        public IPlayer Player { get; private set; }

        public PokerHands Type { get; set; }

        public void Validate(PlayedCards activeCards)
        {
            if (activeCards == null || activeCards[0] == null || this[0] == null)
            {
                return;
            }

            if (activeCards.Count != this.Count)
            {
                throw new InvalidHandException(this, "You must play the same number of cards.");
            }

            if (this.Count == 1)
            {
                // Validate single card hand
                var nextCard = this[0];
                var activeCard = activeCards[0];

                if (nextCard.GameValue < activeCard.GameValue)
                {
                    throw new InvalidHandException(this, "Cannot play a card of lesser value.");
                }

                if (nextCard.GameValue == activeCard.GameValue && nextCard.Suit < activeCard.Suit)
                {
                    throw new InvalidHandException(this, "Cannot play an equal value card of a lesser suit.");
                }
            }

            // todo: validate poker style hands
        }
    }
}