using System;

namespace BigTwo.Types
{
    public class Card
    {
        public Card(int value, int gameValue, CardSuit suit)
        {
            this.Value = value;
            this.Suit = suit;
            this.GameValue = gameValue;
        }

        public CardSuit Suit { get; private set; }

        public int Value { get; private set; }

        public int GameValue { get; private set; }

        public bool IsGreaterThan(Card other)
        {
            return (GameValue == other.GameValue && Suit > other.Suit) || GameValue > other.GameValue;
        }

        public static bool operator >(Card a, Card b)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a", "null value passed to greater than operator");
            }

            if (b == null)
            {
                throw new ArgumentNullException("b", "null value passed to greater than operator");
            }

            return (a.GameValue == b.GameValue && a.Suit > b.Suit) || a.GameValue > b.GameValue;
        }

        public static bool operator <(Card a, Card b)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a", "null value passed to less than operator");
            }

            if (b == null)
            {
                throw new ArgumentNullException("b", "null value passed to less than operator");
            }

            return (a.GameValue == b.GameValue && a.Suit < b.Suit) || a.GameValue < b.GameValue;
        }

        public static bool operator ==(Card a, Card b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Value == b.Value && a.Suit == b.Suit;
        }

        public static bool operator !=(Card a, Card b)
        {
            return !(a == b);
        }

        public string GetCardName()
        {
            string name;

            switch (Value)
            {
                case 1:
                    name = "Ace";
                    break;
                case 11:
                    name = "Jack";
                    break;
                case 12:
                    name = "Queen";
                    break;
                case 13:
                    name = "King";
                    break;
                default:
                    name = Value.ToString();
                    break;
            }

            return name + " of " + Suit;
        }
    }
}
