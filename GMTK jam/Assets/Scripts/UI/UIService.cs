using Main;
using UI.MainMenuUI;
using UnityEngine;

namespace UI
{
    public class UIService : MonoBehaviour
    {
        [SerializeField] private MainMenuUIViewController  mainMenuUIViewController;
        [SerializeField] private InGameUIViewController inGameUIViewController;
        
        public MainMenuUIViewController MainMenuUIViewController => mainMenuUIViewController;
        public InGameUIViewController InGameUIViewController => inGameUIViewController;
            
        private void Start()
        {
           
        }

        private void SubscribeToEvents()
        {
           
        }

        private void InitializeUIControllers()
        {
           
        }

    }
}
