using DefaultNamespace;

namespace UnityEngine
{
    public class GameTools: MonoBehaviour
    {
        public static Vector3Int ClampSpawnPosition(Squad squad, Vector3Int pos)
        {
            MapLayer map = FindObjectOfType<MapLayer>();

            pos.x = Mathf.Clamp(pos.x, -squad.Left, map.size.x - squad.Right - 1);
            return pos;
        }
    }
}