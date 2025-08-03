using Level.Interactables;
using Main;
using UI;
using UnityEngine;

namespace Level.Portals
{
    public class GameeFinishPortalViewController : MonoBehaviour, IInteractable
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                OnInteracted();
            }
        }

        public void OnInteracted()
        {
            GameManager.Instance.UIService.GameEndUIController.EnableEndScreen();
            //GameManager.Instance.EventService.OnLevelFinished.InvokeEvent();
        }

        public void OnStoppedInteracted()
        {
            
        }
    }
}