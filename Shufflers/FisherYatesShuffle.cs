using System;
using BigTwo.Types;

namespace BigTwo.Shufflers
{
    public class FisherYatesShuffle : IShuffle
    {
        private static readonly Random random = new Random();

        public void Shuffle(Card[] deck)
        {
            for (int n = deck.Length - 1; n > 0; --n)
            {
                int k = random.Next(n + 1);
                Card temp = deck[n];
                deck[n] = deck[k];
                deck[k] = temp;
            }
        }
    }
}