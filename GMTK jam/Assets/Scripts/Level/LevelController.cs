using Main;
using UnityEngine;

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
            SetCheckPoints();
        }

        private void SetCheckPoints()
        {
            for(int  i=0; i<levelView.CheckPointController.Length; i++)
            {
                levelView.CheckPointController[i].CurrentIndex = i;
                levelView.CheckPointController[i].SetController(this);
            }
        }


        public void LoadCurrentCheckPoint()
        {
            string key = $"Checkpoint_Level_{levelModel.LevelID}";
            int checkpointIndex = PlayerPrefs.GetInt(key, 0); 
            levelModel.currentCheckPoint = checkpointIndex;
            
            Debug.Log("Called");
            GameManager.Instance.EventService.OnCheckPointChanged.InvokeEvent
                (levelView.CheckPointController[levelModel.currentCheckPoint]);
        }
        
        public void OnCheckPointChanged(int newCheckpointIndex)
        {
            levelModel.currentCheckPoint = newCheckpointIndex;

            string key = $"Checkpoint_Level_{levelModel.LevelID}";
            PlayerPrefs.SetInt(key, newCheckpointIndex);
            PlayerPrefs.Save();
            
            Debug.Log($"Checkpoint changed to {newCheckpointIndex} for level {levelModel.LevelID}");
            // Trigger the event directly instead of calling LoadCurrentCheckPoint to avoid recursion
            GameManager.Instance.EventService.OnCheckPointChanged.InvokeEvent
                (levelView.CheckPointController[levelModel.currentCheckPoint]);
        }
        
    }
}