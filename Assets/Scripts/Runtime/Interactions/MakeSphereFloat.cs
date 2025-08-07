using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

public class MakeSphereFloat : MonoBehaviour
{
    [Header("References")]
    public InputActionReference toggleAction;

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
        ToggleIsKinematic();
    }

    private void ToggleIsKinematic()
    {
        Rigidbody rb = SpawnVisualizer.Instance.visualizer.GetComponentInChildren<Rigidbody>();
        if (rb != null)
            rb.isKinematic = !rb.isKinematic;
    }
}
