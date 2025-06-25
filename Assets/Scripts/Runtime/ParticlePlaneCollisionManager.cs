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
}

