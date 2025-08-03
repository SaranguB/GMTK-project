using UnityEngine;
using VFX;

namespace Player
{
    public class PlayerModel
    {
        private PlayerSO playerData;
        
        public bool IsGrounded;
        public float moveInput;
        public Vector2 InputVector;
        
        public PlayerSO PlayerData => playerData;
        public VFXViewController currentVFX;

        public bool SwitchPlaceActionTriggered = false;

        public float SwitchPlaceButtonHoldTimer = 0f;

        public bool RevivalActionTriggered = false;
        public float RevivalButtonHoldTime = 0f;
        public bool JumpPressed = false;
        
        public PlayerModel(PlayerSO playerSo)
        {
           this.playerData = playerSo;
        }
    }
}