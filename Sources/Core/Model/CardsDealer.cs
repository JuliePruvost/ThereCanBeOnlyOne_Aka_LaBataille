using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class CardsDealer
    {
        public const int nbCardPerSymbol = 13;

        public static IList<Card> CreateSorted52CardsSet()
        {
            return Enumerable.Range(0, nbCardPerSymbol).Select(i => new Card { Name = (CardName)i, Symbol = CardSymbol.club })
                .Union(Enumerable.Range(0, nbCardPerSymbol).Select(i => new Card { Name = (CardName)i, Symbol = CardSymbol.diamond }))
                .Union(Enumerable.Range(0, nbCardPerSymbol).Select(i => new Card { Name = (CardName)i, Symbol = CardSymbol.heart }))
                .Union(Enumerable.Range(0, nbCardPerSymbol).Select(i => new Card { Name = (CardName)i, Symbol = CardSymbol.spade }))
                .ToList();
        }

        public static IList<Card> Shuffle(IEnumerable<Card> cards)
        {
            var list = cards.ToList();

            // algo taken from https://stackoverflow.com/questions/273313/randomize-a-listt
            var rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                // switch list[k] and list[n], with a temp value
                var temp = list[k];
                list[k] = list[n];
                list[n] = temp;
            }
            return list;
        }

        public static IEnumerable<PlayerDeck> CreatePlayerDecks(IEnumerable<Player> players)
        {
            var set = Shuffle(CreateSorted52CardsSet());
            var result = players.Select(p => new PlayerDeck(p, new Deck(new Card[] { }))).ToList();
            var playersCount = players.Count();

            for (int i=0; i < set.Count; i++)
            {
                result[i % playersCount].Deck.Add(new Card[] { set.ElementAt(i) });
            }

            return result;
        }
    }
}
