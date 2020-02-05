using System.Collections.Generic;

namespace Utils.Dictionary
{
    public class DictionaryOfLists<TKey, TValue> : Dictionary<TKey, List<TValue>>
    {

        public List<TValue> this[TKey key]
        {
            get
            {
                if (!ContainsKey(key))
                {
                    base[key] = new List<TValue>();
                }

                return base[key];
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (!HasValue(key, value))
            {
                this[key].Add(value);
            }
        }

        public void Remove(TKey key, TValue value)
        {
            if (HasValue(key, value))
            {
                this[key].Remove(value);
            }
        }

        public bool HasValue(TKey key, TValue value)
        {
            return this[key].Contains(value);
        }
    }
}