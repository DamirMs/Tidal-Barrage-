using System;
using Gameplay.Current.Ball_Blast.Interactables;
using UnityEngine;

namespace Gameplay.Current.Ball_Blast.Balls
{
    [Serializable]
    public class BallsSplitInfo
    {
        [SerializeField] private InteractableTypeEnum type;
        [SerializeField] private int amount = 1;
        [SerializeField] private Vector2 range = Vector2.one;
        
        public InteractableTypeEnum Type => type;
        public int Amount => amount;
        public Vector2 Range => range;
    }
}