using System.Collections.Generic;
using UnityEngine;

public class SphereSurfacePoints
{
    public GameObject sphere;
    [Tooltip("Total points on the sphere")]
    [Range(100, 2000)]
    public int pointCount = 500;

    [Tooltip("Positions and normals of sphere")]
    public List<SurfacePoint> surfacePoints = new List<SurfacePoint>();

    public SphereSurfacePoints(GameObject sphere)
    {
        this.sphere = sphere;
        if (sphere == null)
        {
            Debug.LogError("Sphere not assigned.");
            return;
        }

        // Generate points on the surface of the sphere
        GenerateSurfacePoints(sphere.transform.position, sphere.transform.lossyScale.x * 0.5f);
    }

    // Function to generate surface points on the sphere
    public GenerateSurfacePoints(Vector3 center, float radius)
    {
        float offset = 2f / pointCount;
        float increment = Mathf.PI * (3f - Mathf.Sqrt(5f));

        for (int i = 0; i < pointCount; i++)
        {
            float y = ((i * offset) - 1) + (offset / 2);
            float r = Mathf.Sqrt(1 - y * y);
            float phi = i * increment;

            float x = Mathf.Cos(phi) * r;
            float z = Mathf.Sin(phi) * r;

            Vector3 normal = new Vector3(x, y, z).normalized;
            Vector3 position = center + normal * radius;

            surfacePoints.Add(new SurfacePoint(position, normal));
        }

        Debug.Log($"Generated {surfacePoints.Count} surface points.");
    }
    public void LogAllSurfacePoints(List<SurfacePoint> surfacePoints)
    {
        if (surfacePoints == null || surfacePoints.Count == 0)
        {
            Debug.LogWarning("No surface points to log.");
            return;
        }

        for (int i = 0; i < surfacePoints.Count; i++)
        {
            SurfacePoint sp = surfacePoints[i];
            Debug.Log($"Point {i}: Position = {sp.position}, Normal = {sp.normal}");
        }

        Debug.Log($"Total Surface Points Logged: {surfacePoints.Count}");
    }
}
