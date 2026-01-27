using System;
using DG.Tweening;
using Gameplay.Current.Ball_Blast.Bullets;
using Gameplay.Current.Ball_Blast.Countables;
using Gameplay.Current.Ball_Blast.Interactables;
using UniRx;
using UnityEngine;

namespace Gameplay.Current.Ball_Blast.Balls
{
    public class Ball : MonoBehaviour, IInteractable, ICountable
    {
        [Header("Physics")]
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private CircleCollider2D col;
        [SerializeField] private float fallForce = 0.6f;
        [SerializeField] private float maxFallSpeed = 2.5f;
        [SerializeField] private float pushOnBulletHitPower = 1.5f;

        [Header("Visual")]
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private Color minColor = Color.green;
        [SerializeField] private Color maxColor = Color.red;

        [Header("Bounce")]
        [SerializeField] private float bounceScale = 0.2f;
        [SerializeField] private float bounceDuration = 0.15f;

        public Transform Transform => transform;
        public InteractableTypeEnum Type { get; private set; }
        public CountableModel CountableModel { get; private set; }

        private Action<IInteractable, Bullet> _onInteracted;
        private IDisposable _valueSub;

        private Vector2 _initialScale;

        private Tween _scaleTween;

        #region Init

        private void Awake()
        {
            _initialScale = transform.localScale;
        }
        
        public void Init(InteractableTypeEnum type, Action<IInteractable, Bullet> onInteracted)
        {
            Type = type;
            _onInteracted = onInteracted;
            
            PlaySpawnAnimation();
        }

        public CountableModel Init(int maxValue, int value)
        {
            CountableModel = new CountableModel(maxValue);
            CountableModel.Set(value);

            _valueSub = CountableModel.CurrentValue
                .Subscribe(UpdateColor);

            return CountableModel;
        }

        #endregion

        #region Physics

        private void FixedUpdate()
        {
            if (rb.isKinematic) return;

            rb.AddForce(Vector2.down * fallForce, ForceMode2D.Force);

            if (rb.velocity.y < -maxFallSpeed)
                rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
        }

        public void Push(Vector2 direction, float power)
        {
            rb.AddForce(direction.normalized * power, ForceMode2D.Impulse);
            PlayBounce();
        }

        #endregion

        #region Interactions

        public void Interact(Bullet bullet)
        {
            var dir = (transform.position - bullet.transform.position).normalized;

            // force UP bias
            dir.y = Mathf.Abs(dir.y) + 0.4f;

            Push(dir, bullet.PushPower);
            _onInteracted?.Invoke(this, bullet);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            PlayBounce();
        }

        #endregion

        #region Visuals

        private void UpdateColor(int value)
        {
            var t = (float)value / CountableModel.MaxValue;
            var mid = Color.yellow;

            sr.color = t < 0.5f
                ? Color.Lerp(minColor, mid, t * 2f)
                : Color.Lerp(mid, maxColor, (t - 0.5f) * 2f);
        }

        private void PlaySpawnAnimation()
        {
            KillScaleTween();

            transform.localScale = Vector3.zero;
            _scaleTween = transform
                .DOScale(_initialScale, 0.25f)
                .SetEase(Ease.OutBack)
                .SetTarget(this);
        }

        private void PlayBounce()
        {
            KillScaleTween();

            _scaleTween = transform
                .DOPunchScale(Vector3.one * bounceScale, bounceDuration)
                .SetEase(Ease.OutQuad)
                .SetTarget(this);
        }

        private void KillScaleTween()
        {
            if (_scaleTween != null && _scaleTween.IsActive())
                _scaleTween.Kill(complete: true);
        }

        #endregion

        private void OnDestroy()
        {
            KillScaleTween();
            _valueSub?.Dispose();
        }
    }
}
