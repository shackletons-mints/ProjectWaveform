using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utilities;

public class Transistion : MonoBehaviour
{
    
    [SerializeField] public InputActionReference toggleAction;

    private void OnEnable()
    {
        if (toggleAction != null)
            toggleAction.action.performed += OnToggle;
    }

    private void OnDisable()
    {
        if (toggleAction != null)
            toggleAction.action.performed -= OnToggle;
    }

    private void OnToggle(InputAction.CallbackContext ctx)
    {
        SceneManager.LoadScene(1); 
    }
}
