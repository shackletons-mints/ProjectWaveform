using UnityEngine;
using UnityEngine.XR.ARFoundation.Samples;
using Utilities;

public class ToggleMicrophoneController : MonoBehaviour
{
    [SerializeField]
    SliderToggle m_AudioSliderToggle;

    [SerializeField]
    AudioToggle m_AudioToggle;

    void Start()
    {
        m_AudioSliderToggle.SetSliderValue(m_AudioToggle.isUsingMicrophone);
    }

    public void ToggleAudioInput()
    {
        m_AudioToggle.ToggleIsUsingMicrophone();
        m_AudioSliderToggle.SetSliderValue(m_AudioToggle.isUsingMicrophone);
    }
}
