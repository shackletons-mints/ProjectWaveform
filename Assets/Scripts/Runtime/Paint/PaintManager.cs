using UnityEngine;
using UnityEngine.Rendering;
using Utilities;

public class PaintManager : Singleton<PaintManager>
{

    public Shader texturePaint;
    public Shader customPainterShader;
    public Shader extendIslands;

    int prepareUVID = Shader.PropertyToID("_PrepareUV");
    int positionID = Shader.PropertyToID("_PainterPosition");
    int hardnessID = Shader.PropertyToID("_Hardness");
    int strengthID = Shader.PropertyToID("_Strength");
    int radiusID = Shader.PropertyToID("_Radius");
    int blendOpID = Shader.PropertyToID("_BlendOp");
    int colorID = Shader.PropertyToID("_PainterColor");
    int textureID = Shader.PropertyToID("_MainTex");
    int uvOffsetID = Shader.PropertyToID("_OffsetUV");
    int uvIslandsID = Shader.PropertyToID("_UVIslands");

    public Material paintMaterial;
    public Material extendMaterial;

    CommandBuffer command;

    public override void Awake()
    {
        base.Awake();

        paintMaterial = new Material(texturePaint);
        extendMaterial = new Material(extendIslands);
        command = new CommandBuffer();
        command.name = "CommmandBuffer - " + gameObject.name;
    }

    public void InitTextures(Paintable paintable)
    {
        RenderTexture mask = paintable.getMask();
        RenderTexture uvIslands = paintable.getUVIslands();
        RenderTexture extend = paintable.getExtend();
        RenderTexture support = paintable.getSupport();
        Renderer rend = paintable.getRenderer();

        command.SetRenderTarget(mask);
        command.SetRenderTarget(extend);
        command.SetRenderTarget(support);

        paintMaterial.SetFloat(prepareUVID, 1);
        command.SetRenderTarget(uvIslands);
        command.DrawRenderer(rend, paintMaterial, 0);

        Graphics.ExecuteCommandBuffer(command);
        command.Clear();
    }


    public void Paint(Paintable paintable, Vector3 pos, float radius = 0.01f, float hardness = .5f, float strength = .5f, Color? color = null)
    {
        RenderTexture support = paintable.getSupport();

        paintMaterial.SetFloat("_Radius", radius);
        paintMaterial.SetTexture("_MainTex", support);
        paintMaterial.SetColor("_Color", color ?? Color.red);
        Graphics.ExecuteCommandBuffer(command);
        // Utilities.Helpers.DebugRenderTexture(mask, "mask_debug");
        // Utilities.Helpers.DebugRenderTexture(support, "support_debug");
        // Utilities.Helpers.DebugRenderTexture(extend, "extend_debug");
        command.Clear();
    }

}
