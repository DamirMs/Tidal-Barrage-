using System;
using PT.Tools.EventListener;
using UniRx;

namespace Gameplay.Current.Ball_Blast
{
    public class GoldManager : MonoBehaviourEventListener
    {
        public ReactiveProperty<int> CurrentGold { get; private set; } = new(0);

        public event Action OnEnoughGoldReached;

        private const int GoldThreshold = 100;

        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted },
            });
        }
        
        public void AddGold(int amount)
        {
            CurrentGold.Value += amount;

            if (CurrentGold.Value >= GoldThreshold)
            {
                OnEnoughGoldReached?.Invoke();
            }
        }

        public void SpendGold(int amount)
        {
            CurrentGold.Value -= amount;
        }

        public bool CanAfford(int cost) => CurrentGold.Value >= cost;

        private void OnGameStarted()
        {
            CurrentGold.Value = 0;
        }
    }
}