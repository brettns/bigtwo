using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BigTwo.Players;
using BigTwo.Types;

namespace BigTwo.Game
{
    public class BigTwoGame
    {
        private readonly List<IPlayer> players;
        private readonly Deck deck;

        public BigTwoGame(IEnumerable<IPlayer> players)
        {
            this.players = new PlayerList(players);
            this.deck = new Deck();
        }

        public event EventHandler<BigTwoEventArgs> GameStarted;

        public event EventHandler<BigTwoPlayerEventArgs> GameCompleted;

        public event EventHandler<BigTwoPlayerEventArgs> PlayerTurnStart;

        public event EventHandler<BigTwoPlayerEventArgs> SequenceCompleted;

        public event EventHandler<BigTwoPlayerHandEventArgs> PlayerPlayedTurn;

        public event EventHandler<BigTwoPlayerEventArgs> PlayerTurnEnd;

        public IList<IPlayer> Players
        {
            get { return this.players; }
        }

        private void DealHands()
        {
            // shuffle the deck before dealing
            deck.Shuffle();

            // deal the cards
            var playerIndex = 0;
            foreach (Card card in deck)
            {
                // give the player a card
                players[playerIndex].AddCard(card);

                playerIndex++;

                if (playerIndex >= players.Count)
                {
                    playerIndex = 0;
                }
            }

            // sort the hands
            foreach (IPlayer player in players)
            {
                player.SortHand();
            }
        }

        public void Start()
        {
            GameStarted.Raise(this, new BigTwoEventArgs());

            DealHands();

            var activePlayerIndex = 0;

            PlayedCards activeCards = null;

            // while all players have at least one card the game continues
            while (players.All(p => p.CardCount > 0))
            {
                IPlayer activePlayer = players[activePlayerIndex];

                PlayerTurnStart.Raise(this, new BigTwoPlayerEventArgs(activePlayer));

                // If the active cards belong to the active player, It means that 
                // all other players have passed meaning this player has won the sequence.
                if (activeCards != null && activePlayer == activeCards.Player)
                {
                    // clear the active card to start a new sequence.
                    SequenceCompleted.Raise(this, new BigTwoPlayerEventArgs(activeCards.Player));
                    activeCards = null;
                }

                PlayedCards nextCards = activePlayer.PlayTurn(activeCards);

                PlayerPlayedTurn.Raise(this, new BigTwoPlayerHandEventArgs(activePlayer, nextCards));

                // null is a passed turn
                if (nextCards != null)
                {
                    // Ensure the cards the player is trying to play are valid
                    nextCards.Validate(activeCards);

                    activeCards = nextCards;

                    // Remove the played cards from the players hand
                    activePlayer.RemoveCards(nextCards);
                }
                

                // Advance to next player
                activePlayerIndex++;

                if (activePlayerIndex >= players.Count)
                {
                    activePlayerIndex = 0;
                }

                PlayerTurnEnd.Raise(this, new BigTwoPlayerEventArgs(activePlayer));
            }

            // Get the winner and notify everyone that the game is over!
            IPlayer winner = players.Single(p => p.CardCount == 0);

            GameCompleted.Raise(this, new BigTwoPlayerEventArgs(winner));
        }
    }
}
