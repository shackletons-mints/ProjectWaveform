using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;
using System.Reflection;

public class RemoveCeilingPlanes : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public Material stencil;

    void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
    }

	void Update()
	{
		foreach (var plane in planeManager.trackables)
		{
			Debug.Log("ModifyCeiling");
			ModifyCeiling(plane);
		}
	}

    void LogPlane(ARPlane plane)
    {
        if (plane == null) return;
        // LogObjectDetails(plane, "ARPlane");
    }

    void ModifyCeiling(ARPlane plane)
    {
        if (plane.classifications == PlaneClassifications.Ceiling)
        {
            var collider = plane.GetComponent<Collider>();
            if (collider)
            {
                collider.enabled = false;
            }

            var renderer = plane.GetComponent<MeshRenderer>();
            if (renderer)
            {
                renderer.material = stencil;
            }
        }
    }

}
