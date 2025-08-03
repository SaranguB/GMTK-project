using System;
using UnityEngine;
using UnityEngine.Pool;
using Utilities;
using VFX;
using Object = UnityEngine.Object;

namespace Player
{
    public class VFXPool : GenericObjectPool<VFXViewController>
    {
        private VFXViewController vfxPrefab;

        public VFXPool()
        {
        }
        
        protected override VFXViewController CreateItem()
        {
            return Object.Instantiate(vfxPrefab);
        }

        protected override void OnGet(VFXViewController vfx)
        {
           vfx.gameObject.SetActive(true);
        }

        protected override void OnRelease(VFXViewController vfx)
        {
            vfx.gameObject.SetActive(false);

        }

        public void SetVFX(VFXViewController vfx)
        {
            vfxPrefab = vfx;
        }
    }
}