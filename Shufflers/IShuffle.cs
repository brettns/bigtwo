using System.Collections.Generic;
using BigTwo.Types;

namespace BigTwo.Shufflers
{
    public interface IShuffle
    {
        void Shuffle(Card[] deck);
    }
}