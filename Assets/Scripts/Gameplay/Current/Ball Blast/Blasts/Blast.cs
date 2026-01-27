using System;
using Cysharp.Threading.Tasks;
using Gameplay.General.Game;
using PT.Tools.Effects;
using UnityEngine;

namespace Gameplay.Current.Ball_Blast.Blasts
{
    public class Blast : MonoBehaviour
    {
        [SerializeField] private BlastInfoConfig blastInfoConfig;
        [SerializeField] private Transform shootingPoint;
        [Space]
        [SerializeField] private TargetTrigger ballTargetTrigger;
        [SerializeField] private SimpleAnimationPlayer simpleAnimationPlayer;
        
        public BlastInfoConfig BlastInfoConfig => blastInfoConfig;
        public Vector2 ShootingPosition => shootingPoint.position;

        private Action _onInteractedWithBall;

        private void OnEnable()
        {
            ballTargetTrigger.OnTriggered += Triggered;
        }
        private void OnDisable()
        {
            ballTargetTrigger.OnTriggered -= Triggered;

            _onInteractedWithBall = null;
        }
        
        public void Init(Action onInteractedWithBall)
        {
            _onInteractedWithBall = onInteractedWithBall;
        }

        public void PlayAnimation()
        {
            simpleAnimationPlayer?.PlayAnimation().Forget();
        }

        private void Triggered(GameObject go)
        {
            _onInteractedWithBall?.Invoke();
        }
    }
}