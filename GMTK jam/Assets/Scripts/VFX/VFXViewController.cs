using Main;
using UnityEngine;

namespace VFX
{
    public class VFXViewController : MonoBehaviour
    {
        [SerializeField] ParticleSystem particleSystem;
        private Transform entityTransform;
        
        public ParticleSystem ParticleSystem => particleSystem;
        
        private void Update()
        {
            if (!particleSystem.main.loop)
            {
                StopNonLoopingVFX();
            }
            SetVFXPosition();
        }

        private void SetVFXPosition()
        {
            if(entityTransform == null)return;
           transform.position = entityTransform.position;
        }

        public void ConfigureVFX(Transform entityTransform)
        {
            this.entityTransform = entityTransform;
            particleSystem.Play();
        }
        
        private void StopNonLoopingVFX()
        {
            if (!this.gameObject.activeInHierarchy) return;
            
            if (!particleSystem.isPlaying && !particleSystem.IsAlive())
            {
                VFXService.Instance.ReturnVFXToPool(this);
            }
        }

        public void PlayVFX()
        {
            if (!particleSystem.isPlaying && !particleSystem.IsAlive())
            {
                particleSystem.Play();
            }
        }

        public void StopVFX()
        {
            if (particleSystem.isPlaying)
            {
                particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
        
        

    }
}