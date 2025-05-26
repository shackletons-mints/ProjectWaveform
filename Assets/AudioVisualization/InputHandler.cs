using UnityEngine;
using UnityEngine.InputSystem;

namespace AudioVisualization
{
    public static class InputHandler
    {
        public static void HandleInput(AudioVisualizer visualizer)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (visualizer.audioToggle != null)
                    visualizer.audioToggle.ToggleAudio();
                else
                    Debug.LogWarning("audioToggle not assigned.");
            }
        }
    }
}
