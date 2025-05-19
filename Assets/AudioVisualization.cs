using System;
using UnityEngine;
using Utilities;
using UnityEngine.InputSystem;



public class AudioVisualization : MonoBehaviour
{
    public ParticleSystem particleSystemG;
    public ParticleSystem particleSystemD;
    public ParticleSystem particleSystemFsharp;
    public AudioSource audioSource;

    [Tooltip("Number of spectrum samples. Must be a power of 2 (e.g., 64, 128, 256, 512, 1024, 2048).")]
    public int spectrumSize = 1024;
    public int sampleRate = 44100;

    [Tooltip("FFT window type used for spectrum analysis.")]
    public FFTWindow fftWindow = FFTWindow.BlackmanHarris;
    public float[] spectrumData;
    public AudioPitchEstimator audioPitchEstimator;

    [Tooltip("Toggle between using microphone or audio clip.")]
    public bool useMicrophone = true;

    public AudioClip audioClip;
    public ToggleAudioHelper toggleAudioHelper;

    void Start()
    {
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>(true);
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

        if (toggleAudioHelper == null)
        {
            toggleAudioHelper = GetComponent<ToggleAudioHelper>();
        }

        audioPitchEstimator = GetComponent<AudioPitchEstimator>();

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
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.GetSpectrumData(spectrumData, 0, fftWindow);
            float estimatedPitch = audioPitchEstimator.Estimate(audioSource);
            int roundedPitch = (int)Math.Round(estimatedPitch, 0);
            SpectrumAnalysis spectrumAnalysis = new SpectrumAnalysis(spectrumData);

            if (!float.IsNaN(estimatedPitch))
            {
                Debug.Log("Rounded Pitch: " + roundedPitch);
                if (roundedPitch >= 465 && roundedPitch <= 515)
                {
                    if (particleSystemG is not null)
                    {
                        particleSystemG.Emit(1);
                    }
                }
                else if (roundedPitch >= 535 && roundedPitch <= 585)
                {
                    if (particleSystemD is not null)
                    {
                        particleSystemFsharp.Emit(1);
                    }
                }
                else if (roundedPitch >= 175 && roundedPitch <= 225)
                {
                    if (particleSystemD is not null)
                    {
                        particleSystemD.Emit(1);
                    }
                }

            }
            else
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
