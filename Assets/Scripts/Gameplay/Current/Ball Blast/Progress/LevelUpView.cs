using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Current.Ball_Blast.Progress
{
    public class LevelUpView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private RectTransform pulseTarget;
        [SerializeField] private Transform rayLight;

        private Tween _scaleTween;
        private Tween _rotateTween;

        private System.Action _onClick;

        public void Init(System.Action onClick)
        {
            _onClick = onClick;
            button.onClick.AddListener(Click);
        }

        public void Show()
        {
            gameObject.SetActive(true);

            KillTweens();

            pulseTarget.localScale = Vector3.one * 0.95f;

            _scaleTween = pulseTarget
                .DOScale(1.05f, 0.6f)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);

            _rotateTween = rayLight
                .DORotate(Vector3.forward * 360f, 3f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1);
        }

        public void Hide()
        {
            KillTweens();
            gameObject.SetActive(false);
        }

        private void Click()
        {
            _onClick?.Invoke();
        }

        private void KillTweens()
        {
            _scaleTween?.Kill();
            _rotateTween?.Kill();
        }

        private void OnDisable()
        {
            KillTweens();
        }
    }
}