using Gameplay.Current.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.Current.Ball_Blast.Progress
{
    public class LevelUpController : MonoBehaviour
    {
        [SerializeField] private LevelUpView speed;
        [SerializeField] private LevelUpView size;
        [SerializeField] private LevelUpView damage;

        [Inject] private ProgressManager _progress;
        [Inject] private GoldManager _gold;
        [Inject] private GameInfoConfig _config;

        private void Awake()
        {
            speed.Init(() => Upgrade(ProgressTypeEnum.BulletsReloadSpeed));
            size.Init(() => Upgrade(ProgressTypeEnum.BulletsSize));
            damage.Init(() => Upgrade(ProgressTypeEnum.BulletsDamage));

            HideAll();
        }

        public void Show()
        {
            speed.Show();
            size.Show();
            damage.Show();
        }

        private void Upgrade(ProgressTypeEnum type)
        {
            if (!_gold.CanAfford(_config.GoldAmountForUpgrade)) return;

            _progress.ImproveProgress(type);
            _gold.SpendGold(_config.GoldAmountForUpgrade);

            HideAll();
        }

        private void HideAll()
        {
            speed.Hide();
            size.Hide();
            damage.Hide();
        }
    }
}