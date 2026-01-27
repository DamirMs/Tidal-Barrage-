using System.Collections.Generic;
using System.Linq;
using Gameplay.Current.Ball_Blast.Countables;
using Gameplay.Current.Ball_Blast.Interactables;
using PT.Tools.EventListener;
using PT.Tools.ObjectPool;
using UnityEngine;

namespace Gameplay.Current.Ball_Blast.InteractablesLabels
{
    public class InteractablesLabelsManager : MonoBehaviourEventListener
    {
        [SerializeField] private InteractableViewLabel labelPrefab;
        [SerializeField] private Transform labelsParent;
        
        private Dictionary<IInteractable, InteractableViewLabel> _interactablesLabels = new();
        
        private MonoBehPool<InteractableViewLabel> _interactablesLabelsPool = new();

        private List<KeyValuePair<IInteractable, InteractableViewLabel>> _interactablesLabelsToRemove;

        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted },
                { GlobalEventEnum.GameEnded, OnGameEnded },
            });
            
            _interactablesLabelsPool.Init(labelPrefab.gameObject, labelsParent, 100);
        }
        
        private void OnGameStarted()
        {
            RemoveAllLabels();
        }
        private void OnGameEnded()
        {
            RemoveAllLabels();
        }
        
        private void Update()
        {
            _interactablesLabelsToRemove = new();

            MoveLabels();
            RemoveLabels();
        }

        public void AddLabel(IInteractable interactable, CountableModel countableModel)
        {
            if (_interactablesLabels.ContainsKey(interactable)) return;

            var label = _interactablesLabelsPool.Get();
            label.Init(countableModel);
            
            _interactablesLabels.Add(interactable, label);
        }
        
        private void MoveLabels()
        {
            foreach (var interactableLabel in _interactablesLabels)
            {
                if (!interactableLabel.Key.Transform.gameObject.activeSelf)
                {
                    _interactablesLabelsToRemove.Add(interactableLabel);
                }
                else 
                {
                    interactableLabel.Value.transform.position = interactableLabel.Key.Transform.position;
                }
            }
        }
        private void RemoveLabels()
        {
            while (_interactablesLabelsToRemove.Count > 0)
            {
                var interactableLabel = _interactablesLabelsToRemove[0];
                
                _interactablesLabelsPool.Set(interactableLabel.Value);
                _interactablesLabels.Remove(interactableLabel.Key);
                _interactablesLabelsToRemove.RemoveAt(0);
            }
        }

        private void RemoveAllLabels()
        {
            _interactablesLabelsToRemove = _interactablesLabels.ToList();
            RemoveLabels();

            _interactablesLabels.Clear();
        }
    }
}