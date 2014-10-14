using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BigTwo.Game;
using BigTwo.Players;

namespace BigTwoConsole
{
    class Program
    {
        private static BigTwoGame game;

        public static void Main(string[] args)
        {
            var players = new IPlayer[]
            {
                //new HumanPlayer("You"),
                new ComputerPlayer("Computer Player 1"),
                new ComputerPlayer("Computer Player 2"),
                new ComputerPlayer("Computer Player 3"),
                new ComputerPlayer("Computer Player 4")
            };

            game = new BigTwoGame(players);
            game.GameStarted += game_GameStarted;
            game.GameCompleted += game_GameCompleted;
            game.PlayerTurnStart += game_PlayerTurnStart;
            game.PlayerTurn += game_PlayerTurn;
            game.PlayerTurnEnd += game_PlayerTurnEnd;
            game.SequenceCompleted += game_SequenceCompleted;
            game.Start();
        }

        static void game_SequenceCompleted(object sender, BigTwoPlayerEventArgs e)
        {
            Console.WriteLine("{0} won the sequence!", e.Player.Name);

            foreach (IPlayer player in game.Players)
            {
                Console.WriteLine("{0} has {1} cards remaining.", player.Name, player.CardCount);
            }

            Console.WriteLine();
        }

        static void game_PlayerTurnEnd(object sender, BigTwoPlayerEventArgs e)
        {
            //Console.WriteLine(e.Player.Name + " turn completed.");
        }

        static void game_PlayerTurn(object sender, BigTwoPlayerHandEventArgs e)
        {
            var sb = new StringBuilder(e.Player.Name);

            if (e.PlayedCards != null && e.PlayedCards[0] != null)
            {
                sb.Append(" plays ");
                foreach (var card in e.PlayedCards.Cards)
                {
                    sb.Append(card.GetCardName());
                    sb.Append(", ");
                }

                sb.Remove(sb.Length - 2, 2);
            }
            else
            {
                sb.Append(" passes.");
            }

            Console.WriteLine(sb.ToString());
        }

        static void game_PlayerTurnStart(object sender, BigTwoPlayerEventArgs e)
        {
            //Console.WriteLine(e.Player.Name + " turn started.");
        }

        static void game_GameCompleted(object sender, BigTwoPlayerEventArgs e)
        {
            Console.WriteLine(e.Player.Name + " is the winner!");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void game_GameStarted(object sender, BigTwoEventArgs e)
        {
            Console.WriteLine("Game Start!");
        }
    }
}
