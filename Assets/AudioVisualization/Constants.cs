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
    }
}
