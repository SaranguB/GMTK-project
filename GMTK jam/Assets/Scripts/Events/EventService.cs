using Level;
using Player;
using Player.Ghost;
using UnityEngine;

namespace Events
{
    public class EventService
    {
        public EventController<PlayerController> OnPlayerDied;
        public EventController OnSkeltonRevived;
        public EventController<Transform> OnSwitchPlaced;
        public EventController<GhostController> OnGhostDestroyed;
        public EventController<CheckPointViewController> OnCheckPointChanged;
        public EventController OnPortalButtonInteracted;
        public EventController OnPortalButtonStopedInteracting;
        public EventController OnLevelFinished;
        public EventController OnNextLevelCreated;
        
        public EventService()
        {
            OnPlayerDied =  new EventController<PlayerController>();
            OnSkeltonRevived =  new EventController();
            OnSwitchPlaced =  new EventController<Transform>();
            OnGhostDestroyed =  new EventController<GhostController>();
            OnCheckPointChanged =  new EventController<CheckPointViewController>();
            OnPortalButtonInteracted =  new EventController();
            OnLevelFinished =  new EventController();
            OnNextLevelCreated =  new EventController();
            OnPortalButtonStopedInteracting = new EventController();
        }

      
    }
}