using UnityEngine;

public class UserMicrophone : MonoBehaviour
{
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
                Debug.Log("Non-zero Spectrum Values: " + soundSpectrum.ToString());
                Debug.Log("Largest Value: " + soundSpectrum.GetLargestValue());
                Debug.Log("Smallest Value: " + soundSpectrum.GetSmallestValue());
                Debug.Log("Estimated Pitch: " + soundSpectrum.GetEstimatedPitch());
            }
        }
    }
}
