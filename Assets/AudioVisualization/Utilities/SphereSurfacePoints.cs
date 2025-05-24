using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SphereSurfacePoints : MonoBehaviour
{
    [Tooltip("Total points on the sphere")]
    [Range(100, 2000)]
    [SerializeField] private int pointCount = 100;

    [Tooltip("Positions and normals of sphere")]
    public List<SurfacePoint> surfacePoints = new List<SurfacePoint>();

    private float radius;
    private Vector3 center;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        SphereCollider collider = GetComponent<SphereCollider>();
        if (collider == null)
        {
            Debug.LogError("No SphereCollider found on GameObject.");
            return;
        }

        center = transform.position;
        radius = collider.radius * collider.transform.lossyScale.x;

        GenerateSurfacePoints();
    }

    [ContextMenu("Generate Surface Points")]
    public void GenerateSurfacePoints()
    {
        Debug.Log("Generating surface points...");

        surfacePoints.Clear();

        float goldenAngle = Mathf.PI * (3f - Mathf.Sqrt(5f));

        for (int i = 0; i < pointCount; i++)
        {
            float y = 1f - (i + 0.5f) * (2f / pointCount);
            float radiusAtY = Mathf.Sqrt(1f - y * y);
            float theta = goldenAngle * i;
            float x = Mathf.Cos(theta) * radiusAtY;
            float z = Mathf.Sin(theta) * radiusAtY;

            Vector3 normal = new Vector3(x, y, z).normalized;
            Vector3 position = center + normal * radius;

            surfacePoints.Add(new SurfacePoint(position, normal));

            // Optional: visualize with Debug.DrawRay
            if (i >= 10 && i <= 21)
            {
                Debug.DrawRay(position, normal * 5f, Color.red, 10f);
            }
        }

        Debug.Log($"Generated {surfacePoints.Count} surface points.");
    }

}
