using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using UnityEngine.InputSystem;

namespace AudioVisualization
{
    public class AudioVisualizer : MonoBehaviour
    {
        public AudioClip audioClip;
        public AudioPitchEstimator audioPitchEstimator;
        public AudioSource audioSource;
        public AudioToggle audioToggle;
        public GameObject sphere;
        public List<Light> highlightLights;
        public ParticleSystem particleSystem;
        public PitchLayoutSelector layoutSelector;
        public SphereSurfacePoints sphereSurfacePoints;

        public int spectrumSize = 1024;
        public int sampleRate = 44100;
        public FFTWindow fftWindow = FFTWindow.BlackmanHarris;

        public bool useMicrophone = true;

        internal float[] spectrumData;

        internal float emitTimer = 0f;
        internal float emitInterval = 0.125f;
        public float sceneTimer = 0f;

        void Start()
        {
            AudioInitializer.InitializeReferences(this);
            AudioInitializer.InitializeSphere(this);
            AudioInitializer.InitializeAudio(this);
            spectrumData = new float[spectrumSize];
        }

        void Update()
        {
            emitTimer += Time.deltaTime;
            sceneTimer += Time.deltaTime;
            if (audioSource != null && audioSource.isPlaying)
            {
                AudioAnalysisHandler.AnalyzeAudio(this);
            }

            InputHandler.HandleInput(this);
        }
    }
}
