using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public class EventDispatcher<TE, TD>
    {
        private Dictionary<TE, List<Action<TD>>> dictionary;

        public EventDispatcher()
        {
            dictionary = new Dictionary<TE, List<Action<TD>>>();
        }
        public void addListener(TE e, Action<TD> listener)
        {
            List<Action<TD>> list;
            if(dictionary.TryGetValue(e, out list) == false)
            {
                list = new List<Action<TD>>();
                dictionary.Add(e, list);
            }
            list.Add(listener);
        }
        public void removeListener(TE e , Action<TD> listener)
        {
            List<Action<TD>> list = null;
            if (dictionary.TryGetValue(e, out list))
            {
                list.Remove(listener);
            }
        }

        public void triggerEvent(TE e, TD data)
        {
            List<Action<TD>> list;
            if (dictionary.TryGetValue(e, out list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i](data);
                }
            }
        }
    }
}