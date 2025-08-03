using Level;
using Main;
using Player.Ghost;
using Player.StateMachine;
using Player.States;
using UnityEngine;

namespace Player
{
    public class PlayerController
    {
        private PlayerView playerView;
        private PlayerModel playerModel;
        private PlayerStateMachine playerStateMachine;
        public GhostController GhostController;
        private GhostPool ghostPool;
        private Transform currentCheckPoint;
        
        public PlayerView PlayerView => playerView;
        public PlayerModel PlayerModel => playerModel;
        public PlayerStateMachine PlayerStateMachine => playerStateMachine;
        
        public PlayerController(PlayerView playerPrefab, PlayerSO playerData, Transform parentTransform, GhostPool ghostPool = null)
        {
            this.playerView = Object.Instantiate(playerPrefab, parentTransform);
            playerView.SetController(this);
            playerModel = new PlayerModel(playerData);
            this.ghostPool = ghostPool;
            CreateStateMachine();
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            GameManager.Instance.EventService.OnGhostDestroyed.AddListener(onGhostDestroyed);
        }
        

        private void UnsubscribeFromEvents()
        {
            GameManager.Instance.EventService.OnGhostDestroyed.RemoveListener(onGhostDestroyed);
        }
        
        private void onGhostDestroyed(GhostController ghost)
        {
            ghostPool.ReturnItem(ghost);
            GhostController = null;
        }

        public void OnPlayerDied()
        {
            playerStateMachine.ChangeState(PlayerState.SkeletonState);
            GameManager.Instance.EventService.OnPlayerDied.InvokeEvent(this);
            
        }

        private void CreateStateMachine()
        {
            playerStateMachine = new PlayerStateMachine(this);
            playerStateMachine.ChangeState(PlayerState.AliveState);
        }

        public void EnablePlayer(bool value)
        {
            
        }

        public void SetPlayer(Transform checkpoint)
        {
           playerView.transform.position = checkpoint.position;
        }

        public GhostController CreateGhost(PlayerController skeletonPlayer)
        {
            GhostController = this.ghostPool.GetItem();
            GhostController.SetGhost(skeletonPlayer);
            return GhostController;
        }
    }
}