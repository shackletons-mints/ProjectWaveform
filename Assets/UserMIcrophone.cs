using System;
using System.Collections.Generic;
using UnityEngine;

public class UserMicrophone : MonoBehaviour
{
    public GameObject sphere;
    public ParticleSystem particleSystemG;
    public ParticleSystem particleSystemD;
    public ParticleSystem particleSystemFsharp;
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
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>(true);
        sphere = GameObject.Find("Sphere");
        if (sphere != null)
        {
            SphereSurfacePoints sphereSurfacePoints = new SphereSurfacePoints(sphere);
            sphereSurfacePoints.LogAllSurfacePoints();
        }
        foreach (var ps in particleSystems)
        {
            if (ps.gameObject.name == "ParticleSystemG")
            {
                particleSystemG = ps;
            }
            else if (ps.gameObject.name == "ParticleSystemD")
            {
                particleSystemD = ps;
            }
            else if (ps.gameObject.name == "ParticleSystemFsharp")
            {
                particleSystemFsharp = ps;
            }

            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var emission = ps.emission;
            emission.enabled = false;

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
                // Debug.Log("Rounded Pitch: " + roundedPitch);
                if (roundedPitch >= 465 && roundedPitch <= 515) {
                    if (particleSystemG is not null)
                    {
                        particleSystemG.Emit(1);
                    }
                }
                else if (roundedPitch >= 535 && roundedPitch <= 585) {
                    if (particleSystemD is not null)
                    {
                        particleSystemFsharp.Emit(1);
                    }
                }
                else if (roundedPitch >= 175 && roundedPitch <= 225) {
                    if (particleSystemD is not null)
                    {
                        particleSystemD.Emit(1);
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
