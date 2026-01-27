using System.Linq;
using Gameplay.Current.Ball_Blast.Configs;
using Gameplay.Current.Ball_Blast.Interactables;

namespace Gameplay.Current.Ball_Blast.Balls
{
    public class BallsSplitInfoProvider
    {
        private BallsSplitInfo[] _ballsSplitInfo;
        
        public BallsSplitInfoProvider(BallsSplitInfoConfig ballsSplitInfoConfig)
        {
            _ballsSplitInfo = ballsSplitInfoConfig.BallsSplitInfo;
        }
        
        public BallsSplitInfo GetBallsSplitInfo(InteractableTypeEnum type)
        {
            var info = _ballsSplitInfo.FirstOrDefault(x => x.Type == type);
            if (info == null) throw new System.Exception($"BallsSplitInfo not found for type: {type}");
            return info;
        }
    }
}