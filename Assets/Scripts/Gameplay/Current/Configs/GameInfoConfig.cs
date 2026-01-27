using System.Collections.Generic;
using Gameplay.Current.Ball_Blast.Interactables;
using Gameplay.General.Configs;
using PT.Tools.Helper;
using UnityEngine;

namespace Gameplay.Current.Configs
{
    [CreateAssetMenu(menuName = "Configs/GameInfo", fileName = "GameInfoConfig")]
    public class GameInfoConfig : BaseGameInfoConfig
    {
        [Space(20)]
        [Header("GAME settings:")]
        [SerializeField] private SerializableKeyValue<InteractableTypeEnum, Vector2Int> interactablesCountableDefaultValues;
        [SerializeField] private SerializableKeyValue<InteractableTypeEnum, float> interactablesCountableCoefficients;
        [Space]
        [SerializeField] private int goldAmountFromBalls = 10;
        [SerializeField] private int goldAmountForUpgrade = 100;
        
        public Dictionary<InteractableTypeEnum, Vector2Int> InteractablesCountableDefaultValues => interactablesCountableDefaultValues.Dictionary;
        public Dictionary<InteractableTypeEnum, float> InteractablesCountableCoefficients => interactablesCountableCoefficients.Dictionary;
        
        public int GoldAmountFromBalls => goldAmountFromBalls;
        public int GoldAmountForUpgrade => goldAmountForUpgrade;
    }
}