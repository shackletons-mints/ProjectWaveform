using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

public class GetSphere : MonoBehaviour
{
	[Header("References")]
    public InputActionReference toggleAction;
    public float distanceFromCamera = 2;

    private void OnEnable()
    {
        if (toggleAction != null)
        {
            toggleAction.action.performed += OnToggle;
        }
    }

    private void OnDisable()
    {
        if (toggleAction != null)
        {
            toggleAction.action.performed -= OnToggle;
        }
    }

    private void OnToggle(InputAction.CallbackContext ctx)
    {
        PositionVisualsInFrontOfCamera();
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
