using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GridLayoutScaler : MonoBehaviour
{
    private GridLayoutGroup layout;

    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        layout = GetComponent<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cellSize = layout.cellSize;
//        Rect rect = rectTransform.rec;
        
//        rect.min = new Vector2(0,rectTransform.parent0.2f);
//        rect.max = new Vector2(0,0.6f);
        cellSize.y =rectTransform.rect.height / layout.constraintCount;
        cellSize.x = cellSize.y;
        layout.cellSize = cellSize;
    }
}