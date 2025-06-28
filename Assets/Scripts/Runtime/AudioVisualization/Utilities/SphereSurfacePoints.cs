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

    public void SetPosition()
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

        int pointCount = 12;

        // Latitude halfway between top (90°) and equator (0°)
        float latitudeAngle = Mathf.Deg2Rad * 45f;
        float y = Mathf.Cos(latitudeAngle);
        float ringRadius = Mathf.Sin(latitudeAngle);

        for (int i = 0; i < pointCount; i++)
        {
            float angle = i * Mathf.PI * 2f / pointCount;
            float x = Mathf.Cos(angle) * ringRadius;
            float z = Mathf.Sin(angle) * ringRadius;

            Vector3 normal = new Vector3(x, y, z).normalized;
            Vector3 position = center + normal * radius;
			Debug.Log("Position: " + position);

            surfacePoints.Add(new SurfacePoint(position, normal));

            Debug.DrawRay(position, normal * 5f, Color.blue, 10f);
        }

        Debug.Log($"Generated {surfacePoints.Count} surface points.");
    }

}
