using System.Collections.Generic;
using BigTwo.Players;

namespace BigTwo.Types
{
    public class PlayerList : List<IPlayer>
    {
        public PlayerList(IEnumerable<IPlayer> players) : base(players)
        {
        }
    }
}