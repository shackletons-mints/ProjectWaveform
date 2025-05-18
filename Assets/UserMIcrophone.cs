using System;
using System.Collections.Generic;
using UnityEngine;

public class UserMicrophone : MonoBehaviour
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
    float emitInterval = 0.02f; // Emit every 0.1 seconds

    void Start()
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var emission = particleSystem.emission;
            emission.enabled = false;
        }

        sphere = GameObject.Find("Sphere");
        if (sphere != null)
        {
            sphereSurfacePoints = new SphereSurfacePoints(sphere);
            sphereSurfacePoints.LogAllSurfacePoints();
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        audioPitchEstimator = GetComponent<AudioPitchEstimator>();

        Debug.Log("NAME: " + Microphone.devices[0]);
        audioSource.clip = Microphone.Start(Microphone.devices[0], true, 10, sampleRate);
        audioSource.loop = true;

        while (!(Microphone.GetPosition(null) > 0)) { }
        audioSource.Play();
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
            if (!float.IsNaN(estimatedPitch) && emitTimer >= emitInterval)
            {
                emitTimer = 0f;
                if (particleSystem != null)
                {
                    var psTransform = particleSystem.transform;
                    var psShape = particleSystem.shape;
                    var psMain = particleSystem.main;


                    Debug.Log("Rounded Pitch: " + roundedPitch);
                    // I know... should abstract this, but I wanted to test it
                    // and having chatGPT generate this slop is relatively quick
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
                        int index = 1;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(1f, 0.5f, 0f, 1f); // Orange (D4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 350 && roundedPitch < 380)
                    {
                        int index = 2;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(1f, 1f, 0f, 1f); // Yellow (D#4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 380 && roundedPitch < 410)
                    {
                        int index = 3;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0f, 1f, 0f, 1f); // Green (E4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 410 && roundedPitch < 440)
                    {
                        int index = 4;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0f, 0f, 1f, 1f); // Blue (F4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 440 && roundedPitch < 470)
                    {
                        int index = 5;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0.29f, 0f, 0.51f, 1f); // Indigo (F#4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 470 && roundedPitch < 500)
                    {
                        int index = 6;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0.56f, 0f, 1f, 1f); // Violet (G4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 500 && roundedPitch < 530)
                    {
                        int index = 7;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(1f, 1f, 1f, 1f); // White (G#4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 530 && roundedPitch < 560)
                    {
                        int index = 8;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0.5f, 0.25f, 0f, 1f); // Brown (A4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 560 && roundedPitch < 590)
                    {
                        int index = 9;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(1f, 0f, 1f, 1f); // Magenta (A#4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 590 && roundedPitch < 620)
                    {
                        int index = 10;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0f, 1f, 1f, 1f); // Cyan (B4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 620 && roundedPitch < 650)
                    {
                        int index = 11;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0.5f, 0.5f, 0f, 1f); // Olive (C5)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 650 && roundedPitch < 680)
                    {
                        int index = 12;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0.8f, 0.2f, 0.8f, 1f); // Orchid (C#5)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 680 && roundedPitch < 710)
                    {
                        int index = 13;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0.3f, 0.7f, 0.3f, 1f); // Mint Green (D5)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 710 && roundedPitch < 740)
                    {
                        int index = 14;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psTransform.rotation = Quaternion.LookRotation(direction);
                        psMain.startColor = new Color(0.6f, 0f, 0f, 1f); // Dark Red (D#5)
                        particleSystem.Emit(1);
                    }

                }
            }
            else
            {
                // Debug.Log("No clear pitch detected");
                // spectrumAnalysis.Log();
                // Parse values from string-returning methods
                float largest = spectrumAnalysis.GetLargestValue();
                float smallest = spectrumAnalysis.GetSmallestValue();

                // Particle System references

                // This is chatgpt slop, BUT - its a start
                // we now have the ability to take in values from the spectrum
                // and use them to modify the particle system
                // the sky is the limit now!
                // main.startSize = Mathf.Clamp(largest * 10f, 0.1f, 5f);
                // main.startSpeed = Mathf.Clamp(largest * 50f, 1f, 20f);
                // emission.rateOverTime = Mathf.Clamp(largest * 100f, 10f, 200f);
                // main.startLifetime = Mathf.Clamp(smallest * 10f, 0.5f, 5f);
            }
        }
    }
}
