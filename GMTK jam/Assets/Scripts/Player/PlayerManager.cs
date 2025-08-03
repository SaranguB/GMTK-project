using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Level;
using Main;
using Player.Ghost;
using Player.States;
using UnityEngine;

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
        
        private Dictionary<PlayerState, Queue<PlayerController>> spawnedPlayers = new();

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
                foreach (var player in playerQueue)
                {
                    player.PlayerStateMachine.Tick();
                }
            }
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void FixedUpdate()
        {
            foreach (var playerQueue in new List<Queue<PlayerController>>(spawnedPlayers.Values))
            {
                foreach (var player in playerQueue)
                {
                    player.PlayerStateMachine.FixedTick();
                }
            }
        }

        private void SubscribeToEvents()
        {
            GameManager.Instance.EventService.OnPlayerDied.AddListener(OnPlayerDied);
            GameManager.Instance.EventService.OnSwitchPlaced.AddListener(OnSwitchPlaced);
            GameManager.Instance.EventService.OnSkeltonRevived.AddListener(OnSkeletonRevived);
            GameManager.Instance.EventService.OnCheckPointChanged.AddListener(OnCheckPointChanged);
            GameManager.Instance.EventService.OnLevelFinished.AddListener(OnLevelFinished);
            GameManager.Instance.EventService.OnNextLevelCreated.AddListener(OnnextLevelCreated);
        }

        private void OnSwitchPlaced(Transform obj)
        {
            playerCamera.Follow =  spawnedPlayers[PlayerState.AliveState].Peek().PlayerView.transform;
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
                    
                    StartCoroutine(RemoveGhostInSeconds(alivePlayer));
                }
                
            }
        }

        private IEnumerator RemoveGhostInSeconds(PlayerController alivePlayer)
        {
           yield return new WaitForSeconds(10);
           
           if (alivePlayer != null)
           {
               if (alivePlayer.GhostController != null)
               {
                   ghostPool.ReturnItem(alivePlayer.GhostController);
                   alivePlayer.GhostController = null;
                   playerCamera.Follow = alivePlayer.PlayerView.transform;
               }
           }
        }

        private void OnPlayerDied(PlayerController playerController)
        {
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
            yield return new WaitForSeconds(5);
            skeletonPlayer.PlayerStateMachine.ChangeState(PlayerState.AliveState);
            playerPool.ReturnItem(skeletonPlayer);
            spawnedPlayers[PlayerState.SkeletonState].Dequeue();
        }

        private void SpawnPlayer()
        {
            PlayerController player = playerPool.GetItem();

            if (!spawnedPlayers.ContainsKey(PlayerState.AliveState))
                spawnedPlayers[PlayerState.AliveState] = new Queue<PlayerController>();
            
            spawnedPlayers[PlayerState.AliveState].Enqueue(player);
            player.SetPlayer(currentCheckPoint);
            playerCamera.Follow = player.PlayerView.transform;
        }
    }
}
