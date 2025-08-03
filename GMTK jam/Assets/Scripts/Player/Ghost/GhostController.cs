using Audio;
using Main;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using VFX;

namespace Player.Ghost
{
    public class GhostController
    {
        private GhostView ghostView;
        private GhostModel ghostModel;

        public GhostView GhostView => ghostView;
        public GhostModel GhostModel => ghostModel;

        public GhostController(GhostView ghostPrefab, GhostSO ghostData, Transform parentTransform)
        {
            this.ghostView = Object.Instantiate(ghostPrefab, parentTransform);
            ghostView.SetController(this);
            ghostModel = new GhostModel(ghostData);
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            GameManager.Instance.EventService.OnSwitchPlaced.AddListener(OnSwitchPlaced);
        }
        

        private void UnsubscribeFromEvents()
        {
            GameManager.Instance.EventService.OnSwitchPlaced.RemoveListener(OnSwitchPlaced);
        }

        private void OnSwitchPlaced(Transform alivePlayerPosition)
        {
            alivePlayerPosition.position = ghostView.transform.position;
            GameManager.Instance.EventService.OnGhostDestroyed.InvokeEvent();
        }

        public void GetInput()
        {
            GhostModel.InputVector = Vector2.zero;

            if (Keyboard.current.aKey.isPressed)
                GhostModel.InputVector.x -= 1f;
            if (Keyboard.current.dKey.isPressed)
                GhostModel.InputVector.x += 1f;
            if (Keyboard.current.wKey.isPressed)
                GhostModel.InputVector.y += 1f;
            if (Keyboard.current.sKey.isPressed)
                GhostModel.InputVector.y -= 1f;

            if (Mathf.Approximately(GhostModel.InputVector.x, 1))
                ghostView.transform.localScale = Vector3.one;
            else if (Mathf.Approximately(GhostModel.InputVector.x, -1))
                ghostView.transform.localScale = new Vector3(-1, 1, 1);

            GhostModel.InputVector = GhostModel.InputVector.normalized;
        }

        public void Move()
        {
            Vector2 currentPos = ghostView.GhostRB.position;
            Vector2 newPos = currentPos + GhostModel.InputVector * (GhostModel.GhostData.Speed * Time.fixedDeltaTime);

            // Clamp to boundary
            PolygonCollider2D boundary = ghostView.BoundaryCollider;
            Bounds bounds = boundary.bounds;

            float halfWidth = ghostView.GhostRB.GetComponent<Collider2D>().bounds.extents.x;
            float halfHeight = ghostView.GhostRB.GetComponent<Collider2D>().bounds.extents.y;

            newPos.x = Mathf.Clamp(newPos.x, bounds.min.x + halfWidth, bounds.max.x - halfWidth);
            newPos.y = Mathf.Clamp(newPos.y, bounds.min.y + halfHeight, bounds.max.y - halfHeight);

            ghostView.GhostRB.MovePosition(newPos);
        }

        public void SetGhost(PlayerController skeletonPlayer)
        {
            VFXService.Instance.PlayVFXAtPosition(ghostView.GhostSmokeVFX, ghostView.transform);
            ghostView.gameObject.SetActive(true);
            SoundManager.Instance.PlaySoundEffects(SoundType.PoofSound);
            RestrictGhost(skeletonPlayer);
        }

        private void RestrictGhost(PlayerController skeletonPlayer)
        {
            ghostView.transform.position = skeletonPlayer.PlayerView.transform.position;
        }
    }
}
