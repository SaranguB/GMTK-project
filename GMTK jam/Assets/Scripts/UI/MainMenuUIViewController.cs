using System;
using System.Collections;
using System.Collections.Generic;
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
          CanvasGroupExtension.Hide(instructionsMenuCanvas);
          UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }
        
        private void OnPlayButtonClicked()
        {
            CanvasGroupExtension.Hide(mainMenuCanvas);
            GameManager.Instance.ChangeState(GameStates.Gameplay);
            CanvasGroupExtension.Show(GameManager.Instance.UIService.InGameUIViewController.IngameUICanvas); 
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }
        
        private void OnInstructionsButtonClicked()
        {
           CanvasGroupExtension.Show(mainMenuCanvas);
           UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
           
        }
        
        private void OnquitButtonClicked()
        {
            Application.Quit();
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }
    }
}