using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SphereSurfacePoints : MonoBehaviour
{
    [Tooltip("Positions and normals of sphere")]
    public List<SurfacePoint> surfacePoints = new List<SurfacePoint>();

    private float radius;
    private Vector3 center;

    private void Awake()
    {
        SetPosition();
    }

    private void Update()
    {
        SetPosition();
    }

    public void SetPosition()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError("No Renderer found on GameObject.");
            return;
        }
        Bounds bounds = rend.bounds;

        center = bounds.center;

        radius = (bounds.extents.x + bounds.extents.y + bounds.extents.z) / 3f;

        GenerateSurfacePoints();
    }

    [ContextMenu("Generate Surface Points")]
    public void GenerateSurfacePoints()
    {
        Debug.Log("Generating surface points...");

        surfacePoints.Clear();

        int pointCount = 12;

        // Latitude three quarters way between top (90°) and equator (0°) would be 22.5f
        // Latitude halfway between top (90°) and equator (0°) would be 45f
        // Latitude three quarters way between top (90°) and equator (0°) would be 67.5f
        float latitudeAngle = Mathf.Deg2Rad * 22.5f;
        float y = Mathf.Cos(latitudeAngle);
        float ringRadius = Mathf.Sin(latitudeAngle);

        for (int i = 0; i < pointCount; i++)
        {
            float angle = i * Mathf.PI * 2f / pointCount;
            float x = Mathf.Cos(angle) * ringRadius;
            float z = Mathf.Sin(angle) * ringRadius;

            Vector3 normal = new Vector3(x, y, z).normalized;
            Vector3 position = center + normal * radius;

            surfacePoints.Add(new SurfacePoint(position, normal));

            Debug.DrawRay(position, normal * 5f, Color.blue, 10f);
        }

        Debug.Log($"Generated {surfacePoints.Count} surface points.");
    }
}
