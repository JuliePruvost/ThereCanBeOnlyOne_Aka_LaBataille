using System.Linq;

namespace Core
{
    public class Card
    {
        public CardName Name { get; set; }
        public CardSymbol Symbol { get; set; }

        public string ShortCardName()
        {
            switch (Name)
            {
                case CardName.two: return "2";
                case CardName.three: return "3";
                case CardName.four: return "4";
                case CardName.five: return "5";
                case CardName.six: return "6";
                case CardName.seven: return "7";
                case CardName.eight: return "8";
                case CardName.nine: return "9";
                case CardName.ten: return "10";
                case CardName.jack: return "J";
                case CardName.queen: return "Q";
                case CardName.king: return "K";
                case CardName.ace: return "A";
                default: return Name.ToString();
            }
        }

        private static bool TryParseSymbol(char shortName, out CardSymbol symbol)
        {
            symbol = default;
            switch (shortName)
            {
                case 's': symbol = CardSymbol.spade; return true;
                case 'c': symbol = CardSymbol.club; return true;
                case 'd': symbol = CardSymbol.diamond; return true;
                case 'h': symbol = CardSymbol.heart; return true;
            }
            return false;
        }

        private static bool TryParseName(string shortName, out CardName cardName)
        {
            cardName = default;
            switch (shortName)
            {
                case "2": cardName = CardName.two; return true;
                case "3": cardName = CardName.three; return true;
                case "4": cardName = CardName.four; return true;
                case "5": cardName = CardName.five; return true;
                case "6": cardName = CardName.six; return true;
                case "7": cardName = CardName.seven; return true;
                case "8": cardName = CardName.eight; return true;
                case "9": cardName = CardName.nine; return true;
                case "10": cardName = CardName.ten; return true;
                case "J": cardName = CardName.jack; return true;
                case "Q": cardName = CardName.queen; return true;
                case "K": cardName = CardName.king; return true;
                case "A": cardName = CardName.ace; return true;
            }
            return false;
        }

        public static bool TryParseCard(string shortName, out Card card)
        {
            card = null;
            var shortSymbol = shortName.Last();
            if (TryParseSymbol(shortSymbol, out var symbol) == false)
            {
                return false;
            }

            var shortCardName = shortName.Substring(0, shortName.Length-1);
            if (TryParseName(shortCardName, out var cardName) == false)
            {
                return false;
            }

            card = new Card() { Name = cardName, Symbol = symbol };
            return true;
        }

        public override string ToString()
        {
            return $"{ShortCardName()}{Symbol.ToString().First()}";
        }
    }
}
