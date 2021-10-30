using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class GameManager
    {
        private static Dictionary<PlayMode, Action<GameManager>> executionStrategyByPlayMode = new Dictionary<PlayMode, Action<GameManager>>() 
        {
            {PlayMode.MutliGame, (gm) => gm.RunMultiGameMode() },
            {PlayMode.SingleGame, (gm) => gm.RunSingleGameMode() }
        };
        private readonly IUserInterface _userInterface;

        public GameManager(IUserInterface userInterface)
        {
            _userInterface = userInterface;
        }

        public void Run()
        {
            try
            {
                while (true)
                {
                    var playMode = _userInterface.GetPlayMode();
                    if (playMode == PlayMode.Quit)
                    {
                        return;
                    }
                    Run(playMode);
                }
            }
            catch (Exception e)
            {
                // log...
                _userInterface.DisplayException(e);
            }
        }

        public void Run(PlayMode playMode)
        {
            if (executionStrategyByPlayMode.TryGetValue(playMode, out var action) == false)
            {
                throw new NotImplementedException($"Play mode {playMode} is not implemented");
            }
            action(this);
        }

        private void RunMultiGameMode()
        {
            (int playersCount, int gamesCount) = _userInterface.GetMultiGameInput();
            var players = CreatePlayers(playersCount);

            for (int gameNumber = 0; gameNumber < gamesCount; gameNumber++)
            {
                var playerDecks = CardsDealer.CreatePlayerDecks(players);
                var game = new Game(_userInterface, playerDecks);
                var winner = game.RunGame();

                _userInterface.DisplayGameResult(winner);

                winner.Score++;
            }
            _userInterface.DisplayRatings(players);
        }

        private void RunSingleGameMode()
        {
            var playerCardsCollection = _userInterface.GetSingleGameInput();
            var playerDecks = CreatePlayerDecks(playerCardsCollection);

            var game = new Game(_userInterface, playerDecks);
            var winner = game.RunGame();

            _userInterface.DisplayGameResult(winner);

            winner.Score++;

            _userInterface.DisplayRatings(playerDecks.Select(pd => pd.Player));
        }

        private IEnumerable<Player> CreatePlayers(int playersCount)
        {
            return Enumerable.Range(1, playersCount)
                .Select(i => "P" + i)
                .Select(name => new Player() { Name = name }).ToList();
        }

        private IEnumerable<PlayerDeck> CreatePlayerDecks(IEnumerable<IEnumerable<Card>> playerCardsCollection)
        {
            var result = new List<PlayerDeck>();
            for(int i=0; i< playerCardsCollection.Count(); i++)
            {
                var player = new Player() { Name = "P" + (i + 1) };
                var deck = new Deck(playerCardsCollection.ElementAt(i));
                result.Add(new PlayerDeck(player, deck));
            }
            return result;
        }
    }
}
