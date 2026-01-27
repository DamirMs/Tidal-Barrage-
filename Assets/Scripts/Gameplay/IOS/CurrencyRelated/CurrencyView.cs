using Gameplay.IOS.Animations;
using Gameplay.IOS.Other;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.IOS.CurrencyRelated
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private CurrencyType type;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private FloatingDeltaTexts deltaTexts;

        [Inject] private CurrencyManager _currencyManager;

        private CompositeDisposable _disposable = new();
        
        private void OnEnable()
        {
            _currencyManager.GetReactive(type)
                .Subscribe(OnScoreChanged)
                .AddTo(_disposable);
        }
        
        private void OnScoreChanged(ValueChange valueChange)
        {
            text.text = valueChange.Value.ToString();

            switch (valueChange.Type)
            {
                case ValueChangeType.Add: deltaTexts.PlayAdd(valueChange.Value - valueChange.PrevValue); break;
                case ValueChangeType.Substract: deltaTexts.PlaySubtract(-(valueChange.Value - valueChange.PrevValue)); break;
            }
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}