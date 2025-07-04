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
            ModifyCeiling(plane);
        }

        foreach (var plane in args.updated)
        {
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

    public static void LogObjectDetails(object obj, string label = "")
    {
        if (obj == null)
        {
            Debug.Log($"{label} [null object]");
            return;
        }

        Type type = obj.GetType();
        string log = $"--- {label} ({type.Name}) ---\n";

        // Log all public properties
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            try
            {
                object value = prop.GetValue(obj, null);
                log += $"{prop.Name}: {value}\n";
            }
            catch { /* ignore inaccessible properties */ }
        }

        // Log all public fields
        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            try
            {
                object value = field.GetValue(obj);
                log += $"{field.Name}: {value}\n";
            }
            catch { /* ignore */ }
        }

        log += "-----------------------------";
        Debug.Log(log);
    }
}
