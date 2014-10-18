using BigTwo.Game;
using BigTwo.Types;

namespace BigTwo.Players
{
    public interface IPlayer
    {
        void AddCard(Card card);

        int CardCount { get; }

        string Name { get; set; }

        PlayedCards PlayTurn(PlayedCards currentMove);

        void RemoveCards(PlayedCards nextPlayedCards);

        void SortHand();
    }
}