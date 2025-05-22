using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using UnityEngine.InputSystem;

public class AudioVisualization : MonoBehaviour
{
    public GameObject sphere;
    public ParticleSystem particleSystem;
    public SphereSurfacePoints sphereSurfacePoints;

    public AudioSource audioSource;

    [Tooltip("Number of spectrum samples. Must be a power of 2 (e.g., 64, 128, 256, 512, 1024, 2048).")]
    public int spectrumSize = 1024;
    public int sampleRate = 44100;

    [Tooltip("FFT window type used for spectrum analysis.")]
    public FFTWindow fftWindow = FFTWindow.BlackmanHarris;
    public float[] spectrumData;
    public AudioPitchEstimator audioPitchEstimator;
    float emitTimer = 0f;
    float emitInterval = 0.05f;

    [Tooltip("Toggle between using microphone or audio clip.")]
    public bool useMicrophone = true;

    public AudioClip audioClip;
    public ToggleAudioHelper toggleAudioHelper;

void Start()
{
    if (particleSystem == null)
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }
    if (particleSystem != null)
    {
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        var emission = particleSystem.emission;
        emission.enabled = false;
    }

    sphere = GameObject.Find("Sphere");
    if (sphere != null)
    {
        sphereSurfacePoints = sphere.GetComponent<SphereSurfacePoints>();

        if (sphereSurfacePoints != null)
        {
            sphereSurfacePoints.GenerateSurfacePoints();
        }
        else
        {
            Debug.LogError("SphereSurfacePoints component not found on the sphere GameObject.");
        }
    }
    else
    {
        Debug.LogWarning("Sphere GameObject not found.");
    }

    if (audioSource == null)
    {
        audioSource = GetComponent<AudioSource>();
    }

    if (toggleAudioHelper == null)
    {
        toggleAudioHelper = GetComponent<ToggleAudioHelper>();
    }

    if (audioPitchEstimator == null)
    {
    audioPitchEstimator = GetComponent<AudioPitchEstimator>();
    }

    if (useMicrophone)
    {
        if (Microphone.devices.Length > 0)
        {
            Debug.Log("Using microphone: " + Microphone.devices[0]);
            audioSource.clip = Microphone.Start(Microphone.devices[0], true, 10, sampleRate);
            audioSource.loop = true;

            while (!(Microphone.GetPosition(null) > 0)) { }

            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No microphone devices found.");
        }
    }
    else
    {
        if (audioClip != null)
        {
            Debug.Log("Using audio clip.");
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No audio clip assigned.");
        }
    }

    spectrumData = new float[spectrumSize];
}



    void Update()
    {
        emitTimer += Time.deltaTime;
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.GetSpectrumData(spectrumData, 0, fftWindow);
            float estimatedPitch = audioPitchEstimator.Estimate(audioSource);
            int roundedPitch = (int)Math.Round(estimatedPitch, 0);
            SpectrumAnalysis spectrumAnalysis = new SpectrumAnalysis(spectrumData);

            bool validPitch = !float.IsNaN(estimatedPitch);
            bool readyToEmit = emitTimer >= emitInterval;
            if (validPitch && readyToEmit)
            {
                emitTimer = 0f;
                if (particleSystem != null)
                {
                    var psTransform = particleSystem.transform;
                    var psMain = particleSystem.main;

                    // A dictionary mapping note bands to index and color
                    var noteBands = new (float min, float max, int index, Color color, string note)[]
                    {
                        (435f, 445f, 10, new Color(0.5f, 0.25f, 0f, 1f), "A4"),
                        (461f, 471f, 11, new Color(1f, 0f, 1f, 1f), "A#4"),
                        (489f, 499f, 12, new Color(0f, 1f, 1f, 1f), "B4"),
                        (256f, 266f, 13, new Color(0.5f, 0.5f, 0f, 1f), "C4"),
                        (272f, 282f, 14, new Color(1f, 0f, 0f, 1f), "C#4"),
                        (288f, 298f, 15, new Color(1f, 0.5f, 0f, 1f), "D4"),
                        (306f, 316f, 16, new Color(1f, 1f, 0f, 1f), "D#4"),
                        (324f, 334f, 17, new Color(0f, 1f, 0f, 1f), "E4"),
                        (344f, 354f, 18, new Color(0f, 0f, 1f, 1f), "F4"),
                        (365f, 375f, 19, new Color(0.29f, 0f, 0.51f, 1f), "F#4"),
                        (387f, 397f, 20, new Color(0.56f, 0f, 1f, 1f), "G4"),
                        (410f, 420f, 21, new Color(1f, 1f, 1f, 1f), "G#4")
                    };

                    foreach (var (min, max, index, color, pitch) in noteBands)
                    {
                        if (roundedPitch >= min && roundedPitch < max)
                        {
                            psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                            Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                            psTransform.rotation = Quaternion.LookRotation(direction);
                            psMain.startColor = color;
                            particleSystem.Emit(1);
                            Debug.Log("Pitch " + pitch);
                            break; // Exit after first match
                        }
                    }
                }
            }
            else if (!validPitch)
            {
                Debug.Log("No clear pitch detected");
            }
        }
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (toggleAudioHelper != null)
            {
                toggleAudioHelper.ToggleAudio();
            }
            else
            {
                Debug.LogWarning("toggleAudioHelper is not assigned or missing on this GameObject.");
            }
        }
    }
}
