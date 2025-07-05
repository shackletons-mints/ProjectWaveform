using UnityEngine;

namespace AudioVisualization
{
	public static class AudioAnalysisHandler
	{
		public static void AnalyzeAudio(AudioVisualizer visualizer)
		{
			var source = visualizer.audioSource;
			source.GetSpectrumData(visualizer.spectrumData, 0, visualizer.fftWindow);
			visualizer.fluxAnalyzer.AnalyzeSpectrum(visualizer.spectrumData, Time.time);

			visualizer.fluxAnalyzer.OnPeakDetected += time => Debug.Log($"Beat detected at {time:F2}s!");

			float pitch = visualizer.audioPitchEstimator.Estimate(source);
			int midiNote = Mathf.RoundToInt(69 + 12 * Mathf.Log(pitch / 440f, 2));
			int pitchClass = midiNote % 12;
			if (pitchClass < 0 || pitchClass >= AudioConstants.PitchNames.Length)
			{
				Debug.LogWarning($"Pitch class {pitchClass} is out of bounds for pitch {pitch}.");
				return;
			}
			string pitchName = AudioConstants.PitchNames[pitchClass];
			int pointIndex = visualizer.layoutSelector.GetPositionForPitchClass(pitchClass);
			float volume = CalculateVolume(visualizer.spectrumData);
			int emitValue = CalculateEmitValue(volume);

			if (float.IsNaN(pitch) || visualizer.emitTimer < visualizer.emitInterval)
			{
				if (float.IsNaN(pitch))
				{
					Debug.Log("No clear pitch detected");
					return;
				}
			}

			if (pitchClass == visualizer.previousPitchClass)
			{
				return;
			}

			SetParticleStartSpeed(visualizer, visualizer.sceneTimer);
			SetParticleColor(visualizer, pitchClass, pitch);
			SetParticlePosition(visualizer, pointIndex);
			SetShaderColor(visualizer, pitchClass);
			SetConeAngle(visualizer, pitch);
			// SetRippleDensity(visualizer);
			// SetRippleAmplitude(visualizer, 1f);
			// SetRippleFrequency(visualizer);


			visualizer.emitTimer = 0f;

			visualizer.particleSystem.Emit(emitValue);
			visualizer.previousPitchClass = pitchClass;

			Debug.Log($"Emitting: {pitchName} (Freq: {pitch} Hz, MIDI: {midiNote}), particles: {emitValue}");
		}

		public static float CalculateVolume(float[] spectrumData)
		{
			if (spectrumData == null || spectrumData.Length == 0)
				return -80f;

			float sum = 0f;
			for (int i = 0; i < spectrumData.Length; i++)
			{
				sum += spectrumData[i] * spectrumData[i];
			}

			float rms = Mathf.Sqrt(sum / spectrumData.Length);
			float db = 20f * Mathf.Log10(rms);

			if (float.IsInfinity(db) || float.IsNaN(db))
				db = -80f;

			return Mathf.Clamp(db, -80f, 0f);
		}

		public static int CalculateEmitValue(float dbLevel)
		{
			const float minDb = -80f;
			const float maxDb = -30f;

			dbLevel = Mathf.Clamp(dbLevel, minDb, maxDb);
			float normalized = (dbLevel - minDb) / (maxDb - minDb);
			normalized = Mathf.Max(normalized, 0.001f);
			float exponent = 1.5f;
			float adjusted = Mathf.Pow(normalized, exponent);
			int emitValue = Mathf.RoundToInt(Mathf.Lerp(1f, 12f, adjusted));

			return emitValue;
		}

		public static float GetNormalizedPitchDeviation(float detectedPitch)
		{
			float midiNoteFloat = 69 + 12 * Mathf.Log(detectedPitch / 440f, 2);
			int nearestMidiNote = Mathf.RoundToInt(midiNoteFloat);

			float nearestFreq = 440f * Mathf.Pow(2f, (nearestMidiNote - 69) / 12f);
			float centsDifference = 1200f * Mathf.Log(detectedPitch / nearestFreq, 2);
			float absCentsDiff = Mathf.Abs(centsDifference);
			float maxCents = 50f;

			return Mathf.Clamp01(absCentsDiff / maxCents);
		}


		public static void SetConeAngle(AudioVisualizer visualizer, float detectedPitch)
		{
			float normalizedDeviation = GetNormalizedPitchDeviation(detectedPitch);

			float angle = Mathf.Lerp(5f, 30f, normalizedDeviation);

			float randomness = Mathf.Lerp(0.05f, 0.7f, normalizedDeviation);

			var shape = visualizer.particleSystem.shape;
			shape.angle = angle;
			shape.randomDirectionAmount = randomness;
		}

