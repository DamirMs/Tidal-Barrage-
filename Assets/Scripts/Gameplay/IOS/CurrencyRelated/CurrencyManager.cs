using System;
using System.Collections.Generic;
using Gameplay.IOS.Other;
using UniRx;
using UnityEngine;
using IInitializable = Zenject.IInitializable;

namespace Gameplay.IOS.CurrencyRelated
{
    public enum CurrencyType
    {
        Gold,
    }

    public class CurrencyManager : IInitializable, IDisposable
    {
        private const string SaveKeyPrefix = "Currency_";

        private readonly Dictionary<CurrencyType, ReactiveProperty<ValueChange>> _balances = new();

        public void Initialize()
        {
            foreach (CurrencyType type in Enum.GetValues(typeof(CurrencyType)))
            {
                int value = PlayerPrefs.GetInt($"{SaveKeyPrefix}{type}", 0);

                var reactive = new ReactiveProperty<ValueChange>(new ValueChange(value, 0, ValueChangeType.Set));

                reactive.Subscribe(v =>
                {
                    PlayerPrefs.SetInt($"{SaveKeyPrefix}{type}", v.Value);
                });

                _balances[type] = reactive;
            }
        }

        public void Dispose()
        {
            foreach (var kvp in _balances)
            {
                PlayerPrefs.SetInt($"{SaveKeyPrefix}{kvp.Key}", kvp.Value.Value.Value);
            }

            PlayerPrefs.Save();
        }

        public ReactiveProperty<ValueChange> GetReactive(CurrencyType type) => _balances[type];
        public int Get(CurrencyType type) => _balances[type].Value.Value;

        public void Set(CurrencyType type, int value)
        {
            int prev = _balances[type].Value.Value;
            _balances[type].Value = new ValueChange(value, prev, ValueChangeType.Set);
        }

        public void Add(CurrencyType type, int amount)
        {
            int prev = _balances[type].Value.Value;

            _balances[type].Value = new ValueChange(prev + amount, prev, ValueChangeType.Add);
        }

        public bool TrySpend(CurrencyType type, int amount)
        {
            int prev = _balances[type].Value.Value;
            if (prev < amount) return false;

            _balances[type].Value = new ValueChange(prev - amount, prev, ValueChangeType.Substract);

            return true;
        }
    }
}