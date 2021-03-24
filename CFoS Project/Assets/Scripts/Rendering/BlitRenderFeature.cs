/* Based on this article by Sam Driver: https://samdriver.xyz/article/scriptable-render */

using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlitRenderFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class MyFeatureSettings
    {
        public bool IsEnabled = true;
        public RenderPassEvent WhenToInsert = RenderPassEvent.AfterRendering;
        public Material MaterialToBlit;
    }

    public MyFeatureSettings settings = new MyFeatureSettings(); // MUST be named "settings" (or it wont show up)

    private RenderTargetHandle _renderTextureHandle;
    private BlitRenderPass _renderPass;

    public override void Create()
    {
        _renderPass = new BlitRenderPass(
          "My custom pass",
          settings.WhenToInsert,
          settings.MaterialToBlit
        );
    }

    // called every frame once per camera
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (!settings.IsEnabled) return;

        // pass camera's color target to the render pass
        var cameraColorTargetIdent = renderer.cameraColorTarget;
        _renderPass.Setup(cameraColorTargetIdent);

        renderer.EnqueuePass(_renderPass);
    }
}