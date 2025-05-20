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
    float emitInterval = 0.01f; // Emit every 0.1 seconds

    [Tooltip("Toggle between using microphone or audio clip.")]
    public bool useMicrophone = true;

    public AudioClip audioClip;
    public ToggleAudioHelper toggleAudioHelper;

void Start()
{
    particleSystem = GetComponentInChildren<ParticleSystem>();
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
            Debug.Log("valid pitch " + validPitch);
            Debug.Log("ready to emit " + readyToEmit);
            if (validPitch && readyToEmit)
            {
                emitTimer = 0f;
                Debug.Log("PARTICLE SYSTEM " + particleSystem);
                if (particleSystem != null)
                {
                    var psTransform = particleSystem.transform;
                    var psShape = particleSystem.shape;
                    var psMain = particleSystem.main;


                   if (roundedPitch >= 300 && roundedPitch < 330)
                    {
                        int index = 0;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(1f, 0f, 0f, 1f); // Red (C#4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 330 && roundedPitch < 350)
                    {
                        int index = 10;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(1f, 0.5f, 0f, 1f); // Orange (D4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 350 && roundedPitch < 380)
                    {
                        int index = 20;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(1f, 1f, 0f, 1f); // Yellow (D#4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 380 && roundedPitch < 410)
                    {
                        int index = 25;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0f, 1f, 0f, 1f); // Green (E4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 410 && roundedPitch < 440)
                    {
                        int index = 30;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0f, 0f, 1f, 1f); // Blue (F4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 440 && roundedPitch < 470)
                    {
                        int index = 40;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0.29f, 0f, 0.51f, 1f); // Indigo (F#4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 470 && roundedPitch < 500)
                    {
                        int index = 45;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0.56f, 0f, 1f, 1f); // Violet (G4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 500 && roundedPitch < 530)
                    {
                        int index = 50;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(1f, 1f, 1f, 1f); // White (G#4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 530 && roundedPitch < 560)
                    {
                        int index = 55;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0.5f, 0.25f, 0f, 1f); // Brown (A4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 560 && roundedPitch < 590)
                    {
                        int index = 60;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(1f, 0f, 1f, 1f); // Magenta (A#4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 590 && roundedPitch < 620)
                    {
                        int index = 70;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0f, 1f, 1f, 1f); // Cyan (B4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 620 && roundedPitch < 650)
                    {
                        int index = 80;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0.5f, 0.5f, 0f, 1f); // Olive (C5)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 650 && roundedPitch < 680)
                    {
                        int index = 85;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0.8f, 0.2f, 0.8f, 1f); // Orchid (C#5)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 680 && roundedPitch < 710)
                    {
                        int index = 90;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0.3f, 0.7f, 0.3f, 1f); // Mint Green (D5)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 710 && roundedPitch < 740)
                    {
                        int index = 95;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0.6f, 0f, 0f, 1f); // Dark Red (D#5)
                        particleSystem.Emit(1);
                    }

                }
            }
            else if (!validPitch)
            {
                Debug.Log("No clear pitch detected");
            }
            else if (!readyToEmit)
            {
                Debug.Log("Waiting to emit — pitch OK, timer not ready");
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
