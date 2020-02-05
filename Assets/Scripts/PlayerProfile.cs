using System;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class PlayerProfile
    {
        public int id;
        
        public int health = 10;
        public float stamina;
        public Vector2 direction;

        public void Reset()
        {
            GameConfig config = GameManager.instance.config;
            health = config.health;
            stamina = config.startStamina;
        }
    }
}