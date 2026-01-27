using UnityEngine;

namespace Gameplay.Current.Ball_Blast.Countables
{
    public interface ICountable
    {
        CountableModel CountableModel { get; }
        CountableModel Init(int maxValue, int value);

        void Push(Vector2 direction, float power);//myb remove
    }
}