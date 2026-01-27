using System;
using System.Collections.Generic;
using Gameplay.Current.Ball_Blast.Interactables;
using NaughtyAttributes;
using PT.Tools.Helper;
using UnityEngine;

namespace Gameplay.Current.Ball_Blast.Difficulty
{
    [Serializable]
    public class DifficultyInfo
    {
        [SerializeField] private SerializableKeyValue<InteractableTypeEnum, float> interactableSpawnChances;
        [SerializeField] 
        // [MinMaxSlider(0.5f, 1)] 
        private float spawnChance;
        [SerializeField] private int maxActiveAtOnce = 4;
        [SerializeField] [MinMaxSlider(0.1f, 4)] private Vector2 spawnInterval = new(0.1f, 0.5f);

        public Dictionary<InteractableTypeEnum, float> InteractableSpawnChances => interactableSpawnChances.Dictionary;
        public float SpawnChance => spawnChance;
        public int MaxActiveAtOnce => maxActiveAtOnce;
        public Vector2 SpawnInterval => spawnInterval;
    }
}