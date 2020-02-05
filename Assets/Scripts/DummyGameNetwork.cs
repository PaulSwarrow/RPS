using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class DummyGameNetwork : IGameNetwork
    {
        public event Action<Squad, int, Vector3Int, float> SpawnEvent;

        public event Action<int, int> CollisionEvent;
        public event Action<int> ReachEvent;
        public event Action GameStartEvent;
        public event Action<int> GameEndEvent;
        public event Action Disconnected;


        public void Start()
        {
            GameStartEvent?.Invoke();
        }

        public void Kill()
        {
        }

        public void Collision(int entityId, int id)
        {
            CollisionEvent?.Invoke(entityId, id);
        }

        public event Action<float> TimestampEvent;
        public void Timestamp(float timestamp)
        {
            TimestampEvent?.Invoke(timestamp);
        }

        public void OnReachEdge(int entityId)
        {
            ReachEvent?.Invoke(entityId);
        }

        public void Spawn(Squad squad, int playerId, Vector3Int pos, float timestamp)
        {
            SpawnEvent(squad, playerId, pos, timestamp);
        }

        public void End(int winnerId)
        {
            GameEndEvent?.Invoke(winnerId);
        }
    }
}