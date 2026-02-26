using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class OutlineColorRendererFeature : ScriptableRendererFeature
{
    [SerializeField] private Shader shader;
    private Material _material;
    private Pass _pass;

    public override void Create()
    {
        if (shader == null) shader = Shader.Find("Hidden/Custom/OutlineColor");
        if (shader == null) return;

        _material = CoreUtils.CreateEngineMaterial(shader);
        _pass = new Pass(_material);
        _pass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (_material != null) renderer.EnqueuePass(_pass);
    }

    protected override void Dispose(bool disposing) => CoreUtils.Destroy(_material);

    private class Pass : ScriptableRenderPass
    {
        private Material _mat;
        public Pass(Material mat) => _mat = mat;

        private class PassData { public TextureHandle src; }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var stack = VolumeManager.instance.stack.GetComponent<OutlineColorVolume>();
            if (stack == null || !stack.IsActive()) return;

            var resources = frameData.Get<UniversalResourceData>();
            var src = resources.activeColorTexture;
            if (!src.IsValid()) return;

            var desc = renderGraph.GetTextureDesc(src);
            desc.name = "OutlineColorTemp";
            desc.clearBuffer = false;
            var dst = renderGraph.CreateTexture(desc);

            using (var builder = renderGraph.AddRasterRenderPass<PassData>("Outline Color Pass", out var pd))
            {
                pd.src = src;
                builder.UseTexture(src, AccessFlags.Read);
                builder.SetRenderAttachment(dst, 0);

                builder.SetRenderFunc((PassData data, RasterGraphContext ctx) =>
                {
                    _mat.SetColor("_OutlineColor", stack.outlineColor.value);
                    _mat.SetColor("_TintColor", stack.tintColor.value);
                    _mat.SetFloat("_Threshold", stack.threshold.value);

                    Blitter.BlitTexture(ctx.cmd, data.src, new Vector4(1, 1, 0, 0), _mat, 0);
                });

                resources.cameraColor = dst;
            }
        }
    }
}