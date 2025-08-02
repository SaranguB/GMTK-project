using System;
using Main;
using UnityEngine;

namespace Level.Portals
{
    public class PortalViewController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            GameManager.Instance.EventService.OnLevelFinished.InvokeEvent();
        }
    }
}