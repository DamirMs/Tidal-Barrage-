using Gameplay.Current.Ball_Blast.Bullets;
using UnityEngine;

namespace Gameplay.Current.Ball_Blast.Blasts
{
    [CreateAssetMenu(menuName = "Configs/BlastInfo", fileName = "BlastInfoConfig")]
    public class BlastInfoConfig : ScriptableObject
    {
        [SerializeField] private BlastType blastType;
        [SerializeField] [Min(1)] private float moveSpeed = 3;
        [Space]
        [SerializeField] private BulletTypeEnum bulletType;
        [SerializeField] [Min(0.01f)] private float fireRate = 1;
        [SerializeField] [Min(0.01f)] private float bulletPower = 1;
        [SerializeField] [Min(1)] private int damage = 1;
        [SerializeField] private float pushPower = 0.5f;
        [SerializeField] [Min(1)] private int bulletsPerShot = 1;
        [SerializeField] [Min(0)] private float spreadAngle;

        public BlastType BlastType => blastType;
        public float MoveSpeed => moveSpeed;
        
        public BulletTypeEnum BulletType => bulletType;
        public float FireRate => fireRate;
        public float BulletPower => bulletPower;
        public int Damage => damage;
        public float PushPower  => pushPower;
        public int BulletsPerShot => bulletsPerShot;
        public float SpreadAngle => spreadAngle;
    }
}