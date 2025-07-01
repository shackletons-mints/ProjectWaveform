using System.Linq;
using UnityEngine;
using Utilities;

namespace AudioVisualization
{
    public static class AudioInitializer
    {
        public static void InitializeReferences(AudioVisualizer visualizer)
        {
            visualizer.particleSystem ??= visualizer.GetComponent<ParticleSystem>();
            visualizer.particleSystem?.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            visualizer.audioSource ??= visualizer.GetComponent<AudioSource>();
            visualizer.audioToggle ??= visualizer.GetComponent<AudioToggle>();
            visualizer.audioPitchEstimator ??= visualizer.GetComponent<AudioPitchEstimator>();
            visualizer.layoutSelector ??= visualizer.GetComponent<PitchLayoutSelector>();

			Light[] lights = visualizer.GetComponents<Light>();
			foreach (var light in lights)
			{
				visualizer.highlightLights.Add(light);
			}
        }

        public static void InitializeSphere(AudioVisualizer visualizer)
        {
            visualizer.sphere ??= GameObject.Find("Sphere");
            visualizer.sphereSurfacePoints = visualizer.sphere?.GetComponent<SphereSurfacePoints>();
            visualizer.sphereSurfacePoints?.GenerateSurfacePoints();
			visualizer.rippleShader = visualizer.sphere?.GetComponent<Renderer>().material;
        }

        public static void InitializeAudio(AudioVisualizer visualizer, AudioToggle audioToggle)
        {
            var source = visualizer.audioSource;
            if (audioToggle.isUsingMicrophone && Microphone.devices.Length > 0)
            {
                string mic = Microphone.devices[0];
                Debug.Log("Using microphone: " + mic);
                source.clip = Microphone.Start(mic, true, 10, visualizer.sampleRate);
                source.loop = true;
                while (Microphone.GetPosition(null) <= 0) { }
                source.Play();
            }
            else if (!audioToggle.isUsingMicrophone && visualizer.audioClip != null)
            {
                Debug.Log("Using audio clip.");
                source.clip = visualizer.audioClip;
                source.loop = true;
                source.Play();
            }
            else
            {
                Debug.LogWarning("No valid audio source found.");
            }
        }
    }
}
