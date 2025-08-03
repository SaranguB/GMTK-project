using Main;
using UI.MainMenuUI;
using UnityEngine;

namespace UI
{
    public class UIService : MonoBehaviour
    {
        [SerializeField] private MainMenuUIViewController  mainMenuUIViewController;
        [SerializeField] private InGameUIViewController inGameUIViewController;
        [SerializeField] private GameEndUIViewController  gameEndUIViewController;
        
        public MainMenuUIViewController MainMenuUIViewController => mainMenuUIViewController;
        public InGameUIViewController InGameUIViewController => inGameUIViewController;
        public GameEndUIViewController GameEndUIController=> gameEndUIViewController;

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
