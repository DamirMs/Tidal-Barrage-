using System.Collections.Generic;
using Gameplay.Current.Configs;
using Gameplay.General.Game;
using PT.Tools.EventListener;
using PT.Tools.ObjectPool;
using UnityEngine;
using Zenject;

namespace Gameplay.Current.Ball_Blast.Bullets
{
    public class BulletsManager : MonoBehaviourEventListener
    {
        [SerializeField] private Bullet[] bulletsPrefab; // myb make a config?
        [SerializeField] private Transform bulletsParent;
        [Space]
        [SerializeField] private TargetTrigger bulletsKillZoneTrigger;

        [Inject] private GameInfoConfig _gameInfoConfig;

        private Dictionary<BulletTypeEnum, MonoBehPool<Bullet>> _bulletsPools = new();

        private List<Bullet> _currentBullets = new();

        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted },
                { GlobalEventEnum.GameEnded, OnGameEnded },
            });

            foreach (var bullet in bulletsPrefab)
            {
                _bulletsPools[bullet.BulletType] = new();
                
                _bulletsPools[bullet.BulletType].Init(bullet.gameObject, bulletsParent, 50);
            }
            
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            bulletsKillZoneTrigger.OnTriggered += ReturnBullet;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            
            bulletsKillZoneTrigger.OnTriggered -= ReturnBullet;
        }
        
        private void OnGameStarted()
        {
        }

        private void OnGameEnded()
        {
            RemoveBullets();
        }

        public void Shoot(BulletTypeEnum bulletType, Vector2 position, float angle, float power, float pushPower, float damage)
        {
            DebugManager.Log(DebugCategory.Gameplay, $"Shoot bullet {bulletType}");
            
            var bullet = _bulletsPools[bulletType].Get();
            bullet.transform.position = position;
            bullet.Init(ReturnBullet);
            bullet.Shoot(angle, power, pushPower, damage);
            
            _currentBullets.Add(bullet);
        }
        
        private void RemoveBullets()
        {
            while (_currentBullets.Count > 0) ReturnBullet(_currentBullets[0]);
        }
        
        private void ReturnBullet(GameObject obj) => ReturnBullet(obj.GetComponent<Bullet>());
        private void ReturnBullet(Bullet bullet)
        {
            _bulletsPools[bullet.BulletType].Set(bullet);

            _currentBullets.Remove(bullet);
        }
    }
}