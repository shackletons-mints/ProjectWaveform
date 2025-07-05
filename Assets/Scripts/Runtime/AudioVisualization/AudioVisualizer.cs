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
        [SerializeField] public AudioToggle audioToggle;
        public GameObject sphere;
        public ParticleSystem particleSystem;
        public PitchLayoutSelector layoutSelector;
        private Vector3 previousSpherePosition;
        public SphereSurfacePoints sphereSurfacePoints;
        public Material rippleShader;
		public SpectralFluxAnalyzer fluxAnalyzer;

		public int previousPitchClass;
		public float previousSpectrumEnergy = 0f;
        public float currentRippleFrequency = 1f;
        public float currentRippleDensity = 4f;
        public float currentEffectRadius = 1.4f;
        public float currentRippleAmplitude = 0.07f;
        public float smoothingSpeed = 5f;

        public int spectrumSize = 1024;
        public int sampleRate = 44100;
        public FFTWindow fftWindow = FFTWindow.BlackmanHarris;

        internal float[] spectrumData;

        internal float emitTimer = 0f;
        internal float emitInterval = 0.25f;
        public float sceneTimer = 0f;

        void Start()
        {
            AudioInitializer.InitializeReferences(this);
            AudioInitializer.InitializeSphere(this);
            AudioInitializer.InitializeAudio(this, audioToggle);
            spectrumData = new float[spectrumSize];
            if (sphere != null)
            {
                previousSpherePosition = sphere.transform.position;
            }
        }

        void Update()
        {
            emitTimer += Time.deltaTime;
            sceneTimer += Time.deltaTime;

            if (sphere != null)
            {
                Vector3 currentPosition = sphere.transform.position;
                if (currentPosition != previousSpherePosition)
                {
                    sphereSurfacePoints?.SetPosition();
                    previousSpherePosition = currentPosition;
                }
            }

            if (audioSource != null && audioSource.isPlaying)
            {
                AudioAnalysisHandler.AnalyzeAudio(this);
            }

            InputHandler.HandleInput(this);
        }
    }
}
