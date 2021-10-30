namespace Core
{
    public class PlayerDeck
    {
        public Player Player { get; private set; }
        public IDeck Deck { get; private set; }

        public PlayerDeck(Player player, IDeck deck)
        {
            Player = player;
            Deck = deck;
        }
    }
}
