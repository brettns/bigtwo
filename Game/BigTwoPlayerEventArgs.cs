using System;
using BigTwo.Players;

namespace BigTwo.Game
{
    public class BigTwoPlayerEventArgs : EventArgs
    {
        public IPlayer Player { get; private set; }

        public BigTwoPlayerEventArgs(IPlayer player)
        {
            Player = player;
        }
    }
}