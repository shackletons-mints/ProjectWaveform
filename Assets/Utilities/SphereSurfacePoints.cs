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
            // n = 50
            // goldenRatio = (1 + 5**0.5)/2
            // i = arange(0, n)
            // theta = 2 *pi * i / goldenRatio
            // phi = arccos(1 - 2*(i+0.5)/n)
            // x, y, z = cos(theta) * sin(phi), sin(theta) * sin(phi), cos(phi);
            float goldenRation = (1 + Mathf.Sqrt(5)) / 2;
            float theta = 2 * Mathf.PI * i / goldenRation;
            float phi = Mathf.Acos(1 - 2*(i+0.5f)/pointCount);

            float x = radius * Mathf.Cos(theta) * Mathf.Sin(phi);
            float y = radius * Mathf.Sin(theta) * Mathf.Sin(phi);
            float z = radius * Mathf.Cos(phi);

            Vector3 normal = new Vector3(x, y, z);
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
