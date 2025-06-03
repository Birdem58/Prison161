using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PSX
{
    public class DitheringRenderFeature : ScriptableRendererFeature
    {
        class DitheringPass : ScriptableRenderPass
        {
            static readonly string k_Tag = "Dithering Effect";
            static readonly int TempTargetId = Shader.PropertyToID("_TempDitherTarget");

            static readonly int DitherStrengthID = Shader.PropertyToID("_DitherStrength");
            static readonly int DitherThresholdID = Shader.PropertyToID("_DitherThreshold");
            static readonly int DitherScaleID = Shader.PropertyToID("_DitherScale");
            static readonly int PatternIndexID = Shader.PropertyToID("_PatternIndex");

            Material material;

            public DitheringPass()
            {
                var shader = Shader.Find("PostEffect/Dithering");
                if (shader != null)
                    material = CoreUtils.CreateEngineMaterial(shader);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (material == null || !renderingData.cameraData.postProcessEnabled)
                    return;

                var stack = VolumeManager.instance.stack;
                var settings = stack.GetComponent<Dithering>();
                if (settings == null || !settings.IsActive())
                    return;

                CommandBuffer cmd = CommandBufferPool.Get(k_Tag);

                RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTarget;
                int w = renderingData.cameraData.camera.scaledPixelWidth;
                int h = renderingData.cameraData.camera.scaledPixelHeight;

                material.SetFloat(DitherStrengthID, settings.ditherStrength.value);
                material.SetFloat(DitherThresholdID, settings.ditherThreshold.value);
                material.SetFloat(DitherScaleID, settings.ditherScale.value);
                material.SetInt(PatternIndexID, settings.patternIndex.value);

                cmd.GetTemporaryRT(TempTargetId, w, h, 0, FilterMode.Point, RenderTextureFormat.Default);
                cmd.Blit(source, TempTargetId);
                cmd.Blit(TempTargetId, source, material);
                cmd.ReleaseTemporaryRT(TempTargetId);

                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }
        }

        DitheringPass pass;

        public override void Create()
        {
            pass = new DitheringPass
            {
                renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing
            };
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(pass);
        }
    }
}
