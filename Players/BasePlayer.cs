using System;
using BigTwo.Game;
using BigTwo.Sorters;
using BigTwo.Types;

namespace BigTwo.Players
{
    public abstract class BasePlayer : IPlayer
    {
        protected BasePlayer(string name)
        {
            this.Name = name;
            this.Hand = new PlayerHand();
        }

        public string Name { get; set; }

        public PlayerHand Hand { get; set; }

        public void AddCard(Card card)
        {
            Hand.AddCard(card);
        }

        public int CardCount
        {
            get { return Hand.Count; }
        }

        public abstract PlayedCards PlayTurn(PlayedCards currentMove);

        public void RemoveCards(PlayedCards playedCards)
        {
            foreach (var card in playedCards)
            {
                Hand.RemoveCard(card);
            }
        }

        public void SortHand()
        {
            ICardSorter sorter = new BigTwoCardSorter();
            Card[] sortedHand = sorter.Sort(Hand.Cards);
            Hand = new PlayerHand(sortedHand);
        }
    }
}