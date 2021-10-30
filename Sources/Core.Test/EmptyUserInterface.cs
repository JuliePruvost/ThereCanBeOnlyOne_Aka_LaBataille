using System;
using System.Collections.Generic;

namespace Core.Test
{
    public class EmptyUserInterface : IUserInterface
    {
        public void DisplayNoCardLeft(Player player)
        {
        }

        public void DisplayWar(IEnumerable<Player> players)
        {
        }

        public void PlayHiddenCard(Player player, Card card)
        {
        }

        public void PlayVisibleCard(Player player, Card card)
        {
        }

        public void StartGame()
        {
        }

        public void StartTurn()
        {
        }
    }
}
