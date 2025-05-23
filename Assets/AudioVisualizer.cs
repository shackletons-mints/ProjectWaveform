using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using UnityEngine.InputSystem;

public class AudioVisualer : MonoBehaviour
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
    float emitInterval = 0.125f; // ~eight of a second
    string[] pitchNames = {
        "C", "C#", "D", "D#", "E", "F",
        "F#", "G", "G#", "A", "A#", "B"
    };

    Color[] pitchColors = {
        new Color(0.5f, 0.5f, 0f, 1f),   // C - Olive
        new Color(1f, 0f, 0f, 1f),       // C# - Red
        new Color(1f, 0.5f, 0f, 1f),     // D - Orange
        new Color(1f, 1f, 0f, 1f),       // D# - Yellow
        new Color(0f, 1f, 0f, 1f),       // E - Green
        new Color(0f, 0f, 1f, 1f),       // F - Blue
        new Color(0.29f, 0f, 0.51f, 1f), // F# - Indigo
        new Color(0.56f, 0f, 1f, 1f),    // G - Violet
        new Color(1f, 1f, 1f, 1f),       // G# - White
        new Color(0.5f, 0.25f, 0f, 1f),  // A - Brown
        new Color(1f, 0f, 1f, 1f),       // A# - Magenta
        new Color(0f, 1f, 1f, 1f)        // B - Cyan
    };

    [Tooltip("Toggle between using microphone or audio clip.")]
    public bool useMicrophone = true;

    public AudioClip audioClip;
    public ToggleAudioHelper toggleAudioHelper;

    void Start()
    {
        if (particleSystem == null)
        {
            // todo fix this
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
            // todo fix this
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

                    int midiNote = Mathf.FloorToInt(69 + 12 * Mathf.Log(estimatedPitch / 440f, 2));
                    int pitchClass = midiNote % 12; // 0 = C, 1 = C#, ..., 9 = A, ...

                    string pitchName = pitchNames[pitchClass];
                    Color pitchColor = pitchColors[pitchClass];
                    int index = pitchClass + 10;

                    psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                    Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                    psTransform.rotation = Quaternion.LookRotation(direction);
                    psMain.startColor = pitchColor;
                    particleSystem.Emit(10);

                    Debug.Log($"Emitting: {pitchName} (Freq: {estimatedPitch} Hz, MIDI: {midiNote})");
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
