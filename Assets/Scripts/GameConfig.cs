using System;
using System.Collections.Generic;
using DefaultNamespace;
using Utils.Dictionary;

namespace UnityEngine
{
    [Serializable]
    public class GameItemPreview : SerializableDictionary<GameItem.Type, Sprite>
    {
    }
    [Serializable]
    public class GameItemPrefabs : SerializableDictionary<GameItem.Type, GameItem>
    {
    }
    [Serializable]
    public class GameItemColor: SerializableDictionary<GameItem.Type, Color>
    {
    }
    
    [Serializable]
    public class Rules: SerializableDictionary<GameItem.Type, GameItem.Type>
    {
    }
    

    [CreateAssetMenu(fileName = "game config", menuName = "game config", order = 1)]
    public class GameConfig : ScriptableObject
    {
        public GameItemPreview itemIcon;
        public GameItemPrefabs itemPrefab;
        public GameItemColor itemColors;

        public Rules rules;

        [Range(0.01f, 2)]
        public float movementSpeed = 1;
        [Range(0, 20)]
        public float startStamina = 7;
        [Range(5, 20)]
        public float maxStamina = 7;
        [Range(0, 20)]
        public float staminaRestorePerSecond = 1;
        
        public int health = 100;
        public List<Squad> squadTemplates;

        [Range(0, 2)]
        public float CollisionDistance = 1;

        public Squad GenerateSquad()
        {
            Squad squad = new Squad();
            squad.type = (GameItem.Type) Random.Range(0, Enum.GetValues(typeof(GameItem.Type)).Length);
            Squad template = squadTemplates[Random.Range(0, squadTemplates.Count)];
            squad.items = template.items.GetRange(0, template.items.Count);
            return squad;
        }
    }
}