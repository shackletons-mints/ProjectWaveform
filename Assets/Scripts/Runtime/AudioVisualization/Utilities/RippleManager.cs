using UnityEngine;
using AudioVisualization;

public class RippleManager : MonoBehaviour
{
    const int MaxRipples = 8;

    private Vector4[] rippleData = new Vector4[MaxRipples];
    private int currentIndex = 0;

    public float rippleDuration = 2f;

    void Awake()
    {
        for (int i = 0; i < MaxRipples; i++) 
		{
			rippleData[i] = Vector4.zero;
		}
    }

    public void EmitRipple(SphereSurfacePoints surfacePoints, int pointIndex)
    {
        if (surfacePoints == null || pointIndex >= surfacePoints.surfacePoints.Count)
		{
            return;
		}

        Vector3 position = surfacePoints.surfacePoints[pointIndex].position;
        float time = Time.time;

        rippleData[currentIndex] = new Vector4(position.x, position.y, position.z, time);
        currentIndex = (currentIndex + 1) % MaxRipples;

		Debug.Log("rippleData first: " + rippleData[0]);
		Debug.Log("rippleData last: " + rippleData[7]);
        Shader.SetGlobalVectorArray("_RipplePoints", rippleData);
        Shader.SetGlobalFloat("_RippleTime", time);
		Shader.SetGlobalFloat("_RippleDuration", rippleDuration);
    }    

}

