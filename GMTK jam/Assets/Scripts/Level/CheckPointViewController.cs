using System;
using UnityEngine;

namespace Level
{
    public class CheckPointViewController : MonoBehaviour
    {
        private LevelController levelController;
        public int CurrentIndex;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                levelController.OnCheckPointChanged(CurrentIndex);
            }
        }

        public void SetController(LevelController levelController)
        {
           this.levelController = levelController;
        }
    }
}