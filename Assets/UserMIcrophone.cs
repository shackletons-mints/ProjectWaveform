using UnityEngine;

public class UserMicrophone : MonoBehaviour
{
    public AudioSource audioSource;

    [Tooltip("Number of spectrum samples. Must be a power of 2 (e.g., 64, 128, 256, 512, 1024, 2048).")]
    public int spectrumSize = 512;

    [Tooltip("FFT window type used for spectrum analysis.")]
    public FFTWindow fftWindow = FFTWindow.Blackman;

    [HideInInspector]
    public float[] spectrum;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        Debug.Log("NAME: " + Microphone.devices[0]);
        audioSource.clip = Microphone.Start(Microphone.devices[0], true, 10, 44100);
        audioSource.loop = true;

        // Wait until the microphone starts recording
        while (!(Microphone.GetPosition(null) > 0)) { }

        audioSource.Play();

        spectrum = new float[spectrumSize];
    }

    void Update()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.GetSpectrumData(spectrum, 0, fftWindow);
            SparseVector sparse = new SparseVector(spectrum);

            if (sparse.vector.Count > 0)
            {
                Debug.Log("Non-zero Spectrum Values: " + sparse.ToString());
            }
        }
    }
}
