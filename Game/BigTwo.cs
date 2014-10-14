using System;
using System.Collections.Generic;
using System.Linq;
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

        public event EventHandler<BigTwoPlayerHandEventArgs> PlayerTurn;

        public event EventHandler<BigTwoPlayerEventArgs> PlayerTurnEnd;

        public IList<IPlayer> Players
        {
            get { return this.players; }
        }

        public void DealHands()
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

                // check if the current cards are this players cards
                // this means no one could beat that players hand
                if (activeCards != null && activePlayer == activeCards.Player)
                {
                    // clear the active card to start a new sequence.
                    SequenceCompleted.Raise(this, new BigTwoPlayerEventArgs(activeCards.Player));
                    activeCards = null;
                }

                PlayedCards nextCards = activePlayer.PlayTurn(activeCards);

                PlayerTurn.Raise(this, new BigTwoPlayerHandEventArgs(activePlayer, nextCards));

                // null is a passed turn
                if (nextCards != null)
                {
                    nextCards.Validate(activeCards);

                    activeCards = nextCards;
                    activePlayer.RemoveCards(nextCards);
                }
                

                // go to next player
                activePlayerIndex++;

                if (activePlayerIndex >= players.Count)
                {
                    // loop back to the first player
                    activePlayerIndex = 0;
                }

                PlayerTurnEnd.Raise(this, new BigTwoPlayerEventArgs(activePlayer));
            }

            // game over!
            IPlayer winner = players.Single(p => p.CardCount == 0);

            GameCompleted.Raise(this, new BigTwoPlayerEventArgs(winner));
        }
    }
}
