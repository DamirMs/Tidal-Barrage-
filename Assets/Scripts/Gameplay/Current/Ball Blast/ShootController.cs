using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay.Current.Ball_Blast.Blasts;
using Gameplay.Current.Ball_Blast.Bullets;
using Gameplay.Current.Ball_Blast.Progress;
using Gameplay.Current.Configs;
using Gameplay.General.Input;
using PT.Tools.EventListener;
using UnityEngine;
using Zenject;

namespace Gameplay.Current.Ball_Blast
{
    public class ShootController : MonoBehaviourEventListener
    {
        [SerializeField] private Blast[] blasts;
        [Space]
        [SerializeField] private Transform blastLimitPointStart;
        [SerializeField] private Transform blastLimitPointEnd;
        [Space]
        [SerializeField] private Transform blastInitialPosition;

        public event Action OnBallHitBlast;
        
        [Inject] private GameInfoConfig _gameInfoConfig;
        [Inject] private GameInputController _gameInputController;
        [Inject] private BulletsManager _bulletsManager;
        [Inject] private ProgressManager _progressManager;

        private Blast _currentBlast;

        private bool _isShooting;
        private bool _isReloading;

        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted },
                { GlobalEventEnum.GameEnded, OnGameEnded },
                { GlobalEventEnum.GameMenuOpened, OnGameMenuOpened },
                { GlobalEventEnum.GameMenuClosed, OnGameMenuClosed },
                { GlobalEventEnum.GameUpgradesOpened, OnGameUpgradesOpened },
                { GlobalEventEnum.GameUpgradesClosed, OnGameUpgradesClosed },
            });

        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            SignUp();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SignOut();
        }
        
        private void SignUp()
        {
            _gameInputController.OnClick += StartShooting;
            _gameInputController.OnDrag += Move;
            _gameInputController.OnRelease += StopShooting;
        }
        private void SignOut()
        {
            _gameInputController.OnClick -= StartShooting;
            _gameInputController.OnDrag -= Move;
            _gameInputController.OnRelease -= StopShooting;
            
            _isShooting = false;
        }
        
        private void Update()
        {
            if (_isShooting)
            {
                Shoot();
            }
        }
        
        private void OnGameStarted()
        {
            _currentBlast?.SetActive(false);
            _currentBlast = blasts[0];
            _currentBlast.SetActive(true);
            
            _currentBlast.Init(BlastTriggered);
            
            _currentBlast.transform.position = blastInitialPosition.position;
            
            //cancel momentum
        }
        private void OnGameEnded()
        {
        }
        private void OnGameMenuOpened()
        {
            SignOut();
        }
        private void OnGameMenuClosed()
        {
            SignUp();
        }
        private void OnGameUpgradesOpened()
        {
            SignOut();
        }
        private void OnGameUpgradesClosed()
        {
            SignUp();
        }

        private void StartShooting(Vector2 position)
        {
            _isShooting = true;
            
            Move(position);
        }

        private void Move(Vector2 position)
        {
            var fixedPosition = new Vector2(
                Utils.AdjustValueBetweenMinMax(blastLimitPointStart.position.x, blastLimitPointEnd.position.x, position.x), 
                blastInitialPosition.position.y);
            
            // _currentBlast.transform.position = Vector2.Lerp(_currentBlast.transform.position, fixedPosition, _currentBlast.BlastInfoConfig.MoveSpeed); 
            _currentBlast.transform.position = fixedPosition; 
        }

        private void StopShooting(Vector2 position)
        {
            _isShooting = false;
            
            Move(position);
        }
        
        private void Shoot()
        {
            if (_isReloading) return;

            var blastInfo = _currentBlast.BlastInfoConfig;
            float reloadSpeedMultiplier = _progressManager.GetProgressValue(ProgressTypeEnum.BulletsReloadSpeed);
            float damageMultiplier = _progressManager.GetProgressValue(ProgressTypeEnum.BulletsDamage);
            float power = _progressManager.GetProgressValue(ProgressTypeEnum.BulletsSize);

            for (int i = 0; i < blastInfo.BulletsPerShot; i++)
            {
                var spread = Utils.GetRandomValue(-blastInfo.SpreadAngle / 2, blastInfo.SpreadAngle / 2);

                _bulletsManager.Shoot(
                    blastInfo.BulletType,
                    _currentBlast.ShootingPosition,
                    spread,
                    blastInfo.BulletPower,
                    blastInfo.PushPower + power,
                    (blastInfo.Damage + damageMultiplier)
                );
                
                _currentBlast.PlayAnimation();
            }

            Reload(blastInfo.FireRate - reloadSpeedMultiplier).Forget();
        }

        private async UniTaskVoid Reload(float time)
        {
            _isReloading = true;
            await UniTask.Delay(TimeSpan.FromSeconds(time));
            _isReloading = false;
        }

        private void BlastTriggered()
        {
            OnBallHitBlast?.Invoke();
        }
    }
}