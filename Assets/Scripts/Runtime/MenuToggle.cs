using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuToggle : MonoBehaviour
{
    [SerializeField] private InputActionReference _toggleAction;
    [SerializeField] private GameObject Canvas;
    private float _distanceFromCamera = 6;

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
        bool newState = !Canvas.activeSelf;
        if (newState)
        {
            PositionCanvasInFrontOfCamera();
        }
        Canvas.SetActive(newState);
    }

    private void PositionCanvasInFrontOfCamera()
    {
        Transform cam = Camera.main.transform;
        Canvas.transform.position = cam.position + cam.forward * _distanceFromCamera;
        Canvas.transform.rotation = Quaternion.LookRotation(cam.forward, cam.up);
    }
}
