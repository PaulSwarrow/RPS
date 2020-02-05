using System;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IGameNetwork
    {
        event Action<Squad, int, Vector3Int, float> SpawnEvent;
        event Action<int, int> CollisionEvent;
        event Action<int> ReachEvent;
        event Action GameStartEvent;
        event Action<int> GameEndEvent;
        event Action<float> TimestampEvent;
        event Action Disconnected;
        
        void Start();
        void Spawn(Squad squad, int playerId, Vector3Int pos, float timestamp);
        void Collision(int entityId, int id);
        void OnReachEdge(int entityId);
        void End(int winnerId);
        void Kill();
        void Timestamp(float timestamp);
    }
}
