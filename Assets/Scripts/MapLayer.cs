using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class MapLayer : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector2Int size;

    private Tilemap tilemap;

    [SerializeField] private TileBase prefab;
    public Vector2 cellSize => tilemap.layoutGrid.cellSize;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        Draw();
    }

    private void Draw()
    {
        tilemap.ClearAllTiles();
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), prefab);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            Draw();
        }
    }
}