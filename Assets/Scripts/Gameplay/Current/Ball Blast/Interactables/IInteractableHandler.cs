using Gameplay.Current.Ball_Blast.Bullets;

namespace Gameplay.Current.Ball_Blast.Interactables
{
    public interface IInteractableHandler
    {
        void Handle(IInteractable interactable, Bullet bullet);
    }
}