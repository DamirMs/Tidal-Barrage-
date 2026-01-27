namespace Gameplay.Current.Ball_Blast.Difficulty
{
    public class DifficultyInfoProvider
    {
        private DifficultyInfo[] _difficultyInfos;
        
        public DifficultyInfoProvider(DifficultyInfoConfig difficultyInfoConfig)
        {
            _difficultyInfos = difficultyInfoConfig.DifficultyInfos;
        }
        
        public DifficultyInfo GetDifficultyInfo(int difficultyLevel)
        {
            if (difficultyLevel < 0 || difficultyLevel >= _difficultyInfos.Length)
                throw new System.ArgumentOutOfRangeException(nameof(difficultyLevel), "Invalid difficulty level");

            return _difficultyInfos[difficultyLevel];
        }
    }
}