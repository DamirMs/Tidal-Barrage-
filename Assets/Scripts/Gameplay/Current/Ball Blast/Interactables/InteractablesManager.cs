using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Current.Ball_Blast.Bullets;
using Gameplay.Current.Ball_Blast.Countables;
using Gameplay.Current.Ball_Blast.Difficulty;
using Gameplay.Current.Ball_Blast.InteractablesLabels;
using Gameplay.Current.Configs;
using Gameplay.IOS.Animations;
using Gameplay.IOS.CurrencyRelated;
using PT.Tools.Effects;
using PT.Tools.EventListener;
using PT.Tools.ObjectPool;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Gameplay.Current.Ball_Blast.Interactables
{
    public class InteractablesManager : MonoBehaviourEventListener
    {
        [SerializeField] private InteractablesLabelsManager interactablesLabelsManager;

        [Space] [SerializeField] private Transform interactablesStartPoint;

        [SerializeField] private Transform interactablesEndPoint;

        [Space] [SerializeField] private InteractablesFactory interactablesFactory;
        
        [Space] 
        [SerializeField] private ParticleSystem destroyEffect;
        [SerializeField] private Transform destroysParent;
        [Space] 
        [SerializeField] private TextPopUp textPopUpPrefab;
        [SerializeField] private Transform popUpsParent;
        [SerializeField] private Camera cam;

        private readonly List<IInteractable> _currentInteractables = new();

        [Inject] private DiContainer _container;
        [Inject] private DifficultyInfoProvider _difficultyInfoProvider;
        [Inject] private GameInfoConfig _gameInfoConfig;
        [Inject] private CurrencyManager _currencyManager;

        private Dictionary<InteractableTypeEnum, IInteractableHandler> _interactablesHandlers;

        private CancellationTokenSource _spawnCts;
        
        private ParticleSystemPool _destroyedParticlesPool = new();
        
        private ObjectPool<TextPopUp> _textPopUpPool;

        private void Awake()
        {
            _destroyedParticlesPool.Init(destroyEffect.gameObject, destroysParent, 50);
            
            var initialTextPopUpScale = textPopUpPrefab.transform.localScale;
            _textPopUpPool = new ObjectPool<TextPopUp>(
                () => Instantiate(textPopUpPrefab, popUpsParent),
                t => t.gameObject.SetActive(true),
                t =>
                {
                    t.gameObject.SetActive(false);
                    t.transform.localScale = initialTextPopUpScale;
                },
                defaultCapacity: 20);
            
            AddEventActions(new Dictionary<GlobalEventEnum, Action>
            {
                { GlobalEventEnum.GameStarted, OnGameStarted },
                { GlobalEventEnum.GameEnded, OnGameEnded }
            });

            _interactablesHandlers = new Dictionary<InteractableTypeEnum, IInteractableHandler>
            {
                { InteractableTypeEnum.Ball, _container.ResolveId<IInteractableHandler>(InteractableTypeEnum.Ball) },
                {
                    InteractableTypeEnum.Ball2Children,
                    _container.ResolveId<IInteractableHandler>(InteractableTypeEnum.Ball2Children)
                },
                {
                    InteractableTypeEnum.BallChild,
                    _container.ResolveId<IInteractableHandler>(InteractableTypeEnum.BallChild)
                },
            };
        }

        private void OnGameStarted()
        {
            RemoveAllInteractables();

            _spawnCts = new CancellationTokenSource();
            SpawnLoop(_spawnCts.Token).Forget();
        }

        private void OnGameEnded()
        {
            _spawnCts?.Cancel();
            RemoveAllInteractables();
        }

        public void RemoveInteractable(IInteractable interactable)
        {
            if (interactable == null) return;
            
            var effect= _destroyedParticlesPool.Get();
            effect.transform.position = interactable.Transform.position;
            effect.Play();
            effect.GetComponent<PoolEffectHandler>().Init((g, ps) => _destroyedParticlesPool.Set(ps));
            
            _currencyManager.Add(CurrencyType.Gold, Utils.GetRandomValueInt(4, 8));
            
            _currentInteractables.Remove(interactable);
            interactablesFactory.Return(interactable);
        }

        public void SpawnInteractables(Vector2 parentPos, InteractableTypeEnum type, int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                var offset = Utils.GetRandomVector(1) * 0.5f;
                var pos = parentPos + offset;

                var difficultyInfo = _difficultyInfoProvider.GetDifficultyInfo(0);
                // var difficultyInfo = _difficultyInfoProvider.GetDifficultyInfo(GameData.LevelIndex);

                var child = SpawnOneInteractable(type, pos, difficultyInfo);
                _currentInteractables.Add(child);

                if (child is ICountable countable)
                    interactablesLabelsManager.AddLabel(child, countable.CountableModel);
            }
        }

        private async UniTaskVoid SpawnLoop(CancellationToken token)
        {
            var difficultyInfo = _difficultyInfoProvider.GetDifficultyInfo(0);

            while (!token.IsCancellationRequested)
            {
                if (IsMaxActiveInteractables(difficultyInfo))
                {
                    await UniTask.Yield();
                    continue;
                }

                if (difficultyInfo.SpawnChance < Utils.GetRandomNext(1)) continue;

                var type = PickInteractableType(difficultyInfo);

                var spawnPos = PickSpawnPosition();
                var interactable = SpawnOneInteractable(type, spawnPos, difficultyInfo);

                _currentInteractables.Add(interactable);

                if (interactable is ICountable countable)
                    interactablesLabelsManager.AddLabel(interactable, countable.CountableModel);

                await UniTask.Delay(TimeSpan.FromSeconds(difficultyInfo.SpawnInterval.GetRandomValue()),
                    cancellationToken: token);
            }
        }

        private bool IsMaxActiveInteractables(DifficultyInfo difficultyInfo)
        {
            return _currentInteractables.Where(x => x.Type != InteractableTypeEnum.BallChild).ToList().Count >=
                   difficultyInfo.MaxActiveAtOnce;
        }

        private InteractableTypeEnum PickInteractableType(DifficultyInfo info)
        {
            var list = new List<(InteractableTypeEnum, float)>();
            foreach (var kvp in info.InteractableSpawnChances) list.Add((kvp.Key, kvp.Value));
            return Utils.PickByWeight(list);
        }

        private Vector2 PickSpawnPosition()
        {
            return Utils.GetRandomBetweenVectors(interactablesStartPoint.position, interactablesEndPoint.position);
        }

        private IInteractable SpawnOneInteractable(InteractableTypeEnum type, Vector2 pos, DifficultyInfo info)
        {
            return interactablesFactory.Spawn(type, OnInteractionHandle, pos, info);
        }

        private void OnInteractionHandle(IInteractable interactable, Bullet bullet)
        {
            var textPopUp = _textPopUpPool.Get();
            textPopUp.Play($"-{bullet.Damage}", cam.WorldToScreenPoint(bullet.transform.localPosition))
                .Done(() =>
                {
                    _textPopUpPool.Release(textPopUp);
                });
            
            _interactablesHandlers[interactable.Type].Handle(interactable, bullet);
        }

        private void RemoveAllInteractables()
        {
            foreach (var interactable in _currentInteractables) interactablesFactory.Return(interactable);

            _currentInteractables.Clear();
        }
    }
}