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
            if (levelController.LevelView.EnterLevelFInishPortalView != null)
            {
                levelController.LevelView.EnterLevelFInishPortalView.gameObject.SetActive(true);
            }

            if (levelController.LevelView.GameFinishedLevelFinishPortalView != null)
            {
                levelController.LevelView.GameFinishedLevelFinishPortalView.gameObject.SetActive(true);
            }
           
        }

        public void OnStoppedInteracted()
        {
            if (levelController.LevelView.EnterLevelFInishPortalView != null)
            {
                levelController.LevelView.EnterLevelFInishPortalView.gameObject.SetActive(false);
            }

            if (levelController.LevelView.GameFinishedLevelFinishPortalView != null)
            {
                levelController.LevelView.GameFinishedLevelFinishPortalView.gameObject.SetActive(false);
            }
        }

        public void SetController(LevelController levelController)
        {
            this.levelController = levelController;
        }
    }
}