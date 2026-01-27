using System;
using Gameplay.Current.Ball_Blast.Progress;
using Gameplay.Current.Configs;
using PT.Tools.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Current.Ball_Blast.Windows
{
    public class UpgradeWindow : WindowBase
    {
        [SerializeField] private Button speedButton, sizeButton, damageButton;

        public event Action OnUpgradeBought;
        
        [Inject] private GameInfoConfig _gameInfoConfig;
        [Inject] private GoldManager _goldManager;
        [Inject] private ProgressManager _progressManager;

        private void Awake()
        {
            speedButton.onClick.AddListener(() => Upgrade(ProgressTypeEnum.BulletsReloadSpeed));
            sizeButton.onClick.AddListener(() => Upgrade(ProgressTypeEnum.BulletsSize));
            damageButton.onClick.AddListener(() => Upgrade(ProgressTypeEnum.BulletsDamage));
        }

        private void Upgrade(ProgressTypeEnum type)
        {
            if (!_goldManager.CanAfford(_gameInfoConfig.GoldAmountForUpgrade)) return;

            _progressManager.ImproveProgress(type);
            _goldManager.SpendGold(_gameInfoConfig.GoldAmountForUpgrade);

            OnUpgradeBought?.Invoke();
        }
    }
}