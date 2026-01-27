using Gameplay.Current.Ball_Blast.Balls;
using UnityEngine;

namespace Gameplay.Current.Ball_Blast.Configs
{
    [CreateAssetMenu(menuName = "Configs/BallsSplitInfo", fileName = "BallsSplitInfoConfig")]
    public class BallsSplitInfoConfig : ScriptableObject
    {
        [SerializeField] private BallsSplitInfo[] ballsSplitInfo;
        
        public BallsSplitInfo[] BallsSplitInfo => ballsSplitInfo;
    }
}