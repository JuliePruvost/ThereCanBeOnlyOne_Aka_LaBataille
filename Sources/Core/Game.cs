using System.Linq;
using System.Collections.Generic;
using System;

namespace Core
{
    public class Game
    {
        private readonly IUserInterface _userInterface;
        private readonly IEnumerable<PlayerDeck> _playerDecks;

        public Game(IUserInterface userInterface, IEnumerable<PlayerDeck> playerDecks)
        {
            _userInterface = userInterface;
            _playerDecks = playerDecks;
        }

        public IDeck GetPlayerDeck(string playerName)
        {
            return _playerDecks.FirstOrDefault(pd => pd.Player.Name == playerName)?.Deck;
        }

        // returns the winner of the game
        public Player RunGame()
        {
            _userInterface.StartGame();            
            while (true)
            {
                var alivePlayerDecks = _playerDecks.Where(pd => pd.Deck.Any());
                var alivePlayerCount = alivePlayerDecks.Count();
                if (alivePlayerCount == 0)
                {
                    // no cards in this game
                    throw new NotImplementedException("No player has any card");
                }
                else if (alivePlayerCount == 1)
                {
                    return alivePlayerDecks.First().Player;
                }

                RunTurn();
            }
        }

        public void RunTurn()
        {
            _userInterface.StartTurn(_playerDecks);
            var topVisibleCardByPlayerDeck = new Dictionary<PlayerDeck, Card>();
            var cardsInThePot = new List<Card>();

            // draw
            foreach (var playerDeck in _playerDecks)
            {
                if (playerDeck.Deck.TryDraw(out var card))
                {
                    _userInterface.PlayVisibleCard(playerDeck.Player, card);
                    topVisibleCardByPlayerDeck[playerDeck] = card;
                    cardsInThePot.Add(card);
                }
                else
                {
                    _userInterface.DisplayNoCardLeft(playerDeck.Player);
                }
            }

            // compare cards
            var maxValue = topVisibleCardByPlayerDeck.Values.Max(CardValue);
            var playerDecksHavingMaxValue = topVisibleCardByPlayerDeck
                .Where(kvp => CardValue(kvp.Value) == maxValue)
                .Select(kvp => kvp.Key);
            PlayerDeck winner;
            if (playerDecksHavingMaxValue.Count() == 1)
            {
                winner = playerDecksHavingMaxValue.Single();
            }
            else
            {
                winner = HandleWar(cardsInThePot, playerDecksHavingMaxValue);

            }
            AddPotToDeck(cardsInThePot, winner);
        }

        private PlayerDeck HandleWar(IList<Card> cardsInThePot, IEnumerable<PlayerDeck> playerDecksEngagedInWar)
        {
            _userInterface.DisplayWar(playerDecksEngagedInWar.Select(pd => pd.Player));

            var topVisibleCardByPlayerDeck = new Dictionary<PlayerDeck, Card>();

            // draw
            foreach (var playerDeck in playerDecksEngagedInWar)
            {
                if (playerDeck.Deck.TryDraw(out var card))
                {
                    _userInterface.PlayHiddenCard(playerDeck.Player, card);
                    cardsInThePot.Add(card);
                    if (playerDeck.Deck.TryDraw(out card))
                    {
                        _userInterface.PlayVisibleCard(playerDeck.Player, card);
                        topVisibleCardByPlayerDeck[playerDeck] = card;
                        cardsInThePot.Add(card);
                    }
                    else
                    {
                        // could implement here a draw in another player's hand
                        _userInterface.DisplayNoCardLeft(playerDeck.Player);
                    }
                }
                else
                {
                    // could implement here a draw in another player's hand
                    _userInterface.DisplayNoCardLeft(playerDeck.Player);
                }
            }

            if (topVisibleCardByPlayerDeck.Any() == false)
            {
                // players in war have no card anymore (very rare case)
                // we select any player as a winner so the game can continue
                return playerDecksEngagedInWar.First();
            }

            // compare cards
            var maxValue = topVisibleCardByPlayerDeck.Values.Max(CardValue);
            var playerDecksHavingMaxValue = topVisibleCardByPlayerDeck
                .Where(kvp => CardValue(kvp.Value) == maxValue)
                .Select(kvp => kvp.Key);
            if (playerDecksHavingMaxValue.Count() == 1)
            {
                return playerDecksHavingMaxValue.Single();
            }
            else
            {
                return HandleWar(cardsInThePot, playerDecksHavingMaxValue);
            }
        }

        private void AddPotToDeck(IEnumerable<Card> cardsInThePot, PlayerDeck playerDeck)
        {
            // we shuffle the card here to prevent never ending games
            playerDeck.Deck.Add(CardsDealer.Shuffle(cardsInThePot));
        }

        private static int CardValue(Card card)
        {
            return (int) card.Name;
        }
    }
}
