using UnityEngine;

namespace AudioVisualization
{
    public static class AudioAnalysisHandler
    {
        public static void AnalyzeAudio(AudioVisualizer visualizer)
        {
            var source = visualizer.audioSource;
            source.GetSpectrumData(visualizer.spectrumData, 0, visualizer.fftWindow);

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
                    Debug.Log("No clear pitch detected");
                return;
            }

            visualizer.emitTimer = 0f;
            SetConeAngle(visualizer, pitch);
            SetParticleColor(visualizer, pitchClass, pitch);
            SetParticlePosition(visualizer, pointIndex);
            SetParticleStartSpeed(visualizer, visualizer.sceneTimer);
            // SetRippleOrigin(visualizer, pitchClass);
            SetShaderColor(visualizer, pitchClass);
            SetRippleDensity(visualizer);

            visualizer.particleSystem.Emit(emitValue);
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

            foreach (var light in visualizer.highlightLights)
            {
                light.color = pitchColor;
                light.intensity = 6f;
            }
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
        }

        public static void SetRippleDensity(AudioVisualizer visualizer)
        {
            float peak = 0f;
            for (int i = 0; i < visualizer.spectrumData.Length; i++)
            {
                if (visualizer.spectrumData[i] > peak)
                    peak = visualizer.spectrumData[i];
            }

            float intensityBase = peak;
            float curved = Mathf.Pow(intensityBase, 0.25f);
            float rippleFrequency = Mathf.Lerp(0.5f, 5f, curved);

            visualizer.rippleShader.SetFloat("_RippleFrequency", rippleFrequency);
        }
    }
}

