using System;
using System.Collections.Generic;
using System.Linq;
using BigTwo.Game;
using BigTwo.Sorters;
using BigTwo.Types;

namespace BigTwo.Players
{
    public class ComputerPlayer : BasePlayer
    {
        public ComputerPlayer(string name) : base(name)
        {
        }

        public override PlayedCards PlayTurn(PlayedCards currentMove)
        {
            PlayedCards cardsToPlay;

            // Are we playing the first turn of this sequence?
            if (currentMove == null || currentMove[0] == null)
            {
                cardsToPlay = GetStartingCards();
            }
            else
            {
                cardsToPlay = GetCardsBetterThan(currentMove);
            }

            return cardsToPlay;
        }

        private PlayedCards GetCardsBetterThan(PlayedCards currentMove)
        {
            PlayedCards cardsToPlay;

            // We're continuing a sequence, determine the best hand to play
            switch (currentMove.Type)
            {
                case PokerHands.Single:
                    cardsToPlay = GetSingleCard(currentMove[0]);
                    break;
                case PokerHands.Double:
                    cardsToPlay = GetMatchingCards(currentMove, PokerHands.Double);
                    break;
                case PokerHands.Triple:
                    cardsToPlay = GetMatchingCards(currentMove, PokerHands.Triple);
                    break;
                case PokerHands.Quadruple:
                    cardsToPlay = GetMatchingCards(currentMove, PokerHands.Quadruple);
                    break;
                case PokerHands.Straight:
                case PokerHands.Flush:
                case PokerHands.FullHouse:
                case PokerHands.StraightFlush:
                    // 5 card hand
                    throw new NotImplementedException();
                default:
                    throw new InvalidHandException(currentMove, "Not implemented");
            }

            return cardsToPlay;
        }

        private PlayedCards GetStartingCards()
        {
            var cardsToPlay = new PlayedCards(this);

            // try to play hands with the most number of cards first.
            if (Hand.HasMatchingCards(PokerHands.Quadruple))
            {
                IEnumerable<Card> lowestMatchingCards = GetLowestMatchingCards(PokerHands.Quadruple);
                cardsToPlay.Type = PokerHands.Quadruple;
                cardsToPlay.AddCards(lowestMatchingCards);
            }
            else if (Hand.HasMatchingCards(PokerHands.Triple))
            {
                IEnumerable<Card> lowestMatchingCards = GetLowestMatchingCards(PokerHands.Triple);
                cardsToPlay.Type = PokerHands.Triple;
                cardsToPlay.AddCards(lowestMatchingCards);
            }
            else if (Hand.HasMatchingCards(PokerHands.Double))
            {
                IEnumerable<Card> lowestMatchingCards = GetLowestMatchingCards(PokerHands.Double);
                cardsToPlay.Type = PokerHands.Double;
                cardsToPlay.AddCards(lowestMatchingCards);
            }
            else
            {
                // we get to start off the hand
                Card lowestCard = GetLowestCard();
                cardsToPlay.Type = PokerHands.Single;
                cardsToPlay.AddCard(lowestCard);
            }

            return cardsToPlay;
        }

        /// <summary>
        /// Gets the lowest hand we have of the specified type
        /// </summary>
        /// <param name="type">The type of hand to match</param>
        /// <returns></returns>
        private IEnumerable<Card> GetLowestMatchingCards(PokerHands type)
        {
            return Hand.GetMatchingCards(type).First();
        }

        /// <summary>
        /// Gets the lowest hand we have of the specified type
        /// </summary>
        /// <param name="type">The type of hand to match</param>
        /// <returns></returns>
        private PlayedCards GetMatchingCards(PlayedCards currentMove, PokerHands type)
        {
            IList<IGrouping<int, Card>> matchingCards = Hand.GetMatchingCards(type).ToList();

            PlayedCards cardsToPlay = null;

            // Do we have any pairs?
            if (matchingCards.Any())
            {
                if (currentMove == null)
                {
                    // Play the lowest pair on a new sequence
                    IList<Card> lowestPair = matchingCards.First().ToList();
                    cardsToPlay = new PlayedCards(this, lowestPair);
                }
                else
                {
                    int currentPairValue = currentMove[0].GameValue;
                    IGrouping<int, Card> lowestPair = matchingCards.FirstOrDefault(c => c.First().GameValue > currentPairValue);

                    if (lowestPair != null)
                    {
                        cardsToPlay = new PlayedCards(this, lowestPair);
                    }
                    else
                    {
                        // we do not have a better pair than the current move
                    }
                }
            }

            if (cardsToPlay != null)
            {
                cardsToPlay.Type = type;
            }

            return cardsToPlay;
        }

        /// <summary>
        /// Gets the single lowest card we have that isn't part of a stronger hand
        /// </summary>
        /// <returns></returns>
        private Card GetLowestCard()
        {
            // Sort the cards, then pick the first card (which is now the lowest) that matches
            ICardSorter sorter = new BigTwoCardSorter();
            Card[] sortedCards = sorter.Sort(Hand.Cards);

            // We don't want to break up a stronger hand if we don't have to
            // For example, we don't want to return a 7 of hearts if we also have a 7 of spades.
            Card card = sortedCards.FirstOrDefault(c => !Hand.IsCardPartOfStrongerHand(c)) ?? sortedCards.First();
            
            return card;
        }

        /// <summary>
        /// Gets a single card that beats the card passed in, tries to preserve stronger hands
        /// </summary>
        /// <param name="activeCard">The card to beat</param>
        /// <returns></returns>
        private PlayedCards GetSingleCard(Card activeCard)
        {
            ICardSorter sorter = new BigTwoCardSorter();
            Card[] sortedCards = sorter.Sort(Hand.Cards);

            // Again, we don't want to break up a stronger hand if we don't have to
            Card cardToPlay = sortedCards.FirstOrDefault(
                c => c.IsGreaterThan(activeCard) && !Hand.IsCardPartOfStrongerHand(c)
            );

            if (cardToPlay == null)
            {
                // Looks like we may have to break up a stronger hand
                cardToPlay = sortedCards.FirstOrDefault(
                    c => c.IsGreaterThan(activeCard) || c.GameValue > activeCard.GameValue
                );
            }

            return new PlayedCards(this, new[] { cardToPlay });
        }
    }
}