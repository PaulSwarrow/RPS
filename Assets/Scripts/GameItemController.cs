using System.Collections.Generic;
using UnityEngine;
using Utils.Dictionary;

namespace DefaultNamespace
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class GameItemController : MonoBehaviour
    {
        public static List<GameItem> Find(Vector3Int cell)
        {
            return cellsMap[cell];
        }

        private static readonly DictionaryOfLists<Vector3Int, GameItem> cellsMap =
            new DictionaryOfLists<Vector3Int, GameItem>();

        private GameItem entity;

        public Vector2 movement;

        private Vector2 startPosition;
        private MapLayer map;
        private Vector3Int lastCell;
        private BoxCollider2D collider;

        

        private void Awake()
        {
            collider = GetComponent<BoxCollider2D>();
            map = FindObjectOfType<MapLayer>();
            entity = GetComponent<GameItem>();
            collider.size = map.cellSize;
        }

        private void OnEnable()
        {
            cellsMap[entity.cell].Add(entity);
            lastCell = entity.cell;
            startPosition = transform.position;
            collider.enabled = true;
        }

        private void OnDisable()
        {
            collider.enabled = false;
            cellsMap[lastCell].Remove(entity);
        }


        private void Update()
        {
//            transform.Translate(GameManager.instance.realDirection(entity.playerId) * Time.deltaTime * speed);

            Vector3 tartetPos = startPosition + entity.timestamp * direction * speed;
            transform.position = Vector3.Lerp(transform.position, tartetPos, 10 * Time.deltaTime);
            if ((movement.y > 0 && entity.cell.y >= map.size.y) || (movement.y < 0 && entity.cell.y < 0))
            {
                GameManager.instance.OnReachEdge(entity);
            }

//            Vector3Int aimCell = entity.cell;
//            aimCell.y += (int) GameManager.instance.GetPlayer(entity.playerId).direction.y;

            if (entity.state == GameItem.State.ingame)
            {
                Ray2D ray = new Ray2D(transform.position, GameManager.instance.realDirection(entity.playerId));
                float distance = GameManager.instance.config.CollisionDistance * map.cellSize.y;
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, distance);
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance);
                if (hit.collider != null)
                {
                    OnCollide(hit.collider);
                }
            }
        }

        private void OnCollide(Collider2D collider)
        {
            GameItem enemy = collider.GetComponent<GameItem>();
            if (enemy.playerId == entity.playerId)
            {
                return;
            }

            GameManager.instance.OnCollision(entity, enemy);
        }

        private void LateUpdate()
        {
            if (lastCell != entity.cell)
            {
                cellsMap[lastCell].Remove(entity);
                cellsMap[entity.cell].Add(entity);
                lastCell = entity.cell;
            }
        }

        public Vector2 direction => GameManager.instance.realDirection(entity.playerId);

        public float speed => GameManager.instance.config.movementSpeed;
        /*private void OnCollisionEnter2D(Collision2D other)
        {
            GameItem enemy = other.collider.GetComponent<GameItem>();
            if (enemy.playerId == entity.playerId)
            {
                return;
            }

            GameManager.instance.OnCollision(entity, enemy);
        }*/
    }
}