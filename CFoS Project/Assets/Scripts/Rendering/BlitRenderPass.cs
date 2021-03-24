/* Based on this article by Sam Driver: https://samdriver.xyz/article/scriptable-render */

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

class BlitRenderPass : ScriptableRenderPass
{
    // used to label this pass in Unity's Frame Debug utility
    private string _profilerTag;

    private Material _materialToBlit;
    private RenderTargetIdentifier _cameraColorTargetIdent;
    private RenderTargetHandle _tempTexture;


    public BlitRenderPass(string profilerTag, RenderPassEvent renderEvent, Material materialToBlit)
    {
        _profilerTag = profilerTag;
        _materialToBlit = materialToBlit;
        renderPassEvent = renderEvent;
    }

    // Set CameraTargetColor for the pass to use
    public void Setup(RenderTargetIdentifier cameraColorTargetIdent)
    {
        _cameraColorTargetIdent = cameraColorTargetIdent;
    }

    // called each frame before Execute (used it to prepare things the pass will need)
    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        // set temporary render texture
        cmd.GetTemporaryRT(_tempTexture.id, cameraTextureDescriptor);
    }

    // called for every eligible camera every frame. (cant directly execute rendering commands tho)
    // Use the methods on ScriptableRenderContext to set up instructions.
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get(_profilerTag);
        cmd.Clear();

        // blit to temp texture, apply material and blit back
        cmd.Blit(_cameraColorTargetIdent, _tempTexture.Identifier(), _materialToBlit, 0);
        cmd.Blit(_tempTexture.Identifier(), _cameraColorTargetIdent);

        context.ExecuteCommandBuffer(cmd);

        // tidy up
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    // called after Execute (used to clean up anything allocated in Configure)
    public override void FrameCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(_tempTexture.id);
    }
}
