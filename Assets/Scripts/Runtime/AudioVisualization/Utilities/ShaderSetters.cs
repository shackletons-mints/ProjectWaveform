using UnityEngine;
using AudioVisualization;

namespace Utilities
{
    public static class ShaderSetters
    {
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
    }
}

