using System;
using Cinemachine;
using Main;
using Player.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Ghost
{
    public class GhostView : MonoBehaviour
    {
        private PolygonCollider2D boundaryCollider;
        
        private GhostController ghostController;
        public Rigidbody2D GhostRB;
       
        public PolygonCollider2D BoundaryCollider => boundaryCollider;

        private void Awake()
        {
            boundaryCollider = GameObject.FindWithTag("Boundary").GetComponent<PolygonCollider2D>();
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentState != GameStates.Gameplay) return;
            
            ghostController.GetInput();
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.CurrentState != GameStates.Gameplay) return;
            
            ghostController.Move();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Laser"))
            {
                GameManager.Instance.EventService.OnGhostDestroyed.InvokeEvent();
            }
        }

        public void SetController(GhostController ghostController)
        {
            this.ghostController = ghostController;
        }
        
    }
}