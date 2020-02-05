using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class AIEnemy : MonoBehaviour
{
    public int playerId;

    public float interval = 2.5f;

    private MapLayer map;

    private Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        map = FindObjectOfType<MapLayer>();
    }

    public void OnGameStart()
    {
        coroutine = StartCoroutine(Work());
    }


    public void OnGameStop()
    {
        StopCoroutine(coroutine);
    }


    public IEnumerator Work()
    {
        while (GameManager.instance.GetPlayer(playerId).health > 0)
        {

            Squad squad = GameManager.instance.config.GenerateSquad();
            if (GameManager.instance.playerTwo.stamina >= squad.items.Count)
            {
                Vector3Int cell = new Vector3Int(Random.Range(0, map.size.x - 1), map.size.y - 1, 0);
                GameManager.instance.Spawn(squad, playerId, cell);
            }
            yield return new WaitForSeconds(interval);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}