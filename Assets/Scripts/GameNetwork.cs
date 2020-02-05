using System;
using DefaultNamespace;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameNetwork : Object, IGameNetwork
{
    public event Action<Squad, int, Vector3Int, float> SpawnEvent;
    public event Action<int, int> CollisionEvent;
    public event Action<int> ReachEvent;
    public event Action GameStartEvent;
    public event Action<int> GameEndEvent;
    public event Action<float> TimestampEvent;
    public event Action Disconnected;


    public GameNetwork()
    {
    }

    public void Start()
    {
        PhotonNetwork.OnEventCall += OnEvent;
        Send(0, PhotonNetwork.player.ID);
    }

    public void Kill()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.OnEventCall -= OnEvent;
    }

    public void Timestamp(float timestamp)
    {
        Send(5, timestamp);
    }


    [Serializable]
    public class SpawnEventData
    {
        public Squad squad;
        public int player;
        public Vector3Int pos;
        public float timestamp;
    }

    public void Spawn(Squad squad, int playerId, Vector3Int pos, float timestamp)
    {
        SpawnEventData data = new SpawnEventData
        {
            squad = squad,
            player = playerId,
            pos = pos,
            timestamp = timestamp
        };
        Send(1, JsonUtility.ToJson(data));
//        SpawnEvent?.Invoke(squad, playerId, from);
    }


    public void Collision(int entityId, int opponentId)
    {
        int[] data = new int[2];
        data[0] = entityId;
        data[1] = opponentId;
        Send(2, data);
//        CollisionEvent?.Invoke(entityId, opponentId);
    }

    public void OnReachEdge(int entityID)
    {
        Send(3, entityID);
//        ReachEvent?.Invoke(entityID);
    }

    public void End(int winnerId)
    {
        Send(4, winnerId);
    }


    void Send<T>(byte evCode, T data)
    {
        bool reliable = true;

        RaiseEventOptions
            raiseEventOptions = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.All
            }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(evCode, data, reliable, raiseEventOptions);


//        int newCounter = Int32.Parse(counter.text) + 1;
//        counter.text = newCounter.ToString();
    }

    private void OnEvent(byte eventCode, object content, int senderId)
    {
        switch (eventCode)
        {
            case 0: //READY
                switch (PhotonNetwork.room.PlayerCount)
                {
                    case 1:
                        Log.m("Wait for opponent");
                        break;
                    case 2:
                        Log.m("Room is ready for game");
                        GameStartEvent?.Invoke();
                        break;
                    default:
                        Log.m("Room is full :(");
                        Disconnected?.Invoke();
                        break;
                }

                break;
            case 1: //spawn
                SpawnEventData spawnData = JsonUtility.FromJson<SpawnEventData>((string) content);
                SpawnEvent?.Invoke(spawnData.squad, spawnData.player, spawnData.pos, spawnData.timestamp);
                break;

            case 2: //collision
                int[] collisionData = (int[]) content;
                CollisionEvent?.Invoke(collisionData[0], collisionData[1]);
                break;
            case 3: // reach
                ReachEvent?.Invoke((int) content);
                break;
            case 4: //end
                GameEndEvent?.Invoke((int) content);
                break;
            case 5: //timestamp
                TimestampEvent?.Invoke((float) content);
                break;
        }

        // Do something
    }
}