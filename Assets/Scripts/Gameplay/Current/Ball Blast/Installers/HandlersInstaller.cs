using Gameplay.Current.Ball_Blast.Interactables;
using Gameplay.Current.Ball_Blast.InteractablesHandlers;
using Zenject;

namespace Gameplay.Current.Ball_Blast.Installers
{
    public class HandlersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInteractableHandler>().WithId(InteractableTypeEnum.Ball).To<BallInteractableHandler>().AsTransient();
            Container.Bind<IInteractableHandler>().WithId(InteractableTypeEnum.BallChild).To<BallInteractableHandler>().AsTransient();
            Container.Bind<IInteractableHandler>().WithId(InteractableTypeEnum.Ball2Children).To<BallInteractableHandler>().AsTransient();
        }
    }
}