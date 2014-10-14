using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BigTwo.Shufflers;

namespace BigTwo.Types
{
    public class Deck : IEnumerable<Card>
    {
        private readonly Card[] cards;
        private const int SuitsInDeck = 4;
        private const int CardsInDeck = 52;
        private const int CardsPerSuit = CardsInDeck / SuitsInDeck;

        public Deck()
        {
            cards = new Card[CardsInDeck];
            Shuffler = new FisherYatesShuffle();

            var suits = new[]
            {
                CardSuit.Diamonds,
                CardSuit.Clubs,
                CardSuit.Hearts,
                CardSuit.Spades
            };

            for (int i = 0; i < SuitsInDeck; i++)
            {
                CardSuit suit = suits[i];


                for (int j = 0; j < CardsPerSuit; j++)
                {
                    var index = j + (i * CardsPerSuit);

                    var value = j + 1;
                    int gameValue;

                    if (value == 1)
                    {
                        // aces are high
                        gameValue = 14;
                    }
                    else if (value == 2)
                    {
                        // 2's are the highest valued cards in this game.
                        gameValue = 15;
                    }
                    else
                    {
                        gameValue = value;
                    }

                    cards[index] = new Card(value, gameValue, suit);
                }
            }
        }

        public IShuffle Shuffler { get; set; }

        public Card this[int i]
        {
            get { return cards[i]; }
            set { cards[i] = value; }
        }

        public int Count
        {
            get { return cards.Length; }
        }

        public void Shuffle()
        {
            Shuffler.Shuffle(cards);
        }

        IEnumerator<Card> IEnumerable<Card>.GetEnumerator()
        {
            return cards.Cast<Card>().GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return cards.GetEnumerator();
        }
    }
}