using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigTwo.Types
{
    public class CardCollection : IEnumerable<Card>
    {
        protected IList<Card> cards;

        public CardCollection()
        {
            this.cards = new List<Card>();
        }

        public CardCollection(IEnumerable<Card> cards)
        {
            this.cards = new List<Card>(cards);
        }

        public Card this[int i]
        {
            get { return cards[i]; }
        }

        public int Count
        {
            get { return cards.Count; }
        }

        public Card[] Cards
        {
            get { return cards.ToArray(); }
        }

        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        public void AddCards(IEnumerable<Card> collection)
        {
            foreach (Card card in collection)
            {
                this.cards.Add(card);
            };
        }

        public void RemoveCard(Card card)
        {
            cards.Remove(card);
        }

        public void Clear()
        {
            cards.Clear();
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return this.cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (Card card in cards)
            {
                sb.Append(card.GetCardName());
                sb.Append(", ");
            }

            // remove the trailing comma and space
            return sb.ToString(0, sb.Length - 2);
        }
    }
}