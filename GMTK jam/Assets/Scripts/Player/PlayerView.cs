using System;
using Cinemachine;
using Main;
using Player.Ghost;
using Player.States;
using UnityEngine;

namespace Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Rigidbody2D playerRB;
        [SerializeField] private Transform[] groundCheckPoint;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private BoxCollider2D playerCollider;
        private PolygonCollider2D boundaryCollider;
        
        private PlayerController playerController;
        private Collider2D[] colliders;

        public SpriteRenderer SpriteRenderer => spriteRenderer;
        public Rigidbody2D PlayerRB => playerRB;
        public BoxCollider2D PlayerCollider => playerCollider;


        private void Awake()
        { 
            boundaryCollider = GameObject.FindWithTag("Boundary").GetComponent<PolygonCollider2D>();
        }

        public void SetController(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Spike"))
            {
                if (playerController.PlayerStateMachine.CurrentState is AliveState)
                {
                    if (playerController.GhostController != null)
                    {
                        GameManager.Instance.EventService.OnGhostDestroyed.InvokeEvent(playerController.GhostController);
                    }
                    playerController.OnPlayerDied();
                }
            }
        }

        private void FixedUpdate()
        {
            playerController.PlayerModel.IsGrounded = false;

            foreach (var point in groundCheckPoint)
            {
                colliders = Physics2D.OverlapCircleAll(point.position, 0.1f, groundLayer);

                foreach (var col in colliders)
                {
                    if (col.gameObject != this.gameObject)
                    {
                        playerController.PlayerModel.IsGrounded = true;
                        break;
                    }
                }

                if (playerController.PlayerModel.IsGrounded)
                    break;
            }
        }
    }
}