using UnityEngine;
using UnityEngine.InputSystem;

public class MenuToggle : MonoBehaviour
{
    [SerializeField] private InputActionReference _toggleAction;
    [SerializeField] private GameObject Canvas;
    [SerializeField] private float distanceFromCamera = 3f;

    private void OnEnable()
    {
        if (_toggleAction != null)
	    {
            _toggleAction.action.performed += OnToggle;
	    }

        PositionCanvasInFrontOfCamera();
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
        bool newState = !Canvas.activeSelf;
        Canvas.SetActive(newState);

        if (newState)
	    {
            PositionCanvasInFrontOfCamera();
	    }
    }

    private void PositionCanvasInFrontOfCamera()
    {
        Transform cam = Camera.main.transform;
        Canvas.transform.position = cam.position + cam.forward * distanceFromCamera;
        Canvas.transform.rotation = Quaternion.LookRotation(cam.forward, cam.up);
    }
}
