using UnityEngine;

namespace AudioVisualization
{
    public static class AudioConstants
    {
        public static readonly string[] PitchNames = {
            "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"
        };

        public static readonly Color[] PitchColors = {
            new Color(0.5f, 0.5f, 0f), new Color(1f, 0f, 0f), new Color(1f, 0.5f, 0f),
            new Color(1f, 1f, 0f), new Color(0f, 1f, 0f), new Color(0f, 0f, 1f),
            new Color(0.29f, 0f, 0.51f), new Color(0.56f, 0f, 1f), new Color(1f, 1f, 1f),
            new Color(0.5f, 0.25f, 0f), new Color(1f, 0f, 1f), new Color(0f, 1f, 1f)
        };
    }
}
