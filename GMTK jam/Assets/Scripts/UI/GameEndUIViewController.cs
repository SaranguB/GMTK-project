using UnityEngine;
using UnityEngine.UIElements;
using Utilis;

namespace UI
{
    public class GameEndUIViewController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup endScreenCanvas;
        public void EnableEndScreen()
        {
            CanvasGroupExtension.Show(endScreenCanvas);
        }
    }
}