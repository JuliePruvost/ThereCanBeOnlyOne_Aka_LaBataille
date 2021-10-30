using System;
using System.Linq;
using System.Collections.Generic;
using Core;

namespace ClientConsole
{
    public class ConsoleUserInterface : IUserInterface
    {
        public void DisplayException(Exception e)
        {
            Console.WriteLine("Something unexepcted occur !");
            Console.WriteLine(e.ToString());
        }

        public void DisplayNoCardLeft(Player player)
        {
            Console.Write($" [{player.Name} NoCard]");
        }

        public void DisplayWar(IEnumerable<Player> players)
        {
            var listOfPlayers = players.Select(p => p.Name).Aggregate((p1, p2) => $"{p1}|{p2}");
            Console.Write($" - [War between {listOfPlayers}]");
        }

        public (int playersCount, int gamesCount) GetMultiGameInput()
        {
            var playersCount = ReadIntInput("How many players ?");
            var gamesCount = ReadIntInput("How many games ?");
            return (playersCount, gamesCount);
        }

        private int ReadIntInput(string message)
        {
            Console.WriteLine(message);
            var playersCountInput = Console.ReadLine();
            if (int.TryParse(playersCountInput, out var playersCount) == false)
            {
                return ReadIntInput(message);
            }
            return playersCount;
        }

        public PlayMode GetPlayMode()
        {
            Console.WriteLine("Select your play mode : 1 for MultiGames, 2 for a SingleGame prearranged deck (anything else to quit)");
            Console.WriteLine("1 or 2 ?");
            var mode = Console.ReadLine();
            switch (mode)
            {
                case "1": return PlayMode.MutliGame;
                case "2": return PlayMode.SingleGame;
                default: return PlayMode.Quit;
            }
        }

        public void PlayHiddenCard(Player player, Card card)
        {
        }

        public void PlayVisibleCard(Player player, Card card)
        {
            Console.Write($" {player.Name}:{card.ToString()}");
        }

        public void StartGame()
        {
            Console.WriteLine();
            Console.Write($" New Game ");
        }

        public void StartTurn(IEnumerable<PlayerDeck> _playerDecks)
        {
            Console.WriteLine();
            // for debug
            var stats = _playerDecks.Select(pd => $"{pd.Player.Name}:{pd.Deck.Count()}").Aggregate((p1, p2) => $"{p1}|{p2}");
            Console.Write("    " + stats);
            Console.Write("    ");
        }

        public void DisplayGameResult(Player winner)
        {
            Console.WriteLine();
            Console.WriteLine($" Winner is {winner.Name}");
        }

        public void DisplayRatings(IEnumerable<Player> players)
        {
            Console.WriteLine();
            Console.WriteLine("RATINGS");
            Console.WriteLine(String.Join(" - ", players
                .OrderByDescending(p => p.Score)
                .Select(p => $"{p.Name} score = {p.Score}")));
            Console.WriteLine();
        }

        public IEnumerable<IEnumerable<Card>> GetSingleGameInput()
        {
            Console.WriteLine();
            Console.WriteLine("Enter each player deck.");
            Console.WriteLine("Cards are separated by -"); 
            Console.WriteLine("Decks are separated by a whitespace"); 
            Console.WriteLine("Example below for 3 decks (player3 has a single card : ace of heart)");
            Console.WriteLine("10s-Kd-Qc 2h-2c-2d-2s Ah");
            return GetSingleGameCorrectInput();
        }

        public IEnumerable<IEnumerable<Card>> GetSingleGameCorrectInput()
        {
            Console.WriteLine("Please enter a correct input");
            var input = Console.ReadLine();
            if (TryParseDeckInputs(input, out var decks) == false)
            {
                return GetSingleGameCorrectInput();
            }
            return decks;
        }

        public bool TryParseDeckInputs(string inputs, out IList<IEnumerable<Card>> decks)
        {
            decks = new List<IEnumerable<Card>>();

            var deckInputs = inputs.Split(' ');
            foreach(var deckInput in deckInputs)
            {
                var deck = new List<Card>();
                var cardInputs = deckInput.Split('-');
                foreach(var cardInput in cardInputs)
                {
                    if (Card.TryParseCard(cardInput, out var card) == false)
                    {
                        return false;
                    }
                    deck.Add(card);
                }
                decks.Add(deck);
            }
            return true;
        }
    }
}
