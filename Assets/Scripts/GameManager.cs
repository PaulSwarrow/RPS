using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private FieldLayer layer;

    private static GameManager manager;


    public static GameManager instance
    {
        get
        {
            if (!manager)
            {
                manager = FindObjectOfType<GameManager>();
            }

            return manager;
        }
    }

    public PlayerProfile GetPlayer(int id)
    {
        return id == 0 ? playerOne : playerTwo;
    }

    public PlayerProfile GetEnemy(int id)
    {
        return id == 1 ? playerOne : playerTwo;
    }

    public GameConfig config;

    public PlayerProfile playerOne;
    public PlayerProfile playerTwo;

    public Grid grid;
    private MapLayer map;
    public SquadPaletteController inventory;
    private AIEnemy bot;
    
    private bool host;
    private IGameNetwork network;
    
    public event Action GameStartEvent;
    public event Action<bool> GameEndEvent;
    public event Action GameDisconnectEvent;
    public event Action<PlayerProfile> HitEvent; 


    public PlayerProfile user => host ? playerOne : playerTwo;

    public PlayerProfile opponent => host ? playerTwo : playerOne;

    // Start is called before the first frame update
    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
        map = FindObjectOfType<MapLayer>();
        layer = GetComponentInChildren<FieldLayer>();
        inventory = FindObjectOfType<SquadPaletteController>();
    }

    public void StartSoloGame()
    {
        host = true;
        bot = GetComponent<AIEnemy>();
        StartGame(new DummyGameNetwork());
        bot.OnGameStart();
    }

    public void StartPvPGame(bool host)
    {
        this.host = host;
        StartGame(new GameNetwork());
    }

    private void StartGame(IGameNetwork network)
    {
        hostTimestamp = 0;
        playerOne.direction = Vector2.up;
        playerTwo.direction = Vector2.down;

        playerOne.id = 0;
        playerTwo.id = 1;
        playerOne.Reset();
        playerTwo.Reset();

        this.network = network;
        this.network.SpawnEvent += OnSpawnConfirmed;
        this.network.CollisionEvent += OnCollisionConfirmed;
        this.network.ReachEvent += OnReachConfirmed;
        this.network.GameStartEvent += OnGameStarted;
        this.network.TimestampEvent += OnTimeUpdate;
        this.network.GameEndEvent += OnEndGameConfirmed;
        this.network.Disconnected += OnDisconnected;

        this.network.Start();
        network.Timestamp(hostTimestamp);
    }

    private void OnGameStarted()
    {
        gameIsRunning = true;
        GameStartEvent?.Invoke();
        inventory.OnGameStart();
    }

    private void CheckGameState()
    {
        if (host)
        {
            if (playerOne.health <= 0)
            {
                network.End(1);
            }
            else if (playerTwo.health <= 0)
            {
                network.End(0);
            }
        }
    }

    private Dictionary<int, GameItem> items = new Dictionary<int, GameItem>();
    private bool gameIsRunning;
    private float hostTimestamp;
    private float gameTimestamp;

    private void Update()
    {
        if (gameIsRunning)
        {
            RestoreStamina(playerOne);
            RestoreStamina(playerTwo);

            if (host)
            {
                hostTimestamp += Time.deltaTime;
                network.Timestamp(hostTimestamp);
            }
        }
    }

    private void OnTimeUpdate(float timestamp)
    {
        gameTimestamp = timestamp;
        foreach (KeyValuePair<int, GameItem> item in items)
        {
            item.Value.timestamp = timestamp - item.Value.bornTime;
        }
    }

    private void RestoreStamina(PlayerProfile player)
    {
        player.stamina += config.staminaRestorePerSecond * Time.deltaTime;
        player.stamina = Math.Min(player.stamina, config.maxStamina);
    }

    private void OnEndGameConfirmed(int winnerID)
    {
        gameIsRunning = false;
        network.Kill();
        if (bot)
        {
            bot.OnGameStop();
            bot = null;
        }
        bool winner = winnerID == user.id;
        GameEndEvent?.Invoke(winner);
    }

    private void OnDisconnected()
    {
        gameIsRunning = false;
        network.Kill();
        KillGame();
        GameDisconnectEvent?.Invoke();
    }

    public void KillGame()
    {
        if (network != null)
        {
            inventory.OnGameEnd();
            network = null;

            foreach (KeyValuePair<int, GameItem> item in items)
            {
                item.Value.Remove();
            }

            items.Clear();
        }
    }


    public void Spawn(Squad squad, int playerId, Vector3Int from)
    {
        if(!gameIsRunning) return;
        from = GameTools.ClampSpawnPosition(squad, from);
        network.Spawn(squad, playerId, from, gameTimestamp);
    }

    private void OnSpawnConfirmed(Squad squad, int playerId, Vector3Int from, float timestamp)
    {
        for (int i = 0; i < squad.items.Count; i++)
        {
            Vector3Int cell = from;
            cell.x += squad.items[i].x;
            cell.y += squad.items[i].y * (int) GetPlayer(playerId).direction.y;


            GameItem entity = GameItem.Spawn(squad.type, cell, layer.transform);
            entity.bornTime = timestamp;
            entity.timestamp = 0;
            entity.playerId = playerId;
            entity.state = GameItem.State.ingame;
            entity.controller.movement = GetPlayer(playerId).direction;
            entity.id = GameItem.counter;
            GameItem.counter++;
            items[entity.id] = entity;

            Log.m("Spawn: " + entity.id);
        }
    }

    public void OnCollision(GameItem entity, GameItem opponent)
    {
        if(!gameIsRunning) return;
        
        OnCollisionConfirmed(entity.id, opponent.id);
        network.Collision(entity.id, opponent.id);
    }

    private void OnCollisionConfirmed(int who, int to)
    {
        GameItem entity = GetItem(who);
        GameItem opponent = GetItem(to);
        if (!entity || !opponent)
        {
            return; // solved
        }

        if (entity.type == opponent.type)
        {
            entity.Die(false, RemoveEntity);
            opponent.Die(false, RemoveEntity);
        }
        else if (config.rules[entity.type] == opponent.type)
        {
            entity.Win();
            opponent.Die(true, RemoveEntity);
        }
        else
        {
            entity.Die(true, RemoveEntity);
            opponent.Win();
        }
    }

    private void RemoveEntity(GameItem entity)
    {
        items.Remove(entity.id);
        entity.Remove();
    }


    public void OnReachEdge(GameItem entity)
    {
        if(!gameIsRunning) return;
        network.OnReachEdge(entity.id);
        OnReachConfirmed(entity.id);
    }

    public void OnReachConfirmed(int entityId)
    {
        GameItem entity = GetItem(entityId);
        if (entity)
        {
            GetEnemy(entity.playerId).health--;
            RemoveEntity(entity);
            HitEvent?.Invoke(GetEnemy(entity.playerId));
            CheckGameState();
        }
    }

    private GameItem GetItem(int id)
    {
        return items.ContainsKey(id) ? items[id] : null;
    }

    public Vector3Int WorldToCell(Vector3 position)
    {
        Vector3Int cell = grid.WorldToCell(position);
        if (!host)
        {
            cell.y = map.size.y - cell.y - 1;
        }

        return cell;
    }

    public Vector3 GetCellCenterWorld(Vector3Int cell)
    {
        if (!host)
        {
            cell.y = map.size.y - cell.y - 1;
        }

        return grid.GetCellCenterWorld(cell);
    }

    public bool IsHost()
    {
        return host;
    }

    public Vector2 realDirection(int playerId)
    {
        return GetPlayer(playerId).direction * (host ? 1 : -1);
    }
}