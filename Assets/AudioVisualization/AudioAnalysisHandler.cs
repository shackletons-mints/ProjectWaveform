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

            if (float.IsNaN(pitch) || visualizer.emitTimer < visualizer.emitInterval)
            {
                if (float.IsNaN(pitch))
                    Debug.Log("No clear pitch detected");
                return;
            }

            visualizer.emitTimer = 0f;
            EmitParticles(visualizer, pitch);
        }

        private static void EmitParticles(AudioVisualizer visualizer, float pitch)
        {
            int midiNote = Mathf.FloorToInt(69 + 12 * Mathf.Log(pitch / 440f, 2));
            int pitchClass = midiNote % 12;

            string pitchName = AudioConstants.PitchNames[pitchClass];
            Color pitchColor = AudioConstants.PitchColors[pitchClass];
            int pointIndex = pitchClass + 10;

            if (visualizer.sphereSurfacePoints != null && pointIndex < visualizer.sphereSurfacePoints.surfacePoints.Count)
            {
                var psTransform = visualizer.particleSystem.transform;
                var psMain = visualizer.particleSystem.main;

                psTransform.position = visualizer.sphereSurfacePoints.surfacePoints[pointIndex].position;
                psTransform.rotation = Quaternion.LookRotation(visualizer.sphereSurfacePoints.surfacePoints[pointIndex].normal);
                psMain.startColor = pitchColor;
                visualizer.particleSystem.Emit(10);

                Debug.Log($"Emitting: {pitchName} (Freq: {pitch} Hz, MIDI: {midiNote})");
            }
        }
    }
}
