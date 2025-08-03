using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Cinemachine;
using Level;
using Main;
using Player.Ghost;
using Player.States;
using UnityEngine;
using UnityEngine.VFX;
using VFX;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerView playerPrefab;
        [SerializeField] private PlayerSO playerData;
        [SerializeField] private Transform ghostParent;
        [SerializeField] private GhostView ghostPrefab;
        [SerializeField] private GhostSO ghostData;
        [SerializeField] private CinemachineVirtualCamera playerCamera;
        
        private PlayerPool playerPool;
        private GhostPool ghostPool;
        private GhostController currentGhost;
        private Transform currentCheckPoint;
        private Coroutine ghostReviveCoroutine;
        
        private Dictionary<PlayerState, Queue<PlayerController>> spawnedPlayers = new();

        public Dictionary<PlayerState, Queue<PlayerController>> SpawnedPlayers => spawnedPlayers;
        private void Awake()
        {
            SubscribeToEvents();
        }

        private void Start()
        {
            ghostPool = new GhostPool(ghostPrefab, ghostParent, ghostData);
            playerPool = new PlayerPool(playerPrefab, playerData, this.transform, ghostPool);
            SpawnPlayer();
        }

        private void Update()
        {
            foreach (var playerQueue in new List<Queue<PlayerController>>(spawnedPlayers.Values))
            {
                PlayerController[] players = playerQueue.ToArray(); 
                foreach (var player in players)
                {
                    player.PlayerStateMachine.Tick();
                }
            }
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            GameManager.Instance.EventService.OnPlayerDied.AddListener(OnPlayerDied);
            GameManager.Instance.EventService.OnSwitchPlaced.AddListener(OnSwitchPlaced);
            GameManager.Instance.EventService.OnSkeltonRevived.AddListener(OnSkeletonRevived);
            GameManager.Instance.EventService.OnCheckPointChanged.AddListener(OnCheckPointChanged);
            GameManager.Instance.EventService.OnLevelFinished.AddListener(OnLevelFinished);
            GameManager.Instance.EventService.OnNextLevelCreated.AddListener(OnnextLevelCreated);
            GameManager.Instance.EventService.OnGhostDestroyed.AddListener(OnGhostDestroyed);
        }

        private void OnSwitchPlaced(Transform obj)
        {
            playerCamera.Follow =  spawnedPlayers[PlayerState.AliveState].Peek().PlayerView.transform;
            OnGhostDestroyed();
        }

        private void OnnextLevelCreated()
        {
           SpawnPlayer();
        }

        private void UnsubscribeFromEvents()
        {
            GameManager.Instance.EventService.OnPlayerDied.RemoveListener(OnPlayerDied);
            GameManager.Instance.EventService.OnSkeltonRevived.RemoveListener(OnSkeletonRevived);
            GameManager.Instance.EventService.OnCheckPointChanged.RemoveListener(OnCheckPointChanged);
            GameManager.Instance.EventService.OnLevelFinished.RemoveListener(OnLevelFinished);
            GameManager.Instance.EventService.OnNextLevelCreated.RemoveListener(OnnextLevelCreated);
            GameManager.Instance.EventService.OnGhostDestroyed.RemoveListener(OnGhostDestroyed);
        }

        private void OnLevelFinished()
        {
                // 1. Remove alive players
                if (spawnedPlayers.TryGetValue(PlayerState.AliveState, out var aliveQueue))
                {
                    while (aliveQueue.Count > 0)
                    {
                        PlayerController player = aliveQueue.Dequeue();
                        playerPool.ReturnItem(player);
                    }
                }

                // 2. Remove skeleton players
                if (spawnedPlayers.TryGetValue(PlayerState.SkeletonState, out var skeletonQueue))
                {
                    while (skeletonQueue.Count > 0)
                    {
                        PlayerController skeleton = skeletonQueue.Dequeue();
                        playerPool.ReturnItem(skeleton);
                    }
                }

                // 3. Clear ghost if any
                if (currentGhost != null)
                {
                    ghostPool.ReturnItem(currentGhost);
                    currentGhost = null;
                }

                // 4. Clear all player queues
                spawnedPlayers.Clear();

                // 5. Reset checkpoint reference
                currentCheckPoint = null;
        }

        private void OnCheckPointChanged(CheckPointViewController checkPoint)
        {
            if (checkPoint == null)
            {
                return;
            }
            
            currentCheckPoint = checkPoint.transform;
        }

        private void OnSkeletonRevived()
        {
            PlayerController alivePlayer;
            
            if (spawnedPlayers.TryGetValue(PlayerState.AliveState, out var aliveQueue) && aliveQueue.Count > 0)
            {
                alivePlayer = aliveQueue.Peek();
                
                if (spawnedPlayers.TryGetValue(PlayerState.SkeletonState, out var skeletonQueue) && skeletonQueue.Count > 0)
                {
                    PlayerController skeletonPlayer = skeletonQueue.Peek();
                    currentGhost = alivePlayer.CreateGhost(skeletonPlayer);
                    playerCamera.Follow = currentGhost.GhostView.transform;
                    
                    // Stop any existing ghost revive coroutine
                    if (ghostReviveCoroutine != null)
                    {
                        StopCoroutine(ghostReviveCoroutine);
                    }
                    ghostReviveCoroutine = StartCoroutine(RemoveGhostInSeconds(currentGhost));
                }
                
            }
        }

        private IEnumerator RemoveGhostInSeconds(GhostController ghost)
        {
            float duration = 5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                if (currentGhost != ghost)
                {
                    yield break;
                }
                elapsed += Time.deltaTime;
                float remaining = Mathf.Clamp01(1f - (elapsed / duration));

                GameManager.Instance.UIService.InGameUIViewController.SetGhostReviveFill(remaining);
                yield return null;
            }
            
            if(currentGhost == ghost)
             OnGhostDestroyed();
        }

        private void OnGhostDestroyed()
        {
            // Stop the ghost revive coroutine if it's running
            if (ghostReviveCoroutine != null)
            {
                StopCoroutine(ghostReviveCoroutine);
                ghostReviveCoroutine = null;
            }
            
            PlayerController alivePlayer = spawnedPlayers[PlayerState.AliveState].Peek();
            if (alivePlayer != null)
            {
                if (alivePlayer.GhostController != null)
                {
                    VFXService.Instance.PlayVFXAtPosition(alivePlayer.GhostController.GhostView.GhostSmokeVFX,
                        alivePlayer.GhostController.GhostView.transform);
                    SoundManager.Instance.PlaySoundEffects(SoundType.PoofSound);
                    alivePlayer.OnGhostDestroyed();
                    playerCamera.Follow = alivePlayer.PlayerView.transform;
                    GameManager.Instance.UIService.InGameUIViewController.ResetGhostReviveFill();
                }
            }
        }

        private void OnPlayerDied(PlayerController playerController)
        {
            SoundManager.Instance.PlaySoundEffects(SoundType.DeathSound);
            if (spawnedPlayers.ContainsKey(PlayerState.AliveState))
            {
                spawnedPlayers[PlayerState.AliveState].Clear(); 
            }

            if (!spawnedPlayers.ContainsKey(PlayerState.SkeletonState))
            {
                spawnedPlayers[PlayerState.SkeletonState] = new Queue<PlayerController>();
            }
            
            spawnedPlayers[PlayerState.SkeletonState].Enqueue(playerController);
            StartCoroutine(RemoveSkeletonInSeconds(spawnedPlayers[PlayerState.SkeletonState].Peek()));
            SpawnPlayer();
        }

        private IEnumerator RemoveSkeletonInSeconds(PlayerController skeletonPlayer)
        {
            yield return new WaitForSeconds(10);
            if (spawnedPlayers.TryGetValue(PlayerState.SkeletonState, out var skeletonQueue) &&
                skeletonQueue.Count > 0 &&
                skeletonQueue.Peek() == skeletonPlayer)
            {
                skeletonPlayer.PlayerStateMachine.ChangeState(PlayerState.AliveState);
                playerPool.ReturnItem(skeletonPlayer);
                skeletonQueue.Dequeue();
            }
        }

        private void SpawnPlayer()
        {
            PlayerController player = playerPool.GetItem();

            if (player.PlayerStateMachine.CurrentState is SkeletonState)
            {
                player.PlayerStateMachine.ChangeState(PlayerState.AliveState);    
            }
            
            if (!spawnedPlayers.ContainsKey(PlayerState.AliveState))
                spawnedPlayers[PlayerState.AliveState] = new Queue<PlayerController>();
            
            spawnedPlayers[PlayerState.AliveState].Enqueue(player);
            player.SetPlayer(currentCheckPoint);
            playerCamera.Follow = player.PlayerView.transform;
        }
    }
}
