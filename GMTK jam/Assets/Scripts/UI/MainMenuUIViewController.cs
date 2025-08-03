using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Main;
using UnityEngine;
using UnityEngine.UI;
using Utilis;
using Utilities;

namespace UI.MainMenuUI
{
    public class MainMenuUIViewController : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button instructionsButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button instructionPanelBackButton;
        
        [SerializeField] private CanvasGroup mainMenuCanvas;
        [SerializeField] private CanvasGroup instructionsMenuCanvas;

        private void Awake()
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
            instructionsButton.onClick.AddListener(OnInstructionsButtonClicked);
            instructionPanelBackButton.onClick.AddListener(OnInstructionPanelBackButtonClicked);
            quitButton.onClick.AddListener(OnquitButtonClicked);
        }

        private void Start()
        {
            playButton.interactable = false; 
            quitButton.interactable = false;
            instructionsButton.interactable = false;
            StartCoroutine(EnableButtonsAfterDelay());
        }
        
    private IEnumerator EnableButtonsAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        playButton.interactable = true;
        instructionsButton.interactable = true;
        quitButton.interactable = true;
    }

        private void OnDestroy()
        {
            playButton.onClick.RemoveListener(OnPlayButtonClicked);
            instructionsButton.onClick.RemoveListener(OnInstructionsButtonClicked);
            quitButton.onClick.RemoveListener(OnquitButtonClicked);
        }
        
        private void OnInstructionPanelBackButtonClicked()
        {
            SoundManager.Instance.PlaySoundEffects(SoundType.ButtonSound);
            
          CanvasGroupExtension.Hide(instructionsMenuCanvas);
          UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }
        
        private void OnPlayButtonClicked()
        {
            SoundManager.Instance.PlaySoundEffects(SoundType.ButtonSound);
            
            CanvasGroupExtension.Hide(mainMenuCanvas);
            GameManager.Instance.ChangeState(GameStates.Gameplay);
            SoundManager.Instance.PlayBackgroundMusic(SoundType.GameplayBackground);
            CanvasGroupExtension.Show(GameManager.Instance.UIService.InGameUIViewController.IngameUICanvas); 
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }
        
        private void OnInstructionsButtonClicked()
        {
            SoundManager.Instance.PlaySoundEffects(SoundType.ButtonSound);
           CanvasGroupExtension.Show(instructionsMenuCanvas);
           UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
           
        }
        
        private void OnquitButtonClicked()
        {
            SoundManager.Instance.PlaySoundEffects(SoundType.ButtonSound);
            Application.Quit();
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }
    }
}