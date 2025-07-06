using UnityEngine;
using AudioVisualization;

namespace Utilities

{
    public static class ShaderSetters
    {
        public static void SetRippleOrigin(AudioVisualizer visualizer, int pointIndex)
        {
            Vector3 rippleOrigin = visualizer.sphereSurfacePoints.surfacePoints[pointIndex].position;
            visualizer.rippleShader.SetVector("_RippleOrigin", rippleOrigin);
        }

		public static void SetShaderColor(AudioVisualizer visualizer, int pitchClass)
		{
			Color pitchColor = AudioConstants.PitchColors[pitchClass];
			Color previousPitchColor = AudioConstants.PitchColors[visualizer.previousPitchClass];
			visualizer.rippleShader.SetColor("_EmissionColor", pitchColor);

			if (pitchColor != previousPitchColor)
			{
				visualizer.rippleShader.SetColor("_PreviousColor", previousPitchColor);
			}
		}

        public static void SetRippleFrequency(AudioVisualizer visualizer)
        {}

        public static void SetRippleDensity(AudioVisualizer visualizer)
        {
            int activeBands = 0;
            float threshold = 0.005f;

            for (int i = 0; i < visualizer.spectrumData.Length; i++)
            {
                if (visualizer.spectrumData[i] > threshold)
                    activeBands++;
            }

            float fullness = (float)activeBands / visualizer.spectrumData.Length;
            float curved = Mathf.Pow(fullness, 0.75f);
            float rippleDensity = Mathf.Lerp(2f, 20f, curved);

            visualizer.rippleShader.SetFloat("_RippleDensity", rippleDensity);
        }

        public static void SetEffectRadius(AudioVisualizer visualizer)
        {
            float weightedSum = 0f;
            float weightTotal = 0f;

            for (int i = 0; i < visualizer.spectrumData.Length; i++)
            {
                float weight = 1f - (float)i / visualizer.spectrumData.Length;
                weightedSum += visualizer.spectrumData[i] * weight;
                weightTotal += weight;
            }

            float average = (weightTotal > 0f) ? weightedSum / weightTotal : 0f;
            float curved = Mathf.Pow(average, 0.5f);
            float radius = Mathf.Lerp(0.1f, 2f, curved);

            visualizer.rippleShader.SetFloat("_EffectRadius", radius);
        }

        public static void SetRippleAmplitude(AudioVisualizer visualizer, float overrideVal)
        {
            if (overrideVal == 0f)
            {
                visualizer.rippleShader.SetFloat("_RippleAmplitude", overrideVal);
            }
            else
            {
                float peak = 0f;
                for (int i = 0; i < visualizer.spectrumData.Length; i++)
                {
                    peak = Mathf.Max(peak, visualizer.spectrumData[i]);
                }

                float curved = Mathf.Pow(peak, 0.35f);
                float amplitude = Mathf.Lerp(0.05f, 0.19f, curved);

                visualizer.rippleShader.SetFloat("_RippleAmplitude", amplitude);
            }
        }
    }
}

