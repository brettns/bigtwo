using System;
using BigTwo.Game;

namespace BigTwo.Players
{
    public class InvalidHandException : Exception
    {
        public PlayedCards Cards { get; private set; }

        public InvalidHandException(PlayedCards cards, string message) : base(message)
        {
            Cards = cards;
        }
    }
}