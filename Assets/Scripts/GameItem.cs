using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Utils.Dictionary;

namespace DefaultNamespace
{
    public class
        GameItem : MonoBehaviour
    {
        public static int counter;

        public static DictionaryOfLists<Type, GameItem> pool = new DictionaryOfLists<Type, GameItem>();


       
        
        public static GameItem Spawn(Type type, Vector3Int cell, Transform layer)
        {
            GameItem item;
            Vector3 position = GameManager.instance.GetCellCenterWorld(cell);
            if (pool[type].Count > 0)
            {
                item = pool[type][0];
                pool[type].RemoveAt(0);
                item.transform.position = position;
                item.transform.parent = layer;
                item.gameObject.SetActive(true);
            }
            else
            {
                item = Instantiate(GameManager.instance.config.itemPrefab[type],
                    position,
                    Quaternion.identity,
                    layer);
                item.type = type;
                item.name = type.ToString();
            }
            
            item.animator = item.GetComponentInChildren<Animator>();
            item.sortGroup = item.GetComponent<SortingGroup>();
//            renderer = GetComponentInChildren<SpriteRenderer>();
            item.controller = item.GetComponent<GameItemController>();
//            item.renderer.color = Color.white;
            item.controller.enabled = false;


            return item;
        }


        public enum Type
        {
            Archer,
            Dragon,
            Knight
        }


        public enum State
        {
            ghost,
            ingame,
            pool
        }

        public State state;


        public Type type;
        public int playerId;
        public bool locked;

//        [HideInInspector] public SpriteRenderer renderer;
        [HideInInspector] public GameItemController controller;

        public int id;

        private Vector3Int lastCell;
        public float bornTime;
        public float timestamp;
        private SortingGroup sortGroup;
        private Animator animator;

        private Coroutine coroutine;

        [SerializeField] private Transform body;
        private SpriteRenderer[] bodyParts;
        [SerializeField]
        private Color color = Color.white;
        
        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            sortGroup = GetComponent<SortingGroup>();
//            renderer = GetComponentInChildren<SpriteRenderer>();
            controller = GetComponent<GameItemController>();
            Debug.Log(controller);

            bodyParts = body.GetComponentsInChildren<SpriteRenderer>(true);
        }

        private void OnEnable()
        {
            animator.SetTrigger("reset");
        }

        public void Win()
        {
            animator.SetTrigger("strike");
        }

        public void Die(bool byStronger, Action<GameItem> callback)
        {
            state = State.pool;
            coroutine = StartCoroutine(DeathCoroutine(byStronger, callback));
        }

        private IEnumerator DeathCoroutine(bool byStronger, Action<GameItem> onComplete)
        {
            animator.SetTrigger(byStronger ? "dead_by_stronger" : "dead_by_equal");
            yield return new WaitForSeconds(0.3f);
            onComplete.Invoke(this);
        }

        public void Remove()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            state = State.pool;
            controller.enabled = false;
            gameObject.SetActive(false);
            pool[type].Add(this);
        }


        private void Update()
        {
            if (state == State.ingame)
            {
                SetColor(Color.white *(GameManager.instance.user.id == playerId ? 1 : 0.9f));
                sortGroup.sortingLayerName = "game";
                controller.enabled = true;
                sortGroup.sortingOrder = -Mathf.RoundToInt(transform.position.y * 200);
            }
            else
            {
                SetColor(locked ? Color.white * 0.5f : Color.white);
                if (state == State.ghost)
                {
                    sortGroup.sortingLayerName = "ghost";
                }

                controller.enabled = false;
            }
        }

        private void SetColor(Color color)
        {
            color.a = state == State.ghost ? 0.6f : 1;
            if (this.color != color)
            {
                this.color = color;
                foreach (SpriteRenderer part in bodyParts)
                {

                    part.color = color;
                }
            }
        }

        public Vector3Int cell => GameManager.instance.WorldToCell(transform.position);
    }
}