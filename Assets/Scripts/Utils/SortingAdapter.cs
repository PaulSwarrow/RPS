using UnityEngine;
using UnityEngine.Rendering;

namespace Utils
{
    public interface ISortAdapter
    {
        int sortingOrder { set; get; }
        void destroy();
    }

    public class SortingAdapter
    {
        public static ISortAdapter create(GameObject gameObject)
        {
            SortingGroup group = gameObject.GetComponent<SortingGroup>();
            if (group)
            {
                return new GroupAdapter(group);
            }

            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            if (renderer)
            {
                return new SpriteRendererAdapter(renderer);
            }

            return null;
        }
    }

    public class SpriteRendererAdapter : ISortAdapter
    {
        private SpriteRenderer target;

        public SpriteRendererAdapter(SpriteRenderer target)
        {
            this.target = target;
        }

        public int sortingOrder
        {
            get { return target.sortingOrder; }
            set { target.sortingOrder = value; }
        }

        public void destroy()
        {
            target = null;
        }
    }

    public class GroupAdapter : ISortAdapter
    {
        private SortingGroup target;

        public GroupAdapter(SortingGroup target)
        {
            this.target = target;
        }

        public int sortingOrder
        {
            get { return target.sortingOrder; }
            set { target.sortingOrder = value; }
        }

        public void destroy()
        {
            target = null;
        }
    }
}