using System;
using System.Text;
using BigTwo.Game;
using BigTwo.Types;

namespace BigTwo.Players
{
    public class HumanPlayer : BasePlayer
    {
        public HumanPlayer(string name) : base(name)
        {
        }

        public override PlayedCards PlayTurn(PlayedCards currentMove)
        {
            var cardsToPlay = new PlayedCards(this);

            // display the cards to the player
            DisplayCardList();

            bool valid = false;
            do
            {

                var stillBuildingHand = true;

                do
                {
                    int selectedCardIndex = GetUserSelectedCardIndex();

                    if (selectedCardIndex >= 0)
                    {
                        Card selectedCard = Hand[selectedCardIndex];
                        cardsToPlay.AddCard(selectedCard);
                        valid = true;
                    }

                    if (!ContinueBuildingHandPrompt())
                    {
                        stillBuildingHand = false;
                    }
                } while (stillBuildingHand);


                try
                {
                    // validate the move
                    cardsToPlay.Validate(currentMove);
                }
                catch (Exception ex)
                {
                    cardsToPlay.Clear();
                    Console.WriteLine("Invalid move: " + ex.Message);
                    valid = false;
                }
            } while (valid == false);

            // play the cards);
            return cardsToPlay;
        }

        private static bool ContinueBuildingHandPrompt()
        {
            Console.WriteLine("Do you want to add another card? (Y/N)");
            string result = Console.ReadLine();
            return string.Equals(result, "Y", StringComparison.OrdinalIgnoreCase);
        }

        private int GetUserSelectedCardIndex()
        {
            // prompt for input
            Console.WriteLine("Please select a card to play.");
            string rawInput = Console.ReadLine();

            int index;

            if (int.TryParse(rawInput, out index) && index >= 0 && index < Hand.Count)
            {
                if (index > 0)
                {
                    // transform to 0 based index
                    index = index - 1;
                }
                else
                {
                    index = -1;
                }
            }
            else
            {
                index = -1;
                Console.WriteLine("Invalid selection... expected a number between 1 and " + Hand.Count);
            }

            return index;
        }

        private void DisplayCardList()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < Hand.Count; i++)
            {
                sb.Append("[");
                sb.Append(i + 1);
                sb.Append("] ");
                sb.AppendLine(Hand[i].GetCardName());
            }

            sb.AppendLine("[0] Pass this turn.");

            Console.WriteLine(sb.ToString());
        }
    }
}