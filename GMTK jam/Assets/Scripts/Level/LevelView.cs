using System;
using Level.Interactables.Button;
using Level.Portals;
using UnityEngine;

namespace Level
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private CheckPointViewController[] checkPointController;
        [SerializeField] private PortalButtonInteractable portalButton;
        [SerializeField] private LevelFInishPortalViewController enterLevelFInishPortal;
        [SerializeField] private LevelFInishPortalViewController exitLevelFInishPortal;
        [SerializeField] private GameeFinishPortalViewController gameFinishedLevelFinishPortal;
        public CheckPointViewController[] CheckPointController => checkPointController;
        public PortalButtonInteractable PortalButton => portalButton;
        public LevelFInishPortalViewController EnterLevelFInishPortalView => enterLevelFInishPortal;
        public LevelFInishPortalViewController ExitLevelFInishPortalView => exitLevelFInishPortal;
        public GameeFinishPortalViewController GameFinishedLevelFinishPortalView => gameFinishedLevelFinishPortal;
        
        private void Awake()
        {
            for(int  i=0; i<checkPointController.Length; i++)
            {
                checkPointController[i].CurrentIndex = i;
            }

            
        }
    }
}