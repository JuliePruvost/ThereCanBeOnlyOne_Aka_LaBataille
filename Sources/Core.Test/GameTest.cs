using System.Linq;
using Xunit;

namespace Core.Test
{
    public class GameTest
    {
        private readonly EmptyUserInterface _userInterface;

        public GameTest()
        {
            _userInterface = new EmptyUserInterface();
        }

        [Fact]
        public void RunTurn_2Player_NoWar()
        {
            // arrange
            var player1 = "player1";
            var player2 = "player2";
            var player1Deck = CreatePlayerDeck(player1, CardName.king);
            var player2Deck = CreatePlayerDeck(player2, CardName.four, CardName.five);
            var game = new Game(_userInterface, new[] { player1Deck, player2Deck });

            // act
            game.RunTurn();

            // assert
            Assert.Equal(2, game.GetPlayerDeck(player1).Count());
            Assert.Equal(1, game.GetPlayerDeck(player2).Count());
        }

        [Fact]
        public void RunTurn_3Player_NoWar()
        {
            // arrange
            var player1 = "player1";
            var player2 = "player2";
            var player3 = "player3";
            var player1Deck = CreatePlayerDeck(player1, CardName.ace);
            var player2Deck = CreatePlayerDeck(player2, CardName.four);
            var player3Deck = CreatePlayerDeck(player3, CardName.jack);
            var game = new Game(_userInterface, new[] { player1Deck, player2Deck, player3Deck });

            // act
            game.RunTurn();

            // assert
            Assert.Equal(3, game.GetPlayerDeck(player1).Count());
            Assert.Equal(0, game.GetPlayerDeck(player2).Count());
            Assert.Equal(0, game.GetPlayerDeck(player3).Count());
        }

        [Fact]
        public void RunTurn_2Player_SimpleWar()
        {
            // arrange
            var player1 = "player1";
            var player2 = "player2";
            var player1Deck = CreatePlayerDeck(player1, CardName.four, CardName.five, CardName.queen);
            var player2Deck = CreatePlayerDeck(player2, CardName.four, CardName.ace, CardName.nine, CardName.four);
            var game = new Game(_userInterface, new[] { player1Deck, player2Deck });

            // act
            game.RunTurn();

            // assert
            Assert.Equal(6, game.GetPlayerDeck(player1).Count());
            Assert.Equal(1, game.GetPlayerDeck(player2).Count());
        }

        [Fact]
        public void RunTurn_3Player_SimpleWarBetween2()
        {
            // arrange
            var player1 = "player1";
            var player2 = "player2";
            var player3 = "player3";
            var player1Deck = CreatePlayerDeck(player1, CardName.four, CardName.five, CardName.queen);
            var player2Deck = CreatePlayerDeck(player2, CardName.four, CardName.ace, CardName.nine);
            var player3Deck = CreatePlayerDeck(player3, CardName.two);
            var game = new Game(_userInterface, new[] { player1Deck, player2Deck, player3Deck });

            // act
            game.RunTurn();

            // assert
            Assert.Equal(7, game.GetPlayerDeck(player1).Count());
            Assert.Equal(0, game.GetPlayerDeck(player2).Count());
            Assert.Equal(0, game.GetPlayerDeck(player3).Count());
        }

        [Fact]
        public void RunTurn_2Player_DoubleWar()
        {
            // arrange
            var player1 = "player1";
            var player2 = "player2";
            var player1Deck = CreatePlayerDeck(player1, CardName.four, CardName.five, CardName.nine, CardName.five, CardName.queen);
            var player2Deck = CreatePlayerDeck(player2, CardName.four, CardName.ace, CardName.nine, CardName.ace, CardName.nine);
            var game = new Game(_userInterface, new[] { player1Deck, player2Deck });

            // act
            game.RunTurn();

            // assert
            Assert.Equal(10, game.GetPlayerDeck(player1).Count());
            Assert.Equal(0, game.GetPlayerDeck(player2).Count());
        }

        [Fact]
        public void RunTurn_3Player_DoubleWar()
        {
            // arrange
            var player1 = "player1";
            var player2 = "player2";
            var player3 = "player3";
            var player1Deck = CreatePlayerDeck(player1, CardName.four, CardName.five, CardName.nine, CardName.five, CardName.queen);
            var player2Deck = CreatePlayerDeck(player2, CardName.four, CardName.ace, CardName.nine, CardName.ace, CardName.nine);
            var player3Deck = CreatePlayerDeck(player3, CardName.four, CardName.ace, CardName.eight, CardName.ace);
            var game = new Game(_userInterface, new[] { player1Deck, player2Deck, player3Deck });

            // act
            game.RunTurn();

            // assert
            Assert.Equal(13, game.GetPlayerDeck(player1).Count());
            Assert.Equal(0, game.GetPlayerDeck(player2).Count());
            Assert.Equal(1, game.GetPlayerDeck(player3).Count());
        }

        private static PlayerDeck CreatePlayerDeck(string playerName, params CardName[] cardNames)
        {
            var player1 = new Player() { Name = playerName };
            return new PlayerDeck(player1, new Deck(cardNames.Select(c => new Card { Name = c })));
        }
    }
}
