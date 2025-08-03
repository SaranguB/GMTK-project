using Player;
using UnityEngine;
using Utilities;

namespace VFX
{
    public class VFXService : GenericMonoSingelton<VFXService>
    {
        private VFXPool vfxPool;

        public VFXService()
        {
            vfxPool = new VFXPool();
        }

        public VFXViewController PlayVFXAtPosition(VFXViewController vfx, Transform entityTransform)
        {
            vfxPool.SetVFX(vfx);
            VFXViewController vfxToPlay = vfxPool.GetItem();
            
            vfxToPlay.ConfigureVFX(entityTransform);
            return vfxToPlay;
        }

        public void ReturnVFXToPool(VFXViewController vfx) 
            => vfxPool.ReturnItem(vfx);
    }
}