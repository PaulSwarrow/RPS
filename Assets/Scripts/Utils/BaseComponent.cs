using UnityEngine;

namespace Utils
{
    public class BaseComponent : MonoBehaviour
    {
        protected void RequireComponent<T>(ref T component) where T : Component
        {
            if (!component)
            {
                if (!(component = GetComponent<T>()))
                {
                    component = gameObject.AddComponent<T>();
                }
            }
        }

        protected void RequireChild<T>(ref T child, string name = null) where T : Component
        {
            if (!child)
            {
                child = GetComponentInChildren<T>();
                if (!child)
                {
                    child = new GameObject().AddComponent<T>();
                    child.transform.parent = transform;
                    child.gameObject.name = name != null ? name : typeof(T).Name;
                }
            }
        }

        protected void RequireChildWithName<T>(ref T child, string name) where T : Component
        {
            if (child)
            {
                child.gameObject.name = name;
                return;
            }

            T[] list = transform.GetComponentsInChildren<T>();
            foreach (T item in list)
            {
                if (item.name == name)
                {
                    child = item;
                    return;
                }
            }

            if (!child)
            {
                child = new GameObject().AddComponent<T>();
                child.transform.parent = transform;
                child.gameObject.name = name;
            }
        }
    }
}