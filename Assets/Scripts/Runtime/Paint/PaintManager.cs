using UnityEngine;
using UnityEngine.Rendering;
using Utilities;

public class PaintManager : Singleton<PaintManager>
{

    public Shader texturePaint;
    public Shader extendIslands;

    int prepareUVID = Shader.PropertyToID("_PrepareUV");
	int paintCenterID = Shader.PropertyToID("_PainterCenter");
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
        RenderTexture mask = paintable.getMask();
        RenderTexture uvIslands = paintable.getUVIslands();
        RenderTexture extend = paintable.getExtend();
        RenderTexture support = paintable.getSupport();
        Renderer rend = paintable.getRenderer();

        // Get the Renderer & Mesh
        MeshCollider meshCollider = paintable.GetComponent<MeshCollider>();
        Mesh mesh = meshCollider.sharedMesh;
        Renderer renderer = paintable.getRenderer();

        RaycastHit hit;
        if (Physics.Raycast(pos + Vector3.up * 0.1f, Vector3.down, out hit))
        {
			paintMaterial.SetVector(positionID, hit.point);
			extendMaterial.SetVector(paintCenterID, hit.point);

			// create object to track collision point
			// GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			// marker.transform.position = hit.point;
			// marker.transform.localScale = Vector3.one * 0.2f;
			// Destroy(marker, 2f);

            // Vector2 uv = hit.textureCoord;
            // Debug.Log("UV: " + uv);
            // paintMaterial.SetVector(positionID, new Vector4(uv.x, uv.y, 0, 0)); // Store UV in xy
        }

        paintMaterial.SetFloat(prepareUVID, 0);
        paintMaterial.SetFloat(hardnessID, hardness);
        paintMaterial.SetFloat(strengthID, strength);
        paintMaterial.SetFloat(radiusID, radius);
        paintMaterial.SetTexture(textureID, support);
        paintMaterial.SetColor(colorID, color ?? Color.red);

        extendMaterial.SetFloat(uvOffsetID, paintable.extendsIslandOffset);
        extendMaterial.SetTexture(uvIslandsID, uvIslands);
		extendMaterial.SetFloat("_MaxExtendDistance", 0.1f);

        // command.SetRenderTarget(mask);
        // command.ClearRenderTarget(true, true, color ?? Color.red);
        // Graphics.ExecuteCommandBuffer(command);
        // command.Clear();

        command.SetRenderTarget(mask);
        command.Blit(null, mask, paintMaterial);
        command.SetRenderTarget(support);
        command.Blit(mask, support);

        command.SetRenderTarget(extend);
        command.Blit(mask, extend, extendMaterial);

        Graphics.ExecuteCommandBuffer(command);
        // Utilities.Helpers.DebugRenderTexture(mask, "mask_debug");
        // Utilities.Helpers.DebugRenderTexture(support, "support_debug");
        // Utilities.Helpers.DebugRenderTexture(extend, "extend_debug");
        command.Clear();
    }

}
