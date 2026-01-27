using Gameplay.Current.Ball_Blast;
using Gameplay.Current.Ball_Blast.Balls;
using Gameplay.Current.Ball_Blast.Bullets;
using Gameplay.Current.Ball_Blast.Configs;
using Gameplay.Current.Ball_Blast.Difficulty;
using Gameplay.Current.Ball_Blast.Interactables;
using Gameplay.Current.Ball_Blast.Progress;
using Gameplay.Current.Ball_Blast.Windows;
using Gameplay.General.Installers;
using UnityEngine;

namespace Gameplay.Current.Installers
{
    public class GameInstaller : BaseGameInstaller
    {
        [SerializeField] private DifficultyInfoConfig difficultyInfoConfig;
        [SerializeField] private BallsSplitInfoConfig ballsSplitInfoConfig;
        [SerializeField] private ProgressInfoConfig progressInfoConfig;
        
        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.Bind<InteractablesManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ShootController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<BulletsManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GoldManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ProgressManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<UpgradeWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<LevelUpController>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<DifficultyInfoConfig>().FromInstance(difficultyInfoConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<DifficultyInfoProvider>().AsSingle();
            
            Container.Bind<BallsSplitInfoConfig>().FromInstance(ballsSplitInfoConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<BallsSplitInfoProvider>().AsSingle();
            
            Container.Bind<ProgressInfoConfig>().FromInstance(progressInfoConfig).AsSingle();
        }
    }
}