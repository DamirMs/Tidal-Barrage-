using Gameplay.Current.Ball_Blast.Interactables;

namespace Gameplay.Current.Ball_Blast
{
    public class ElementModel
    {
        public InteractableTypeEnum Type { get; private set; }
        public int Value { get; private set; }

        public ElementModel(InteractableTypeEnum type, int value = 0)
        {
            Type = type;
            Value = value;
        }
    }
}