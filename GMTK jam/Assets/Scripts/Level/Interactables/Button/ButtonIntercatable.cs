using System;
using Main;
using UnityEngine;

namespace Level.Interactables.Button
{
    public class PortalButtonInteractable : MonoBehaviour, IInteractable
    {
        private LevelController levelController;
        
        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OnInteracted();
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            OnStoppedInteracted();
        }

        public void OnInteracted()
        {
           GameManager.Instance.EventService.OnPortalButtonInteracted.InvokeEvent();
        }

        public void OnStoppedInteracted()
        {
            
        }

        public void SetController(LevelController levelController)
        {
            this.levelController = levelController;
        }
    }
}