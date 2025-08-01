using Main;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.StateMachine;

namespace Player.States
{
    public class AliveState : IState<PlayerController>
    {
        public PlayerController Owner { get; set; }
        
        public void OnEnter()
        {
            Owner.PlayerModel.RevivalButtonHoldTime = 0f;
            Owner.PlayerModel.RevivalActionTriggered = false;
        }

        public void OnExit() {}

        public void Tick()
        {
            GetInput();
            Move();
            Jump();
        }

        public void FixedTick()
        {
            
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
            
            ManageTimeSwitchPress();
            ManageRevivalPress();
        }

        private void ManageTimeSwitchPress()
        {
            if (Keyboard.current.fKey.isPressed)
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
            if (Keyboard.current.gKey.isPressed)
            {
                Owner.PlayerModel.RevivalButtonHoldTime += Time.deltaTime;

                if (!Owner.PlayerModel.RevivalActionTriggered && Owner.PlayerModel.RevivalButtonHoldTime >= Owner.PlayerModel.PlayerData.RevivalTime)
                {
                    GameManager.Instance.EventService.OnSkeltonRevived.InvokeEvent();
                    Owner.PlayerModel.RevivalActionTriggered = true;
                }
            }
            else
            {
                Owner.PlayerModel.RevivalButtonHoldTime = 0f;
                Owner.PlayerModel.RevivalActionTriggered = false;
            }
        }

        private void Move()
        {
            Vector3 move = new Vector2(
                Owner.PlayerModel.moveInput * Owner.PlayerModel.PlayerData.StateDataDict[PlayerState.AliveState].Speed,
                0f
            );
            Owner.PlayerView.transform.position += move * Time.deltaTime;
        }

        private void Jump()
        {
            if (Owner.PlayerModel.JumpPressed && Owner.PlayerModel.IsGrounded)
            {
                Owner.PlayerView.PlayerRB.linearVelocity = new Vector2(
                    Owner.PlayerView.PlayerRB.linearVelocity.x,
                    Owner.PlayerModel.PlayerData.StateDataDict[PlayerState.AliveState].JumpForce
                );
            }
        }

        private void ReviveToGhost()
        {
           
        }
    }
}
