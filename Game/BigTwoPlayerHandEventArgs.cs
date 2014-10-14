using BigTwo.Players;

namespace BigTwo.Game
{
    public class BigTwoPlayerHandEventArgs : BigTwoPlayerEventArgs
    {
        public PlayedCards PlayedCards { get; private set; }

        public BigTwoPlayerHandEventArgs(IPlayer activePlayer, PlayedCards nextCards) : base(activePlayer)
        {
            PlayedCards = nextCards;
        }
    }
}