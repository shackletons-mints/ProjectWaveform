using System;
using UnityEngine;
using Utilities;
using UnityEngine.InputSystem;

public class AudioVisualizer : MonoBehaviour
{
    public GameObject sphere;
    public ParticleSystem particleSystem;
    public SphereSurfacePoints sphereSurfacePoints;

    public AudioSource audioSource;
    public AudioClip audioClip;
    public ToggleAudioHelper toggleAudioHelper;
    public AudioPitchEstimator audioPitchEstimator;

    public int spectrumSize = 1024;
    public int sampleRate = 44100;
    public FFTWindow fftWindow = FFTWindow.BlackmanHarris;

    public bool useMicrophone = true;

    private float[] spectrumData;
    private float emitTimer = 0f;
    private float emitInterval = 0.125f;

    private static readonly string[] pitchNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
    private static readonly Color[] pitchColors = {
        new Color(0.5f, 0.5f, 0f), new Color(1f, 0f, 0f), new Color(1f, 0.5f, 0f),
        new Color(1f, 1f, 0f), new Color(0f, 1f, 0f), new Color(0f, 0f, 1f),
        new Color(0.29f, 0f, 0.51f), new Color(0.56f, 0f, 1f), new Color(1f, 1f, 1f),
        new Color(0.5f, 0.25f, 0f), new Color(1f, 0f, 1f), new Color(0f, 1f, 1f)
    };

    void Start()
    {
        InitializeReferences();
        InitializeSphere();
        InitializeAudio();
        spectrumData = new float[spectrumSize];
    }

    void Update()
    {
        emitTimer += Time.deltaTime;

        if (audioSource != null && audioSource.isPlaying)
        {
            AnalyzeAudio();
        }

        HandleInput();
    }

    // ---- Initialization Methods ----

    void InitializeReferences()
    {
        particleSystem ??= GetComponent<ParticleSystem>();
        particleSystem?.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        audioSource ??= GetComponent<AudioSource>();
        toggleAudioHelper ??= GetComponent<ToggleAudioHelper>();
        audioPitchEstimator ??= GetComponent<AudioPitchEstimator>();
    }

    void InitializeSphere()
    {
        sphere = GameObject.Find("Sphere");
        sphereSurfacePoints = sphere?.GetComponent<SphereSurfacePoints>();
        sphereSurfacePoints?.GenerateSurfacePoints();
    }

    void InitializeAudio()
    {
        if (useMicrophone && Microphone.devices.Length > 0)
        {
            string mic = Microphone.devices[0];
            Debug.Log("Using microphone: " + mic);
            audioSource.clip = Microphone.Start(mic, true, 10, sampleRate);
            audioSource.loop = true;
            while (Microphone.GetPosition(null) <= 0) { }
            audioSource.Play();
        }
        else if (!useMicrophone && audioClip != null)
        {
            Debug.Log("Using audio clip.");
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No valid audio source found.");
        }
    }

    // ---- Audio Analysis ----

    void AnalyzeAudio()
    {
        audioSource.GetSpectrumData(spectrumData, 0, fftWindow);
        float pitch = audioPitchEstimator.Estimate(audioSource);

        if (float.IsNaN(pitch) || emitTimer < emitInterval)
        {
            if (float.IsNaN(pitch))
                Debug.Log("No clear pitch detected");
            return;
        }

        emitTimer = 0f;
        EmitParticles(pitch);
    }

    void EmitParticles(float pitch)
    {
        int midiNote = Mathf.FloorToInt(69 + 12 * Mathf.Log(pitch / 440f, 2));
        int pitchClass = midiNote % 12;

        string pitchName = pitchNames[pitchClass];
        Color pitchColor = pitchColors[pitchClass];
        int pointIndex = pitchClass + 10;

        if (sphereSurfacePoints != null && pointIndex < sphereSurfacePoints.surfacePoints.Count)
        {
            var psTransform = particleSystem.transform;
            var psMain = particleSystem.main;

            psTransform.position = sphereSurfacePoints.surfacePoints[pointIndex].position;
            psTransform.rotation = Quaternion.LookRotation(sphereSurfacePoints.surfacePoints[pointIndex].normal);
            psMain.startColor = pitchColor;
            particleSystem.Emit(10);

            Debug.Log($"Emitting: {pitchName} (Freq: {pitch} Hz, MIDI: {midiNote})");
        }
    }

    // ---- Input Handling ----

    void HandleInput()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (toggleAudioHelper != null)
                toggleAudioHelper.ToggleAudio();
            else
                Debug.LogWarning("toggleAudioHelper not assigned.");
        }
    }
}
