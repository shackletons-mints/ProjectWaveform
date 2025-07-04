using UnityEngine;

namespace AudioVisualization
{
    public static class AudioConstants
    {
        public static readonly string[] PitchNames = {
            "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"
        };
        public static readonly Color Red        = new Color(1f, 0f, 0f);         // C
        public static readonly Color Orange     = new Color(1f, 0.5f, 0f);       // C#
        public static readonly Color Yellow     = new Color(1f, 1f, 0f);         // D
        public static readonly Color YellowGreen = new Color(0.5f, 1f, 0f);      // D#
        public static readonly Color Green      = new Color(0f, 1f, 0f);         // E
        public static readonly Color GreenCyan  = new Color(0f, 1f, 0.5f);       // F
        public static readonly Color Cyan       = new Color(0f, 1f, 1f);         // F#
        public static readonly Color SkyBlue    = new Color(0f, 0.5f, 1f);       // G
        public static readonly Color Blue       = new Color(0f, 0f, 1f);         // G#
        public static readonly Color Indigo     = new Color(0.29f, 0f, 0.51f);   // A
        public static readonly Color Violet     = new Color(0.56f, 0f, 1f);      // A#
        public static readonly Color Magenta    = new Color(1f, 0f, 1f);         // B

        public static readonly Color[] PitchColors = {
            // C,    C#,     D,       D#,         E,      F,
            Red, Orange, Yellow, YellowGreen, Green, GreenCyan,
            // F#,    G,       G#,   A,      A#,      B
            Cyan, SkyBlue, Blue, Indigo, Violet, Magenta
        };

		// AMERICA FILTER
		// public static readonly Color White        = new Color(1f, 1f, 1f);         // C
		// public static readonly Color LightRed     = new Color(1f, 0.6f, 0.6f);     // C#
		// public static readonly Color Red          = new Color(1f, 0f, 0f);         // D
		// public static readonly Color DeepRed      = new Color(0.6f, 0f, 0f);       // D#
		// public static readonly Color LightBlue    = new Color(0.6f, 0.8f, 1f);     // E
		// public static readonly Color Blue         = new Color(0f, 0f, 1f);         // F
		// public static readonly Color Navy         = new Color(0f, 0f, 0.5f);       // F#
		// public static readonly Color SkyBlue      = new Color(0.4f, 0.6f, 1f);     // G
		// public static readonly Color Crimson      = new Color(0.86f, 0.08f, 0.24f);// G#
		// public static readonly Color SteelBlue    = new Color(0.27f, 0.51f, 0.71f);// A
		// public static readonly Color Firebrick    = new Color(0.7f, 0.13f, 0.13f); // A#
		// public static readonly Color AmericanBlue = new Color(0.0f, 0.2f, 0.4f);   // B

		// public static readonly Color[] PitchColors = {
		// 	//  C,       C#,         D,      D#,        E,          F,
		// 	White, LightRed,    Red,   DeepRed, LightBlue,     Blue,
		// 	//  F#,   G,       G#,     A,          A#,         B
		// 	Navy, SkyBlue, Crimson, SteelBlue, Firebrick, AmericanBlue
		// };
    }
}
