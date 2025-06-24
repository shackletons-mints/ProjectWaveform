using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPlaneColliderEnabler : MonoBehaviour
{
	[SerializeField] private Material planeMaterial;
    public ARPlaneManager planeManager;

    void OnEnable()
    {
        planeManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)
        {
            AddOrUpdateCollider(plane);
			AddPaintable(plane);
			SetupMeshRenderer(plane);
			SetupRigidbody(plane);
        }

        foreach (var plane in args.updated)
        {
            AddOrUpdateCollider(plane);
        }
    }

    void AddOrUpdateCollider(ARPlane plane)
    {
        if (!plane.TryGetComponent<MeshCollider>(out var collider))
		{
            collider = plane.gameObject.AddComponent<MeshCollider>();
		}

        collider.sharedMesh = plane.GetComponent<MeshFilter>().mesh;
    }

	void AddPaintable(ARPlane plane)
	{
        if (!plane.TryGetComponent<Paintable>(out var script))
        {
            plane.gameObject.AddComponent<Paintable>();
        }
	}

	void SetupMeshRenderer(ARPlane plane)
	{
		if (!plane.TryGetComponent<MeshRenderer>(out var meshRenderer))
		{
			meshRenderer = plane.gameObject.AddComponent<MeshRenderer>();
		}

		meshRenderer.material = planeMaterial;

		meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		meshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
		meshRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.BlendProbes;
		meshRenderer.probeAnchor = null;
		meshRenderer.allowOcclusionWhenDynamic = true;
		// meshRenderer.renderingLayerMask = 1 << 1;
	}	

	void SetupRigidbody(ARPlane plane)
	{
		if (!plane.TryGetComponent<Rigidbody>(out var rb))
		{
			rb = plane.gameObject.AddComponent<Rigidbody>();
		}

		rb.mass = 1f;
		rb.linearDamping = 0f;
		rb.angularDamping = 0.05f;
		rb.useGravity = false;
		rb.isKinematic = true;
		rb.interpolation = RigidbodyInterpolation.None;
		rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
	}
}

