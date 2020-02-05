using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class Squad
    {
        public GameItem.Type type;
        public List<Vector2Int> items = new List<Vector2Int>();

        public int Left
        {
            get
            {
                if (items.Count == 0)
                {
                    return 0;
                }

                int min = items[0].x;
                for (int i = 1; i < items.Count; i++)
                {
                    if (items[i].x < min)
                    {
                        min = items[i].x;
                    }
                }

                return min;
            }
        }

        public int Right
        {
            get
            {
                if (items.Count == 0)
                {
                    return 0;
                }

                int max = items[0].x;
                for (int i = 1; i < items.Count; i++)
                {
                    if (items[i].x > max)
                    {
                        max = items[i].x;
                    }
                }

                return max;
            }
        }

        public int Bottom
        {
            get
            {
                if (items.Count == 0)
                {
                    return 0;
                }

                int min = items[0].y;
                for (int i = 1; i < items.Count; i++)
                {
                    if (items[i].y < min)
                    {
                        min = items[i].y;
                    }
                }

                return min;
            }
        }

        public int Top
        {
            get
            {
                if (items.Count == 0)
                {
                    return 0;
                }

                int max = items[0].y;
                for (int i = 1; i < items.Count; i++)
                {
                    if (items[i].y > max)
                    {
                        max = items[i].y;
                    }
                }

                return max;
            }
        }

        public int width => items.Count > 0 ? 1 + Right - Left : 0;
        public int height => items.Count > 0 ? 1 + Top - Bottom : 0;
    }
}