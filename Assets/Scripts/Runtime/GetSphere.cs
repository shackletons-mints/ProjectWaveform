using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GetSphere: MonoBehaviour
{
    [SerializeField] private InputActionReference _toggleAction;
    [SerializeField] private GameObject visuals;
    private float _distanceFromCamera = 2;

	public void OnStart()
	{
		PositionVisualsInFrontOfCamera();
	}

    private void OnEnable()
    {
        if (_toggleAction != null)
        {
            _toggleAction.action.performed += OnToggle;
        }
    }

    private void OnDisable()
    {
        if (_toggleAction != null)
        {
            _toggleAction.action.performed -= OnToggle;
        }
    }

    private void OnToggle(InputAction.CallbackContext ctx)
    {
		PositionVisualsInFrontOfCamera();
    }

    private void PositionVisualsInFrontOfCamera()
    {
		Transform cam = Camera.main.transform;
		Vector3 targetPosition = cam.position + cam.forward * _distanceFromCamera;
		Quaternion targetRotation = Quaternion.LookRotation(cam.forward, cam.up);

		Rigidbody rb = visuals.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.isKinematic = true;
			rb.position = targetPosition;
			rb.rotation = targetRotation;
			rb.isKinematic = false;
		}
    }
}
