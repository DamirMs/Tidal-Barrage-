using System;
using UniRx;
using UnityEngine;

namespace Gameplay.Current.Ball_Blast.Countables
{
    public class CountableModel
    {
        public IReadOnlyReactiveProperty<int> CurrentValue => _currentValue;
        public int MaxValue => _maxValue;
        
        private readonly ReactiveProperty<int> _currentValue = new();
        
        private int _maxValue;
        
        public CountableModel(int maxValue)
        {
            _maxValue = maxValue;
        }
        
        public void Set(int value)
        {
            if (value < 0 || value > _maxValue)
                throw new System.ArgumentOutOfRangeException(nameof(value), $"Value must be between 0 and {_maxValue}");
            
            _currentValue.Value = value;
        }
        public void Decrease(float amount = 1)
        {
            if (_currentValue.Value > 0) _currentValue.Value = (int)(Mathf.Max(0, (float)_currentValue.Value - Mathf.Abs(amount)));
        }
    }
}