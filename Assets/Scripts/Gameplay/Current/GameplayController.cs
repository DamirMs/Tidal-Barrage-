using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Current.Ball_Blast;
using Gameplay.Current.Ball_Blast.Interactables;
using Gameplay.Current.Ball_Blast.Progress;
using Gameplay.Current.Ball_Blast.Windows;
using Gameplay.General.Game;
using Gameplay.General.Other;
using PT.Tools.EventListener;
using PT.Tools.Windows;
using Zenject;

namespace Gameplay.Current
{
    public class GameplayController : LevelGameplayController
    {
        [Inject] private InteractablesManager _interactablesManager;
        [Inject] private ShootController _shootController;
        [Inject] private GoldManager _goldManager;
        [Inject] private UpgradeWindow _upgradeWindow;
        [Inject] private LevelUpController _levelUpController;
        
        protected override void SignUp()
        {
            _shootController.OnBallHitBlast += BallHitBlast;
            _goldManager.OnEnoughGoldReached += ShowUpgradePanel;
            _upgradeWindow.OnUpgradeBought += UpgradeBought ;
        }
        
        protected override void SignOut()
        {
            _shootController.OnBallHitBlast -= BallHitBlast;
            _goldManager.OnEnoughGoldReached -= ShowUpgradePanel;
            _upgradeWindow.OnUpgradeBought -= UpgradeBought ;
        }
        
        private void AllBallsReturned()
        {
            // DebugManager.Log(DebugCategory.Gameplay, "All balls returned, starting new game turn");
            
            GameTurn().Forget();
        }
        
        private void BallHitBlast()
        {
            DebugManager.Log(DebugCategory.Gameplay, "Ball hit blast");
            
            GameOver().Forget();
        }
        private void ShowUpgradePanel()
        {
            _levelUpController.Show();
        }
        private void UpgradeBought()
        {
            // GlobalEventBus.On(GlobalEventEnum.GameUpgradesClosed);
            // _windowsManager.Close(WindowTypeEnum.Upgrades).Forget();
        }
        
        protected override UniTask OnGameTurn(CancellationToken token)
        {
            return UniTask.CompletedTask;
        }
    }
}