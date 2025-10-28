using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

public class GetSphere : MonoBehaviour
{
    [Header("References")]
    public float distanceFromCamera = 2;
    public float maxDistance = 10f;
    public Camera xrCamera;

    void Update()
    {
        Rigidbody rb = SpawnVisualizer.Instance.visualizer.GetComponentInChildren<Rigidbody>();
        if (rb != null)
        {
            float distance = Vector3.Distance(rb.position, xrCamera.transform.position);
            if (distance > maxDistance)
                PositionVisualsInFrontOfCamera();
        }
    }

    private void PositionVisualsInFrontOfCamera()
    {
        Transform cam = Camera.main.transform;
        Vector3 targetPosition = cam.position + cam.forward * distanceFromCamera;
        Quaternion targetRotation = Quaternion.LookRotation(cam.forward, cam.up);

        Rigidbody rb = SpawnVisualizer.Instance.visualizer.GetComponentInChildren<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.position = targetPosition;
            rb.rotation = targetRotation;
            rb.isKinematic = false;
        }
    }
}
