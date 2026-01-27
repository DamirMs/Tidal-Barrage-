using System;
using Gameplay.Current.Ball_Blast.Bullets;
using UnityEngine;

namespace Gameplay.Current.Ball_Blast.Interactables
{
    public interface IInteractable
    {
        Transform Transform { get; }
        InteractableTypeEnum Type { get; }

        void Init(InteractableTypeEnum type, Action<IInteractable, Bullet> onInteracted);
        void Interact(Bullet bullet);
    }
}