using System;
using System.Collections.Generic;

namespace Core
{
    public interface IUserInterface
    {
        void PlayVisibleCard(Player player, Card card);
        void PlayHiddenCard(Player player, Card card);
        void DisplayNoCardLeft(Player player);
        void StartTurn(IEnumerable<PlayerDeck> _playerDecks);
        void StartGame();
        void DisplayWar(IEnumerable<Player> players);
        void DisplayException(Exception e);
        PlayMode GetPlayMode();
        (int playersCount, int gamesCount) GetMultiGameInput();
        void DisplayGameResult(Player winner);
        void DisplayRatings(IEnumerable<Player> players);
        IEnumerable<IEnumerable<Card>> GetSingleGameInput();
    }
}
