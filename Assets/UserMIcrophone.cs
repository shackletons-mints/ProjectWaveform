using UnityEngine;

public class UserMicrophone : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public AudioSource audioSource;

    [Tooltip("Number of spectrum samples. Must be a power of 2 (e.g., 64, 128, 256, 512, 1024, 2048).")]
    public int spectrumSize = 4096;
    public int sampleRate = 44100;

    [Tooltip("FFT window type used for spectrum analysis.")]
    public FFTWindow fftWindow = FFTWindow.Blackman;

    public float[] spectrumData;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        Debug.Log("NAME: " + Microphone.devices[0]);
        audioSource.clip = Microphone.Start(Microphone.devices[0], true, 10, sampleRate);
        audioSource.loop = true;

        // Wait until the microphone starts recording
        while (!(Microphone.GetPosition(null) > 0)) { }

        audioSource.Play();

        spectrumData = new float[spectrumSize];
    }

    void Update()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.GetSpectrumData(spectrumData, 0, fftWindow);
            SoundSpectrum soundSpectrum = new SoundSpectrum(spectrumData, spectrumSize, sampleRate);

            if (soundSpectrum.data.Length > 0)
            {
                // Parse values from string-returning methods
                float largest = float.Parse(soundSpectrum.GetLargestValue());
                float smallest = float.Parse(soundSpectrum.GetSmallestValue());
                float pitch = float.Parse(soundSpectrum.GetEstimatedPitch(4));

                // Particle System references
                var main = particleSystem.main;
                var emission = particleSystem.emission;

                // This is chatgpt slop, BUT - its a start
                // we now have the ability to take in values from the spectrum
                // and use them to modify the particle system
                // the sky is the limit now!
                main.startSize = Mathf.Clamp(largest * 10f, 0.1f, 5f);
                main.startSpeed = Mathf.Clamp(largest * 50f, 1f, 20f);
                emission.rateOverTime = Mathf.Clamp(largest * 100f, 10f, 200f);
                main.startLifetime = Mathf.Clamp(smallest * 10f, 0.5f, 5f);
            }
        }
    }
}
