using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Current.Ball_Blast
{
    public class GoldView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        
        [Inject] private GoldManager _goldManager;

        private CompositeDisposable _disposables = new(); 
        
        private void OnEnable()
        {
            _goldManager.CurrentGold
                .Subscribe(UpdateText)
                .AddTo(this);
        }
        private void OnDisable()
        {
            _disposables.Clear();
        }

        private void UpdateText(int value)
        {
            text.text = value.ToString();
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}