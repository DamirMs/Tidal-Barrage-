using Gameplay.Current.Ball_Blast.Balls;
using Gameplay.Current.Ball_Blast.Bullets;
using Gameplay.Current.Ball_Blast.Countables;
using Gameplay.Current.Ball_Blast.Interactables;
using Gameplay.Current.Configs;
using Gameplay.General.Other;
using Gameplay.General.Score;

namespace Gameplay.Current.Ball_Blast.InteractablesHandlers
{
    public class BallInteractableHandler : IInteractableHandler
    {
        private readonly SoundManager _soundManager;
        private readonly InteractablesManager _interactablesManager;
        private readonly BallsSplitInfoProvider _ballsSplitInfoProvider;
        private readonly GoldManager _goldManager;
        private readonly ScoreManager _scoreManager;
        private readonly GameInfoConfig _gameInfoConfig;
        private readonly VibroManager _vibroManager;

        public BallInteractableHandler(SoundManager soundManager, InteractablesManager interactablesManager, 
            BallsSplitInfoProvider ballsSplitInfoProvider, GoldManager goldManager, ScoreManager scoreManager,
            GameInfoConfig gameInfoConfig, VibroManager vibroManager)
        {
            _soundManager = soundManager;
            _interactablesManager = interactablesManager;
            _ballsSplitInfoProvider = ballsSplitInfoProvider;
            _goldManager = goldManager;
            _scoreManager = scoreManager;
            _gameInfoConfig = gameInfoConfig;
            _vibroManager = vibroManager;
        }

        public void Handle(IInteractable interactable, Bullet bullet)
        {
            if (interactable is ICountable countable)
            {
                countable.CountableModel.Decrease(bullet.Damage);
                
                _scoreManager.UpdateScore(1); 
                
                if (countable.CountableModel.CurrentValue.Value <= 0)
                {
                    if (interactable.Type == InteractableTypeEnum.Ball2Children)
                    {
                        _interactablesManager.SpawnInteractables(interactable.Transform.position, InteractableTypeEnum.BallChild, _ballsSplitInfoProvider.GetBallsSplitInfo(interactable.Type).Amount);
                    }
                    
                    _interactablesManager.RemoveInteractable(interactable);
                    
                    _goldManager.AddGold(_gameInfoConfig.GoldAmountFromBalls);
                    //_soundManager.PlaySound(SoundManager.SoundEventEnum.FinishReached);
                    //_vibroManager.Vibrate();
                }
            }
        }
    }
}