using System.Collections.Generic;
using Main;
using UnityEngine;

namespace Level.Interactables.Button
{
    public class PortalButtonInteractable : MonoBehaviour, IInteractable
    {
        private LevelController levelController;
        private HashSet<GameObject> playersOnButton = new();

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (playersOnButton.Add(other.gameObject)) 
                {
                    OnInteracted();
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (playersOnButton.Remove(other.gameObject))
                {
                    if (playersOnButton.Count == 0)
                    {
                        OnStoppedInteracted();
                    }
                }
            }
        }

        public void OnInteracted()
        {
            levelController.LevelView.EnterPortalView.gameObject.SetActive(true);
        }

        public void OnStoppedInteracted()
        {
            levelController.LevelView.EnterPortalView.gameObject.SetActive(false);
        }

        public void SetController(LevelController levelController)
        {
            this.levelController = levelController;
        }
    }
}