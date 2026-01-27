using Gameplay.IOS.CurrencyRelated;
using Gameplay.IOS.Other;
using PT.Tools.EventListener;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.General.Score
{
    public class ScoreManager : MonoBehaviourEventListener
    {
        public ReactiveProperty<ValueChange> TotalScoreReactive { get; private set; } = new(new ValueChange(0, 0, ValueChangeType.Set));

        public ReactiveProperty<int> BestScoreReactive { get; private set; } = new();

        [Inject] private ScoreMultiplier _multiplier;
        [Inject] private CurrencyManager _currencyManager;

        private int _bestScore;

        private int BestScore
        {
            get => _bestScore;
            set
            {
                _bestScore = value;
                BestScoreReactive.Value = value;
                PlayerPrefs.SetInt("BestScore", value);
            }
        }

        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted },
                { GlobalEventEnum.GameEnded, OnGameFinished },
            });
        }

        private void Start()
        {
            _bestScore = PlayerPrefs.GetInt("BestScore", 0);
            BestScoreReactive.Value = _bestScore;
        }

        protected virtual void OnGameStarted()
        {
            ResetScore();
        }

        private void OnGameFinished()
        {
            if (TotalScoreReactive.Value.Value > BestScore) BestScore = TotalScoreReactive.Value.Value;
        }

        public virtual void UpdateScore(int value)
        {
            // _currencyManager.Add(CurrencyType.Gold, 1);
            var add = Mathf.RoundToInt(value * _multiplier.Current);
            var prev = TotalScoreReactive.Value.Value;

            TotalScoreReactive.Value = new ValueChange(prev + add, prev, ValueChangeType.Add);
        }

        protected virtual void ResetScore()
        {
            TotalScoreReactive.Value = new ValueChange(0, TotalScoreReactive.Value.Value, ValueChangeType.Set);
        }
    }
}
