﻿using UnityEngine;
using UnityEngine.Serialization;

namespace Logic.Settings
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "MyAsteroids/Settings/Create game settings")]
    public class GameSettings : ScriptableObject
    {
        public float SpawnEnemyCooldown;
        [FormerlySerializedAs("MaxCountEnemyes")]
        public int MaxCountEnemies;
    }
}