using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace AudioVisualization
{
    public class AudioVisualizer : MonoBehaviour
    {
        private static AudioVisualizer _instance;
        public static AudioVisualizer Instance
        {
            get
            {
                return _instance;
            }
        }

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        public AudioClip audioClip;
        public AudioPitchEstimator audioPitchEstimator;
        public AudioSource audioSource;
        public AudioToggle audioToggle;
        public GameObject sphere;
        public ParticleSystem particleSystem;
        public PitchLayoutSelector layoutSelector;
        private Vector3 previousSpherePosition;
        public SphereSurfacePoints sphereSurfacePoints;
        public Material rippleShader;

        public int previousPitchClass;
        public int spectrumSize = 1024;
        public int sampleRate = 44100;
        public FFTWindow fftWindow = FFTWindow.BlackmanHarris;

        internal float[] spectrumData;

        internal float emitTimer = 0f;
        internal float emitInterval = 0.125f;
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

		public void SetAudio(AudioClip _audioClip, AudioSource _audioSource)
		{
			audioSource.Stop();

			if (_audioClip == null || _audioSource == null)
			{
                if (Microphone.devices.Length > 0)
                {
                    // Debug.Log("Switching to microphone: " + Microphone.devices[0]);
                    audioSource.clip = Microphone.Start(
                        Microphone.devices[0],
                        true,
                        10,
                        sampleRate
                    );
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
				audioClip = _audioClip;
				audioSource = _audioSource;
				audioSource.Play();
			}
		}
    }
}
