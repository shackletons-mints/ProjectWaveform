using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SphereSurfacePoints
{
    public GameObject sphere;
    public float radius;
    public Vector3 center;

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

        center = sphere.transform.position;
        radius = sphere.GetComponent<SphereCollider>().radius;

        Debug.Log("Calculated radius: " + radius);

        GenerateSurfacePoints();
    }
    public void GenerateSurfacePoints()
    {
        if (sphere == null)
        {
            Debug.LogError("Sphere not assigned.");
            return;
        }

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
    }

    public void LogAllSurfacePoints()

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