		private static void SetParticleColor(AudioVisualizer visualizer, int pitchClass, float detectedPitch)
		{
			var psMain = visualizer.particleSystem.main;
			Color pitchColor = AudioConstants.PitchColors[pitchClass];
			float normalizedDeviation = GetNormalizedPitchDeviation(detectedPitch);
			pitchColor.a = normalizedDeviation;
			psMain.startColor = pitchColor;
		}

		private static void SetParticlePosition(AudioVisualizer visualizer, int pointIndex)
		{
			if (visualizer.sphereSurfacePoints != null && pointIndex < visualizer.sphereSurfacePoints.surfacePoints.Count)
			{
				var psTransform = visualizer.particleSystem.transform;
				psTransform.position = visualizer.sphereSurfacePoints.surfacePoints[pointIndex].position;
				psTransform.rotation = Quaternion.LookRotation(visualizer.sphereSurfacePoints.surfacePoints[pointIndex].normal);
			}
		}

		private static void SetParticleStartSpeed(AudioVisualizer visualizer, float elapsedTime)
		{
			var psMain = visualizer.particleSystem.main;
			float startSpeed = (elapsedTime * 0.02f) + 0.5f;
			psMain.startSpeed = startSpeed;
		}

		public static void SetRippleOrigin(AudioVisualizer visualizer, int pointIndex)
		{

			Vector3 rippleOrigin = visualizer.sphereSurfacePoints.surfacePoints[pointIndex].position;
			visualizer.rippleShader.SetVector("_RippleOrigin", rippleOrigin);
		}

		public static void SetShaderColor(AudioVisualizer visualizer, int pitchClass)
		{
			Color pitchColor = AudioConstants.PitchColors[pitchClass];
			visualizer.rippleShader.SetColor("_EmissionColor", pitchColor);

			Color previousPitchColor = AudioConstants.PitchColors[visualizer.previousPitchClass];
			if (pitchColor != previousPitchColor)
			{
				visualizer.rippleShader.SetColor("_PreviousColor", previousPitchColor);
			}
		}

		public static void SetRippleFrequency(AudioVisualizer visualizer)
		{
			float energy = 0f;
			for (int i = 0; i < visualizer.spectrumData.Length; i++)
			{
				energy += visualizer.spectrumData[i];
			}

			float delta = energy - visualizer.previousSpectrumEnergy;
			visualizer.previousSpectrumEnergy = Mathf.Lerp(visualizer.previousSpectrumEnergy, energy, Time.deltaTime * 4f);

			// Frequency limits
			float minFreq = 0.5f;
			float maxFreq = 8f;
			float growthSpeed = 3.5f;
			float normalDecaySpeed = 3f;
			float silentDecaySpeed = 10f;  // fast decay for silence

			bool isSilent = energy < 0.001f;

			if (delta > 0.0005f)
			{
				visualizer.currentRippleFrequency += delta * growthSpeed;
			}
			else
			{
				float decaySpeed = isSilent ? silentDecaySpeed : normalDecaySpeed;

				visualizer.currentRippleFrequency = Mathf.Lerp(
					visualizer.currentRippleFrequency,
					minFreq,
					Time.deltaTime * decaySpeed
				);
			}

			visualizer.currentRippleFrequency = Mathf.Clamp(
				visualizer.currentRippleFrequency,
				minFreq,
				maxFreq
			);

			visualizer.rippleShader.SetFloat("_RippleFrequency", visualizer.currentRippleFrequency);
		}

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

			visualizer.currentRippleDensity = Mathf.Lerp(
				visualizer.currentRippleDensity,
				rippleDensity,
				Time.deltaTime * visualizer.smoothingSpeed
			);

			visualizer.rippleShader.SetFloat("_RippleDensity", visualizer.currentRippleDensity);
		}

		public static void SetEffectRadius(AudioVisualizer visualizer)
		{
			float weightedSum = 0f;
			float weightTotal = 0f;

			for (int i = 0; i < visualizer.spectrumData.Length; i++)
			{
				float weight = 1f - (float)i / visualizer.spectrumData.Length; // Lower freqs heavier
				weightedSum += visualizer.spectrumData[i] * weight;
				weightTotal += weight;
			}

			float average = (weightTotal > 0f) ? weightedSum / weightTotal : 0f;
			float curved = Mathf.Pow(average, 0.5f);
			float radius = Mathf.Lerp(0.1f, 2f, curved);

			visualizer.currentEffectRadius = Mathf.Lerp(
				visualizer.currentEffectRadius,
				radius,
				Time.deltaTime * visualizer.smoothingSpeed
			);

			visualizer.rippleShader.SetFloat("_EffectRadius", visualizer.currentEffectRadius);
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

				float curved = Mathf.Pow(peak, 0.35f); // more responsive for quiet dynamics
				float amplitude = Mathf.Lerp(0.05f, 0.19f, curved);

				visualizer.rippleShader.SetFloat("_RippleAmplitude", amplitude);
			}
		}
	}
}

