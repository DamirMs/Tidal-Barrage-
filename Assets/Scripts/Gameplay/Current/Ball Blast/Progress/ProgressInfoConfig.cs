using UnityEngine;

namespace Gameplay.Current.Ball_Blast.Progress
{
    [CreateAssetMenu(menuName = "Configs/ProgressInfo", fileName = "ProgressInfoConfig")]
    public class ProgressInfoConfig : ScriptableObject
    {
        [Space]
        [SerializeField] private int bulletsReloadSpeedMaxProgressLevel = 20;
        [SerializeField] private float bulletsReloadSpeedProgressValue = 0.1f;
        [Space]
        [SerializeField] private int bulletsDamageMaxProgressLevel = 20;
        [SerializeField] private float bulletsDamageProgressValue = 0.1f;
        [Space]
        [SerializeField] private int bulletsSizeMaxProgressLevel = 20;
        [SerializeField] private float bulletsSizeProgressValue = 0.1f;
        
        public int BulletsReloadSpeedMaxProgressLevel => bulletsReloadSpeedMaxProgressLevel;
        public int BulletsDamageMaxProgressLevel => bulletsDamageMaxProgressLevel;
        public int BulletsSizeMaxProgressLevel => bulletsSizeMaxProgressLevel;
        public float BulletsReloadSpeedProgressValue => bulletsReloadSpeedProgressValue;
        public float BulletsDamageProgressValue => bulletsDamageProgressValue;
        public float BulletsSizeProgressValue => bulletsSizeProgressValue;
    }
}