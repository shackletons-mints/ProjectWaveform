using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

public class SceneToggleAudio : MonoBehaviour
{
    [SerializeField]
    private InputActionReference _toggleAction;

    [SerializeField]
    private AudioToggle _audioToggle;

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
        _audioToggle.ToggleIsUsingMicrophone();
        _audioToggle.ToggleAudio();
    }
}
