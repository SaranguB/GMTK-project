using System;
using System.Collections.Generic;
using Main;
using UnityEngine;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<LevelView> levelView;
        [SerializeField] private LevelSO levelSO;

        private Dictionary<Levels, LevelController> levelControllers = new();
        private LevelController levelController;


        private void Awake()
        {
            for (int i = 0; i < levelView.Count; i++)
            {
                levelController = new LevelController(levelView[i], levelSO);
                levelController.LevelModel.LevelID = levelSO.LevelData[i].levelID;  
                levelControllers.Add(levelSO.LevelData[i].level, levelController);
            }

            LoadCurrentLevel();
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
           GameManager.Instance.EventService.OnLevelFinished.AddListener(OnlevelFinished);
        }

        private void UnsubscribeFromEvents()
        {
            GameManager.Instance.EventService.OnLevelFinished.RemoveListener(OnlevelFinished);
        }
        


        private void LoadCurrentLevel()
        {
            if (!PlayerPrefs.HasKey("CurrentLevel"))
            {
                PlayerPrefs.SetInt("CurrentLevel", 0);
                PlayerPrefs.Save();
            }
            
            int currenLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            if (levelControllers.TryGetValue(levelSO.LevelData[currenLevel].level, out levelController))
            {
                levelController.LevelView.gameObject.SetActive(true);
                levelController.LoadCurrentCheckPoint();
            }
        }

        private void OnlevelFinished()
        {
            int currentLevel =  PlayerPrefs.GetInt("CurrentLevel", 0);
            if (levelControllers.TryGetValue(levelSO.LevelData[currentLevel].level, out levelController))
            {
                levelController.LevelView.gameObject.SetActive(false);
            }
            PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
            LoadCurrentLevel();
            
            GameManager.Instance.EventService.OnNextLevelCreated.InvokeEvent();
        }
    }
}