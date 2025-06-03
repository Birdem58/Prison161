using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PSX
{
    [System.Serializable]
    [VolumeComponentMenu("Post-processing/PSX/Dithering")]
    public class Dithering : VolumeComponent, IPostProcessComponent
    {
        public ClampedFloatParameter ditherStrength = new ClampedFloatParameter(1f, 0f, 1f);
        public FloatParameter ditherThreshold = new FloatParameter(1f);
        public FloatParameter ditherScale = new FloatParameter(4f);
        public IntParameter patternIndex = new IntParameter(0);

        public bool IsActive() => ditherStrength.value > 0f;
        public bool IsTileCompatible() => false;
    }
}
