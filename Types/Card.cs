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
