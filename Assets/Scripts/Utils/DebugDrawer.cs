using UnityEngine;

namespace Utils
{
    public class DebugDrawer
    {
        public static void DrawRect(Vector3 leftTop, Vector3 rightTop, Vector3 leftBottom, Vector3 rightBottom,
            Color color)
        {
            Debug.DrawLine(leftTop, rightTop, color);
            Debug.DrawLine(leftTop, leftBottom, color);
            Debug.DrawLine(rightTop, rightBottom, color);
            Debug.DrawLine(leftBottom, rightBottom, color);
        }
    }
}