using Main;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.StateMachine;
using VFX;

namespace Player.States
{
    public class AliveState : IState<PlayerController>
    {
        public PlayerController Owner { get; set; }
        
        public void OnEnter()
        {
            Owner.PlayerView.SpriteRenderer.sprite =
                Owner.PlayerModel.PlayerData.StateDataDict[PlayerState.AliveState].PlayerSprite;
            Owner.PlayerModel.RevivalButtonHoldTime = 0f;
            Owner.PlayerModel.RevivalActionTriggered = false;
        }

        public void OnExit()
        {
          
        }

        public void Tick()
        {
            if (GameManager.Instance.CurrentState != GameStates.Gameplay) return;
            
            GetInput();
            Move();
            Jump();
        }

        private void GetInput()
        {
            Owner.PlayerModel.moveInput = 0;
            Owner.PlayerModel.JumpPressed = false;

            if (Keyboard.current.leftArrowKey.isPressed)
                Owner.PlayerModel.moveInput -= 1f;

            if (Keyboard.current.rightArrowKey.isPressed )
                Owner.PlayerModel.moveInput += 1f;

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                Owner.PlayerModel.JumpPressed = true;
            
            if (Mathf.Approximately(Owner.PlayerModel.moveInput, 1))
            {
                Owner.PlayerView.transform.localScale = Vector3.one;
            }
            else if (Mathf.Approximately(Owner.PlayerModel.moveInput, -1))
            {
                Vector3 scale = Owner.PlayerView.transform.localScale;
                scale.x = -1;
                Owner.PlayerView.transform.localScale = scale;
            }
            
            ManageSwitchPlacePress();
            ManageRevivalPress();
        }

        private void ManageSwitchPlacePress()
        {
            if (Keyboard.current.fKey.isPressed && Owner.GhostController != null)
            {
                Owner.PlayerModel.SwitchPlaceButtonHoldTimer += Time.deltaTime;

                if (!Owner.PlayerModel.SwitchPlaceActionTriggered && Owner.PlayerModel.SwitchPlaceButtonHoldTimer >= Owner.PlayerModel.PlayerData.SwitchPlaceButtonHoldTime)
                {
                    
                    GameManager.Instance.EventService.OnSwitchPlaced.InvokeEvent(Owner.PlayerView.transform); 
                    Owner.PlayerModel.SwitchPlaceActionTriggered = true;
                }
            }
            else
            {
                Owner.PlayerModel.SwitchPlaceButtonHoldTimer = 0f;
                Owner.PlayerModel.SwitchPlaceActionTriggered = false;
            }
        }

        private void ManageRevivalPress()
        {
            if (GameManager.Instance.PlayerManager.SpawnedPlayers.ContainsKey(PlayerState.SkeletonState))
            {
                if (Keyboard.current.gKey.isPressed && Owner.GhostController == null)
                {
                    if (!Owner.PlayerView.GhostRevivingVFX.ParticleSystem.isPlaying)
                    {
                        Owner.PlayerView.GhostRevivingVFX.ParticleSystem.Play();
                    }

                    Owner.PlayerModel.RevivalButtonHoldTime += Time.deltaTime;
                    Owner.PlayerView.SpriteRenderer.sprite = Owner.PlayerModel.PlayerData.PlayerReviveSprite;
                    GameManager.Instance.UIService.InGameUIViewController.SetGhostReviveFill
                        (Owner.PlayerModel.RevivalButtonHoldTime / Owner.PlayerModel.PlayerData.RevivalTime);

                    if (!Owner.PlayerModel.RevivalActionTriggered && Owner.PlayerModel.RevivalButtonHoldTime >=
                        Owner.PlayerModel.PlayerData.RevivalTime)
                    {
                        Owner.PlayerView.GhostRevivingVFX.ParticleSystem.Stop();
                        GameManager.Instance.EventService.OnSkeltonRevived.InvokeEvent();
                        Owner.PlayerModel.RevivalActionTriggered = true;
                        Owner.PlayerView.SpriteRenderer.sprite = Owner.PlayerModel.PlayerData
                            .StateDataDict[PlayerState.AliveState].PlayerSprite;
                    }
                }
                else
                {
                    Owner.PlayerView.GhostRevivingVFX.ParticleSystem.Stop();
                    Owner.PlayerView.SpriteRenderer.sprite = Owner.PlayerModel.PlayerData
                        .StateDataDict[PlayerState.AliveState].PlayerSprite;
                    Owner.PlayerModel.RevivalButtonHoldTime = 0f;
                    Owner.PlayerModel.RevivalActionTriggered = false;
                }
            }
        }

        private void Move()
        {
            if (Owner.GhostController == null)
            {
                Vector3 move = new Vector2(
                    Owner.PlayerModel.moveInput *
                    Owner.PlayerModel.PlayerData.StateDataDict[PlayerState.AliveState].Speed,
                    0f
                );
                Owner.PlayerView.transform.position += move * Time.deltaTime;
            }
        }

        private void Jump()
        {
            if (Owner.PlayerModel.JumpPressed && Owner.PlayerModel.IsGrounded)
            {
                float jumpForce = Owner.PlayerModel.PlayerData.StateDataDict[PlayerState.AliveState].JumpForce;
                Owner.PlayerView.PlayerRB.linearVelocity = new Vector2(
                    Owner.PlayerView.PlayerRB.linearVelocity.x,
                    jumpForce
                );
                Debug.Log($"Jump executed! Force: {jumpForce}, Current velocity: {Owner.PlayerView.PlayerRB.linearVelocity}");
            }
            else if (Owner.PlayerModel.JumpPressed && !Owner.PlayerModel.IsGrounded)
            {
                Debug.Log("Jump pressed but player is not grounded!");
            }
        }

        private void ReviveToGhost()
        {
           
        }
    }
}
