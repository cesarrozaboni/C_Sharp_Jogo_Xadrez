using System.Collections.Generic;

namespace Jogo_Xadrez.Util
{
    public class ResultInfo<T> : Result
    {
        public T Item { get; set; }
        public IList<T> Items { get; set; }
    }
}
