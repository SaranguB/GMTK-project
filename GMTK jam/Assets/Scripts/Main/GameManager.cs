using Audio;
using Events;
using Player;
using UI;
using UnityEngine;
using Utilities;

namespace Main
{
    public enum GameStates
    {
        MainMenu,
        Gameplay
    }

    public class GameManager : GenericMonoSingelton<GameManager>
    {
        [SerializeField] private UIService uiService;
        [SerializeField] private PlayerManager playerManager;
        
        private EventService eventService;

        public EventService EventService => eventService;

        private GameStates currentState;

        public GameStates CurrentState => currentState;
        public UIService UIService => uiService;
        public PlayerManager PlayerManager => playerManager;

        protected override void Awake()
        {
            base.Awake();
            eventService = new EventService();
            currentState = GameStates.MainMenu; 
            PlayerPrefs.DeleteAll();
            PlayBackgroudMusic();
        }

        public void ChangeState(GameStates newState)
        {
            if (currentState == newState)
                return;
            
            currentState = newState;
            PlayBackgroudMusic();
        }

        private void PlayBackgroudMusic()
        {
            if (currentState == GameStates.MainMenu)
                SoundManager.Instance.PlayBackgroundMusic(SoundType.MenuBackground, true);
            else if(currentState == GameStates.Gameplay)
                SoundManager.Instance.PlayBackgroundMusic(SoundType.GameplayBackground, true);
        }
    }
    
}