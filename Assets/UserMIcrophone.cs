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
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.GetSpectrumData(spectrumData, 0, fftWindow);
            float estimatedPitch = audioPitchEstimator.Estimate(audioSource);
            int roundedPitch = (int)Math.Round(estimatedPitch, 0);
            SpectrumAnalysis spectrumAnalysis = new SpectrumAnalysis(spectrumData);
            
            if (!float.IsNaN(estimatedPitch))
            {

                if (particleSystem != null)
                {
                    var psTransform = particleSystem.transform;
                    var psShape = particleSystem.shape;
                    var psMain = particleSystem.main;

                    Debug.Log("Rounded Pitch: " + roundedPitch);
                    if (roundedPitch >= 186 && roundedPitch <= 206) {
                        int index = 0;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psShape.rotation = Quaternion.LookRotation(direction).eulerAngles;
                        psMain.startColor = new Color(1f, 0f, 0f, 1f); // Red (G3)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 210 && roundedPitch <= 230) {
                        int index = 14;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psShape.rotation = Quaternion.LookRotation(direction).eulerAngles;
                        psMain.startColor = new Color(1f, 0.5f, 0f, 1f); // Orange (A3)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 237 && roundedPitch <= 257) {
                        int index = 28;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psShape.rotation = Quaternion.LookRotation(direction).eulerAngles;
                        psMain.startColor = new Color(1f, 1f, 0f, 1f); // Yellow (B3)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 251 && roundedPitch <= 271) {
                        int index = 42;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psShape.rotation = Quaternion.LookRotation(direction).eulerAngles;
                        psMain.startColor = new Color(0f, 1f, 0f, 1f); // Green (C4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 284 && roundedPitch <= 304) {
                        int index = 56;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psShape.rotation = Quaternion.LookRotation(direction).eulerAngles;
                        psMain.startColor = new Color(0f, 0f, 1f, 1f); // Blue (D4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 320 && roundedPitch <= 340) {
                        int index = 71;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psShape.rotation = Quaternion.LookRotation(direction).eulerAngles;
                        psMain.startColor = new Color(0.29f, 0f, 0.51f, 1f); // Indigo (E4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 360 && roundedPitch <= 380) {
                        int index = 85;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psShape.rotation = Quaternion.LookRotation(direction).eulerAngles;
                        psMain.startColor = new Color(0.56f, 0f, 1f, 1f); // Violet (F#4)
                        particleSystem.Emit(1);
                    }
                    else if (roundedPitch >= 382 && roundedPitch <= 402) {
                        int index = 99;
                        psTransform.position = sphereSurfacePoints.surfacePoints[index].position;
                        Vector3 direction = sphereSurfacePoints.surfacePoints[index].normal;
                        psShape.rotation = Quaternion.LookRotation(direction).eulerAngles;
                        psMain.startColor = new Color(1f, 1f, 1f, 1f); // White (G4)
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
