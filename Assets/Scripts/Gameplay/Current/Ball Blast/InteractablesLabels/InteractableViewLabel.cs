using System;
using Gameplay.Current.Ball_Blast.Countables;
using TMPro;
using UniRx;
using UnityEngine;

namespace Gameplay.Current.Ball_Blast.InteractablesLabels
{
    public class InteractableViewLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private IDisposable _disposable;
        
        public void Init(CountableModel countableModel)
        {
            _disposable = countableModel.CurrentValue
                .Subscribe(value => text.text = value.ToString());
            
            text.text = countableModel.CurrentValue.Value.ToString();
        }
        
        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}