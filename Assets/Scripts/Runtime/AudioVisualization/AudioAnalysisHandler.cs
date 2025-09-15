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
            int particlesToEmit = Mathf.RoundToInt(CalculateLerpedValue(volume, 1f, 12f)); // 1 particle on low volumes, 12 on high
            float particleStartSize = CalculateLerpedValue(volume, 0.02f, 0.07f);

            if (float.IsNaN(pitch) || visualizer.emitTimer < visualizer.emitInterval)
            {
                return;
            }

            // SetParticleStartSpeed(visualizer, visualizer.sceneTimer);
            SetParticleColor(visualizer, pitchClass, pitch);
            SetParticlePosition(visualizer, pointIndex);
			SetParticleStartSize(visualizer, particleStartSize);
            Utilities.ShaderSetters.SetShaderColor(visualizer, pitchClass);
            SetConeAngle(visualizer, pitch); // the more centered the pitch, the more straight it's trajectory

            visualizer.emitTimer = 0f;
            visualizer.previousPitchClass = pitchClass;

            visualizer.particleSystem.Emit(particlesToEmit);

            Debug.Log(
                $"Emitting: {pitchName} (Freq: {pitch} Hz, MIDI: {midiNote}), particles: {particlesToEmit}"
            );
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

        public static float CalculateLerpedValue(float dbLevel, float low, float high)
        {
            const float minDb = -80f;
            const float maxDb = -30f;

            dbLevel = Mathf.Clamp(dbLevel, minDb, maxDb);
            float normalized = (dbLevel - minDb) / (maxDb - minDb);
            normalized = Mathf.Max(normalized, 0.001f);
            float exponent = 1.5f;
            float adjusted = Mathf.Pow(normalized, exponent);
            float emitValue = Mathf.Lerp(low, high, adjusted);

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

        private static void SetParticleColor(
            AudioVisualizer visualizer,
            int pitchClass,
            float detectedPitch
        )
        {
            var psMain = visualizer.particleSystem.main;
            Color pitchColor = AudioConstants.PitchColors[pitchClass];
            float normalizedDeviation = GetNormalizedPitchDeviation(detectedPitch);
            pitchColor.a = normalizedDeviation;
            psMain.startColor = pitchColor;
        }

        private static void SetParticlePosition(AudioVisualizer visualizer, int pointIndex)
        {
            if (
                visualizer.sphereSurfacePoints != null
                && pointIndex < visualizer.sphereSurfacePoints.surfacePoints.Count
            )
            {
                var psTransform = visualizer.particleSystem.transform;
                psTransform.position = visualizer
                    .sphereSurfacePoints
                    .surfacePoints[pointIndex]
                    .position;
                psTransform.rotation = Quaternion.LookRotation(
                    visualizer.sphereSurfacePoints.surfacePoints[pointIndex].normal
                );
            }
        }

        private static void SetParticleStartSpeed(AudioVisualizer visualizer, float elapsedTime)
        {
            var psMain = visualizer.particleSystem.main;
            float startSpeed = (elapsedTime * 0.02f) + 0.5f;
            psMain.startSpeed = startSpeed;
        }

        private static void SetParticleStartSize(AudioVisualizer visualizer, float startSize)
        {
            var psMain = visualizer.particleSystem.main;
            psMain.startSize = startSize;
        }
    }
}
