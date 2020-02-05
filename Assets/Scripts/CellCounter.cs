using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class CellCounter : MonoBehaviour
{
    // Start is called before the first frame update

    public int width;
    public int height;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Tilemap map = FindObjectOfType<Tilemap>();


        width = map.cellBounds.xMax - map.cellBounds.xMin;
        height = map.cellBounds.yMax - map.cellBounds.yMin;

    }
}
