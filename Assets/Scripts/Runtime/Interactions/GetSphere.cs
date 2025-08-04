using System.Collections;
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
			Debug.Log("ENABLED!");
            toggleAction.action.performed += OnToggle;
        }
    }

    private void OnDisable()
    {
        if (toggleAction != null)
        {
			Debug.Log("ENABLED!");
            toggleAction.action.performed -= OnToggle;
        }
    }

    private void OnToggle(InputAction.CallbackContext ctx)
    {
		Debug.Log("TOGGLED!");
        PositionVisualsInFrontOfCamera();
    }

	private void PositionVisualsInFrontOfCamera()
    {
		Debug.Log("POSITIONED!");
        Transform cam = Camera.main.transform;
        Vector3 targetPosition = cam.position + cam.forward * distanceFromCamera;
        Quaternion targetRotation = Quaternion.LookRotation(cam.forward, cam.up);

        Rigidbody rb = SpawnVisuals.Instance.visuals.GetComponent<Rigidbody>();
		Utilities.Helpers.LogObjectDetails(SpawnVisuals.Instance.visuals, "VISUALS");
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.position = targetPosition;
            rb.rotation = targetRotation;
            rb.isKinematic = false;
        }
    }
}
