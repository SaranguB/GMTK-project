using System;
using UnityEngine;

namespace Level
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private CheckPointViewController[] checkPointController;
        public CheckPointViewController[] CheckPointController => checkPointController;

        private void Awake()
        {
            for(int  i=0; i<checkPointController.Length; i++)
            {
                checkPointController[i].CurrentIndex = i;
            }
        }
    }
}