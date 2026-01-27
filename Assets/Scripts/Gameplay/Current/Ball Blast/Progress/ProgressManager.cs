using System.Collections.Generic;
using PT.Tools.EventListener;
using Zenject;

namespace Gameplay.Current.Ball_Blast.Progress
{
    public class ProgressManager : MonoBehaviourEventListener
    {
        [Inject] private ProgressInfoConfig _config;

        private readonly Dictionary<ProgressTypeEnum, int> _levels = new()
        {
            { ProgressTypeEnum.BulletsReloadSpeed, 0 },
            { ProgressTypeEnum.BulletsDamage, 0 },
            { ProgressTypeEnum.BulletsSize, 0 }
        };

        public float GetProgressValue(ProgressTypeEnum type)
        {
            int level = _levels[type];

            return type switch
            {
                ProgressTypeEnum.BulletsReloadSpeed => (level * _config.BulletsReloadSpeedProgressValue),
                ProgressTypeEnum.BulletsDamage => 1f + ((float)level * _config.BulletsDamageProgressValue),
                ProgressTypeEnum.BulletsSize => (float)level * _config.BulletsSizeProgressValue,
                _ => throw new System.ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public void ImproveProgress(ProgressTypeEnum type)
        {
            int currentLevel = _levels[type];
            int maxLevel = GetMaxLevel(type);

            if (currentLevel >= maxLevel) return;

            _levels[type] = currentLevel + 1;
        }

        private int GetMaxLevel(ProgressTypeEnum type)
        {
            return type switch
            {
                ProgressTypeEnum.BulletsReloadSpeed => _config.BulletsReloadSpeedMaxProgressLevel,
                ProgressTypeEnum.BulletsDamage => _config.BulletsDamageMaxProgressLevel,
                ProgressTypeEnum.BulletsSize => _config.BulletsSizeMaxProgressLevel,
                _ => 0
            };
        }
    }
}