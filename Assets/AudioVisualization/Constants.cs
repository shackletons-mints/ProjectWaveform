using UnityEngine;

namespace AudioVisualization
{
    public static class AudioConstants
    {
        public static readonly string[] PitchNames = {
            "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"
        };
        public static readonly Color Olive    = new Color(0.5f, 0.5f, 0f);     // C
        public static readonly Color Red      = new Color(1f, 0f, 0f);         // C#
        public static readonly Color Orange   = new Color(1f, 0.5f, 0f);       // D
        public static readonly Color Yellow   = new Color(1f, 1f, 0f);         // D#
        public static readonly Color Green    = new Color(0f, 1f, 0f);         // E
        public static readonly Color Blue     = new Color(0f, 0f, 1f);         // F
        public static readonly Color Indigo   = new Color(0.29f, 0f, 0.51f);   // F#
        public static readonly Color Violet   = new Color(0.56f, 0f, 1f);      // G
        public static readonly Color White    = new Color(1f, 1f, 1f);         // G#
        public static readonly Color Brown    = new Color(0.5f, 0.25f, 0f);    // A
        public static readonly Color Magenta  = new Color(1f, 0f, 1f);         // A#
        public static readonly Color Cyan     = new Color(0f, 1f, 1f);         // B

        public static readonly Color[] PitchColors = {
            Olive, Red, Orange, Yellow, Green, Blue,
            Indigo, Violet, White, Brown, Magenta, Cyan
        };
        public static readonly int[] ChromaticPitchPositions = {
            11, 12, 16, 21, 13, 20, 18, 19, 15, 17, 14, 10
        };

        public static readonly int[] CircleOfFifthsPositions = {
            11, 19, 16, 17, 13, 10, 18, 12, 15, 21, 14, 20
        };

        public static readonly int[] RandomPitchPositions = {
            10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21
        };
    }
}
