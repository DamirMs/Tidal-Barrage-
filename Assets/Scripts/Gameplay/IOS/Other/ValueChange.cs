namespace Gameplay.IOS.Other
{
    public enum ValueChangeType
    {
        Set,
        Add,
        Substract
    }

    public struct ValueChange
    {
        public readonly int Value;
        public readonly int PrevValue;
        public readonly ValueChangeType Type;

        public ValueChange(int value, int prevValue, ValueChangeType type)
        {
            Value = value;
            PrevValue = prevValue;
            Type = type;
        }
    }
}