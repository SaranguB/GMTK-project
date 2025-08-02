namespace Level
{
    public class LevelModel
    {
        private LevelSO levelSO;
        
        public int levelNumber;
        public Levels currentLevel;
        public int LevelID;
        public int currentCheckPoint;
        
        public LevelModel(LevelSO levelSo)
        {
            this.levelSO = levelSo;
        }

      
    }
}