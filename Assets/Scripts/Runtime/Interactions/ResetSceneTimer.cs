using AudioVisualization;
using UnityEngine;

public class ResetSceneTimer : MonoBehaviour
{

    public void ToZero()
    {
        AudioVisualizer _audioVisualizer = SpawnVisualizer.Instance.audioVisualizer;
        _audioVisualizer.sceneTimer = 0f;
    }
}
