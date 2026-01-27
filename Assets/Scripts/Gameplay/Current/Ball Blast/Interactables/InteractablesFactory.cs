using System;
using System.Collections.Generic;
using Gameplay.Current.Ball_Blast.Bullets;
using Gameplay.Current.Ball_Blast.Countables;
using Gameplay.Current.Ball_Blast.Difficulty;
using Gameplay.Current.Configs;
using PT.Tools.EventListener;
using PT.Tools.Helper;
using PT.Tools.ObjectPool;
using UnityEngine;
using Zenject;

namespace Gameplay.Current.Ball_Blast.Interactables
{
    public class InteractablesFactory : MonoBehaviourEventListener
    {
        [SerializeField] private SerializableKeyValue<InteractableTypeEnum, InteractableObjectData> interactablesObjectsDatas;
        [Serializable]
        class InteractableObjectData
        {
            [SerializeField] private GameObject prefab;
            [SerializeField] private int amount;
            
            public GameObject Prefab => prefab;
            public int Amount => amount;
        }
        
        [SerializeField] private Transform interactablesParent;

        [Inject] private GameInfoConfig _gameInfoConfig;
        
        private Dictionary<InteractableTypeEnum, ObjectPool> _interactablesPools = new();
        
        private float _spawnCount = 0;
        
        private void Awake()
        {
            foreach (var interactablePrefab in interactablesObjectsDatas.Dictionary)
            {
                _interactablesPools.Add(interactablePrefab.Key, new());
                _interactablesPools[interactablePrefab.Key].Init(interactablePrefab.Value.Prefab, interactablesParent, interactablePrefab.Value.Amount);
            }
            
            AddEventActions(new Dictionary<GlobalEventEnum, Action>
            {
                { GlobalEventEnum.GameStarted, OnGameStarted },
            });
        }

        public IInteractable Spawn(
            InteractableTypeEnum type, Action<IInteractable, Bullet> onInteracted, 
            Vector2 position, DifficultyInfo difficultyInfo)
        {
            if (!_interactablesPools.ContainsKey(type))
            {
                DebugManager.Log(DebugCategory.Errors, $"Interactable type {type} not found in pools.", LogType.Error); 
                return null;
            }

            var interactableObj = _interactablesPools[type].Get();
            interactableObj.transform.position = position;
            
            var interactable = interactableObj.GetComponent<IInteractable>();

            if (_gameInfoConfig.InteractablesCountableDefaultValues.TryGetValue(type, out var defaultRange))
            {
                var countable = interactable.Transform.gameObject.GetComponent<ICountable>();

                var value = (int)((float)defaultRange.GetRandomValue() * _gameInfoConfig.InteractablesCountableCoefficients[type]);
                value += (int)_spawnCount;
                
                countable.Init(
                    value,
                    value);
            }

            interactable.Init(type, onInteracted);
            
            _spawnCount += 0.1f;

            return interactable;
        }
        
        public void Return(IInteractable interactable)
        {
            if (!_interactablesPools.ContainsKey(interactable.Type))
            {
                DebugManager.Log(DebugCategory.Errors, $"Interactable type {interactable.Type} not found in pools.", LogType.Error); 
                return;
            }

            _interactablesPools[interactable.Type].Set(interactable.Transform.gameObject);
        }

        private void OnGameStarted()
        {
            _spawnCount = 0;
        }
    }
}