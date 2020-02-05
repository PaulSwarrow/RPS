using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class SquadCardButton : MonoBehaviour
{
    [HideInInspector]
    public Squad squad;

    // Start is called before the first frame update


    private Dictionary<Vector2Int, Image> cells;
    bool ispressed = false;

    public event Action<SquadCardButton> OnSelected;
    public event Action<SquadCardButton> OnDeselected;
    public event Action<SquadCardButton> OnActivate;


    private ScrollRect scroller;
    private Draggable draggable;

    public bool active;
    private CanvasController input;
    private PointerEventData touch;
    private RectTransform rectTransform;
    public Vector3 aimPoint;

    public Sprite emptySprite;
    public Sprite fillSprite;

    private Animator animator;
    [SerializeField]
    private Image icon;

    [SerializeField] private Image bg;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
        draggable = GetComponentInChildren<Draggable>();


        scroller = GetComponentInChildren<ScrollRect>();

        cells = new Dictionary<Vector2Int, Image>();
        Image[] list = GetComponentsInChildren<Image>(true);
        foreach (Image image in list)
        {
            string[] arr = image.name.Split('_');
            if (arr[0] == "cell")
                cells[new Vector2Int(Int32.Parse(arr[1]), Int32.Parse(arr[2]))] = image;
        }
    }

    public void OnGameStart()
    {
        draggable.DragStartEvent += Select;
        draggable.DragEndEvent += Deselect;
        draggable.DragUpdateEvent += DragUpdate;
    }

    public void OnGameEnd()
    {
        draggable.DragStartEvent -= Select;
        draggable.DragEndEvent -= Deselect;
        draggable.DragUpdateEvent -= DragUpdate;
    }

    private void Update()
    {
        icon.color = Available ? Color.white : Color.gray;
        bg.color = Available ? Color.white : Color.gray;
        animator.SetBool("active", active);

    }

    public bool Available => GameManager.instance.user.stamina >= squad.items.Count;

    public void show(Squad squad)
    {
        this.squad = squad;
        foreach (KeyValuePair<Vector2Int, Image> pair in cells)
        {
            pair.Value.sprite = emptySprite;
            pair.Value.color = GameManager.instance.config.itemColors[squad.type];
        }

        icon.sprite = GameManager.instance.config.itemIcon[squad.type];

        foreach (Vector2Int point in squad.items)
        {
            Vector2Int cell = new Vector2Int(-squad.Left, -squad.Bottom);
            cell += point;
            if (cells.ContainsKey(cell))
            {
                cells[cell].sprite = fillSprite;
                cells[cell].color = GameManager.instance.config.itemColors[squad.type];
            }
        }

        for (int i = 0; i < 4; i++)
        {
            cells[new Vector2Int(i, 0)].gameObject.SetActive(i < squad.width);
            cells[new Vector2Int(i, 1)].gameObject.SetActive(i < squad.width);
        }
    }


    private void Select(Vector2 vector2)
    {
        ispressed = true;
        
        OnSelected?.Invoke(this);
    }

    private void DragUpdate(Vector2 current, Vector2 from)
    {
        float delta = (current - from).y / rectTransform.rect.height;

        aimPoint = Camera.main.ScreenToWorldPoint(new Vector3(current.x, current.y));
        aimPoint.z = 0;

        bool activate = delta > 0.5f;

        if (activate && !active)
        {
            active = true;
        }

        if (active && !activate)
        {
            active = false;
        }
    }

    public void Deselect(Vector2 vector2)
    {
        ispressed = false;
        OnDeselected?.Invoke(this);
        active = false;
    }
}