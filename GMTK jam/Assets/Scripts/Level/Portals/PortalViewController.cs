using System;
using Level.Interactables;
using Main;
using UnityEngine;

namespace Level.Portals
{
    public class PortalViewController : MonoBehaviour, IInteractable
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
            GameManager.Instance.EventService.OnLevelFinished.InvokeEvent();
        }

        public void OnStoppedInteracted()
        {
            
        }
    }
}