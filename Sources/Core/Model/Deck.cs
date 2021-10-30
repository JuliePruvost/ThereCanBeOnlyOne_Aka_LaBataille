using System.Linq;
using System.Collections.Generic;

namespace Core
{
    public class Deck : IDeck
    {
        private Queue<Card> _queue;

        public Deck(IEnumerable<Card> cards)
        {
            _queue = new Queue<Card>(cards);
        }

        public bool TryDraw(out Card card)
        {
            card = null;
            if (_queue.Any() == false)
            {
                return false;
            }

            card = _queue.Dequeue();
            return true;
        }

        public void Add(IEnumerable<Card> cards)
        {
            foreach(var card in cards)
            {
                _queue.Enqueue(card);
            }
        }

        public bool Any()
        {
            return _queue.Any();
        }

        public int Count()
        {
            return _queue.Count();
        }
    }
}
