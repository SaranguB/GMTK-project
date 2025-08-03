using System;
using Level.Interactables;
using Main;
using UnityEngine;

namespace Level.Portals
{
    public class LevelFInishPortalViewController : MonoBehaviour, IInteractable
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
            GameManager.Instance.UIService.InGameUIViewController.ResetGhostReviveFill();
        }

        public void OnStoppedInteracted()
        {
            
        }
    }
}