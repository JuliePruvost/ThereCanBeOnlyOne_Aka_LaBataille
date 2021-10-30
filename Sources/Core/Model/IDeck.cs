using System.Collections.Generic;

namespace Core
{
    public interface IDeck
    {
        bool TryDraw(out Card card);

        void Add(IEnumerable<Card> cards);

        bool Any();

        int Count();
    }
}
