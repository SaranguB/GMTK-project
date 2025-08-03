using Events;
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
        
        private EventService eventService;

        public EventService EventService => eventService;

        private GameStates currentState;

        public GameStates CurrentState => currentState;

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