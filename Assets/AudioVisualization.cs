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
    float emitInterval = 0.01f;

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
                    var psShape = particleSystem.shape;
                    var psMain = particleSystem.main;
                    int normalizedPitch = roundedPitch % 1200;

                    if (normalizedPitch >= 300 && normalizedPitch < 330)
                    {
                        int index = 10;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(1f, 0f, 0f, 1f); // Red (C#)
                        particleSystem.Emit(1);
                    }
                    else if (normalizedPitch >= 330 && normalizedPitch < 350)
                    {
                        int index = 11;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(1f, 0.5f, 0f, 1f); // Orange (D)
                        particleSystem.Emit(1);
                    }
                    else if (normalizedPitch >= 350 && normalizedPitch < 380)
                    {
                        int index = 12;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(1f, 1f, 0f, 1f); // Yellow (D#)
                        particleSystem.Emit(1);
                    }
                    else if (normalizedPitch >= 380 && normalizedPitch < 410)
                    {
                        int index = 13;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0f, 1f, 0f, 1f); // Green (E)
                        particleSystem.Emit(1);
                    }
                    else if (normalizedPitch >= 410 && normalizedPitch < 440)
                    {
                        int index = 14;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0f, 0f, 1f, 1f); // Blue (F)
                        particleSystem.Emit(1);
                    }
                    else if (normalizedPitch >= 440 && normalizedPitch < 470)
                    {
                        int index = 15;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0.29f, 0f, 0.51f, 1f); // Indigo (F#)
                        particleSystem.Emit(1);
                    }
                    else if (normalizedPitch >= 470 && normalizedPitch < 500)
                    {
                        int index = 16;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0.56f, 0f, 1f, 1f); // Violet (G)
                        particleSystem.Emit(1);
                    }
                    else if (normalizedPitch >= 500 && normalizedPitch < 530)
                    {
                        int index = 17;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(1f, 1f, 1f, 1f); // White (G#)
                        particleSystem.Emit(1);
                    }
                    else if (normalizedPitch >= 530 && normalizedPitch < 560)
                    {
                        int index = 18;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0.5f, 0.25f, 0f, 1f); // Brown (A)
                        particleSystem.Emit(1);
                    }
                    else if (normalizedPitch >= 560 && normalizedPitch < 590)
                    {
                        int index = 19;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(1f, 0f, 1f, 1f); // Magenta (A#)
                        particleSystem.Emit(1);
                    }
                    else if (normalizedPitch >= 590 && normalizedPitch < 620)
                    {
                        int index = 20;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0f, 1f, 1f, 1f); // Cyan (B)
                        particleSystem.Emit(1);
                    }
                    else if (normalizedPitch >= 620 && normalizedPitch < 650)
                    {
                        int index = 21;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0.5f, 0.5f, 0f, 1f); // Olive (C)
                        particleSystem.Emit(1);
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
