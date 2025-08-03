using System;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilis;

namespace UI
{
    public class GameEndUIViewController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup endScreenCanvas;
        [SerializeField] private Button restartButton;

        private void Awake()
        {
            restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void OnDestroy()
        {
            restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        }

        private void OnRestartButtonClicked()
        {
            SoundManager.Instance.PlaySoundEffects(SoundType.ButtonSound);
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void EnableEndScreen()
        {
            CanvasGroupExtension.Show(endScreenCanvas);
        }
    }
}