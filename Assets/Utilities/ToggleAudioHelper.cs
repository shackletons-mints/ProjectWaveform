using UnityEngine;

namespace Utilities
{
    public class ToggleAudioHelper : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip audioClip;
        public int sampleRate = 44100;
        public bool isUsingMicrophone = true;

        public void ToggleAudio()
        {
            audioSource.Stop();

            if (isUsingMicrophone)
            {
                if (audioClip != null)
                {
                    Debug.Log("Switching to audio clip.");
                    if (Microphone.IsRecording(null))
                    {
                        Microphone.End(null);
                    }

                    audioSource.clip = audioClip;
                    audioSource.loop = true;
                    audioSource.Play();
                    isUsingMicrophone = false;
                }
                else
                {
                    Debug.LogWarning("No audio clip assigned.");
                }
            }
            else
            {
                if (Microphone.devices.Length > 0)
                {
                    Debug.Log("Switching to microphone: " + Microphone.devices[0]);
                    audioSource.clip = Microphone.Start(Microphone.devices[0], true, 10, sampleRate);
                    audioSource.loop = true;

                    while (!(Microphone.GetPosition(null) > 0)) { }

                    audioSource.Play();
                    isUsingMicrophone = true;
                }
                else
                {
                    Debug.LogWarning("No microphone devices found.");
                }
            }
        }
    }
}
