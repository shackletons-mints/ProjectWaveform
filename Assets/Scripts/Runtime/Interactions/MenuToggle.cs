using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuToggle : MonoBehaviour
{
    [Header("References")]
    public InputActionReference toggleAction;
    public GameObject canvas;

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
        bool newState = !canvas.activeSelf;
        canvas.SetActive(newState);
    }
}
