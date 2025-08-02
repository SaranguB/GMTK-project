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
        [SerializeField] private PortalViewController EnterPortal;
        [SerializeField] private PortalViewController ExitPortal;
        
        public CheckPointViewController[] CheckPointController => checkPointController;
        public PortalButtonInteractable PortalButton => portalButton;
        public PortalViewController EnterPortalView => EnterPortal;
        public PortalViewController ExitPortalView => ExitPortal;
        
        private void Awake()
        {
            for(int  i=0; i<checkPointController.Length; i++)
            {
                checkPointController[i].CurrentIndex = i;
            }

            
        }
    }
}