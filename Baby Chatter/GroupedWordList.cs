using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using BabyChatterData;
using Baby_Chatter;

namespace Baby_Chatter
{
    /*
     * A list with an associated key object.
     */
    public class KeyedList<TKey, TList> : List<Word>
    {
        public KeyedList(TKey key, IEnumerable<Word> items) : base(items)
        {
            Key = key;
        }

        private TKey _Key;
        public TKey Key{ protected set; get;}
    }
}
