using System;
using Gameplay.Current.Ball_Blast.Interactables;
using UnityEngine;

namespace Gameplay.Current.Ball_Blast.Bullets
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [Space]
        [SerializeField] private LayerMask collisionLayer;
        [Space]
        [SerializeField] private BulletTypeEnum bulletType;
        
        public BulletTypeEnum BulletType => bulletType;
        public float Damage { get; private set; }
        public float PushPower { get; private set; }

        private Action<Bullet> _onCollision;

        //bulletModel

        private void OnDisable()
        {
            rb.velocity = Vector2.zero;

            transform.localScale = Vector2.one; //or to initial scale
        }

        public void Init(Action<Bullet> onCollision)
        {
            _onCollision = onCollision;
        }
        
        public void Shoot(float angle, float power, float pushPower, float damage)
        {
            Damage = damage;
            PushPower = pushPower;

            transform.rotation = Quaternion.Euler(0, 0, angle);
            rb.AddForce(transform.up * power, ForceMode2D.Impulse);
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            TryInteract(collision.gameObject);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            TryInteract(collision.gameObject);
        }
        
        private void TryInteract(GameObject go)
        {
            if (((1 << go.layer) & collisionLayer.value) != 0)
            {
                if (go.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact(this);

                    _onCollision?.Invoke(this);
                }
            }
        }
    }
}