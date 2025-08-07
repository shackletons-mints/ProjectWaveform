using UnityEngine;

[System.Serializable]
public class SurfacePoint
{
    [Tooltip("Position on the sphere")]
    public Vector3 position;

    [Tooltip("Direction perpendicular to the surface at this point")]
    public Vector3 normal;

    public SurfacePoint(Vector3 position, Vector3 normal)
    {
        this.position = position;
        this.normal = normal.normalized;
    }
}
