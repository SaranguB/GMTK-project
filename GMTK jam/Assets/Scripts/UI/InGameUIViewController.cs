using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InGameUIViewController : MonoBehaviour
    {
        [SerializeField] private Image ghostImage;
        [SerializeField] private CanvasGroup inGameUICanvas;
        
        public CanvasGroup IngameUICanvas => inGameUICanvas;
        
        public void SetGhostReviveFill(float fillPercent)
        {
            ghostImage.fillAmount = Mathf.Clamp01(fillPercent);
        }
        
        public void ResetGhostReviveFill()
        {
            ghostImage.fillAmount = 0f;
        }

    }
}