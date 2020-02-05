using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class SquadGhost : MonoBehaviour
    {

        private Squad squad;


        private List<GameItem> items;
        private Vector3Int zero;
        public bool dropPossible;
        public bool cardAvailable;

        public Vector3Int position => zero;

        private void Awake()
        {
            items = new List<GameItem>();
        }


        public void SetSquad(Squad squad)
        {
            if (this.squad == squad)
            {
                return;
            }

            Clear();

            this.squad = squad;
            if (squad != null)
            {
                foreach (Vector2Int item in squad.items)
                {
                    GameItem entity = GameItem.Spawn(squad.type, zero, transform);
                    entity.state = GameItem.State.ghost;

                    items.Add(entity);
                }

                Update();
            }
        }

        private void Update()
        {
            if (squad != null)
            {
                
                zero = GameManager.instance.WorldToCell(transform.position);
                zero = GameTools.ClampSpawnPosition(squad, zero);

                dropPossible = true;
                for (int i = 0; i < squad.items.Count; i++)
                {
                    Vector3Int cell = zero;
                    cell.x += squad.items[i].x;
                    cell.y += squad.items[i].y * (int) GameManager.instance.user.direction.y;
                    GameItem item = items[i];
                    item.transform.position = GameManager.instance.GetCellCenterWorld(cell);

                    if (GameItemController.Find(cell).Count > 0)
                    {
                        dropPossible = false;
                        item.locked = true;
                    }
                    else
                    {
                        item.locked = !cardAvailable;
                    }
                }
            }
        }

        public void Clear()
        {
            foreach (GameItem item in items)
            {
                item.Remove();
            }
            items.Clear();
            squad = null;
        }
    }
}