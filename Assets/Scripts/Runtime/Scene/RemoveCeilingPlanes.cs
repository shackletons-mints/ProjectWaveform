using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RemoveCeilingPlanes : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public Material stencil;

    void Awake()
    {
        planeManager = FindFirstObjectByType<ARPlaneManager>();
    }
	
	public void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARPlane> changes)
	{
		foreach (var plane in changes.added)
		{
			if (plane.classifications == PlaneClassifications.Ceiling)
				ModifyCeiling(plane);
		}

		foreach (var plane in changes.updated)
		{
			if (plane.classifications == PlaneClassifications.Ceiling)
				ModifyCeiling(plane);
		}
	}

    void ModifyCeiling(ARPlane plane)
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
