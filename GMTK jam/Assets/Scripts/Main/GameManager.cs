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
        }

        public void ChangeState(GameStates newState)
        {
            if (currentState == newState)
            {
                return;
            }
            else
            {
                currentState = newState;
            }
        }
    }
    
}