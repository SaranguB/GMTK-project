namespace Level
{
    public class LevelController
    {
        private LevelModel levelModel;
        private LevelView levelView;
        
        public LevelModel LevelModel => levelModel;
        public LevelView LevelView => levelView;
        
        public LevelController(LevelView levelView, LevelSO levelSo)
        {
            this.levelView = levelView;
            levelModel = new LevelModel(levelSo);
        }

        
        private void LoadCurrentCheckPoint()
        {
           
        }
        
        private void OnCheckPointChanged()
        {
            
        }
        
    }
}