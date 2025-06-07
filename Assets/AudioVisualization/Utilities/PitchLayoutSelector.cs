using UnityEngine;

public class PitchLayoutSelector : MonoBehaviour
{
    public enum PitchLayoutMode
    {
        Chromatic,
        CircleOfFifths,
        Random
    }

    [Tooltip("Choose which pitch layout to use.")]
    public PitchLayoutMode layoutMode = PitchLayoutMode.CircleOfFifths;


	// corresponding to pitch names
    //   C, C#,  D, D#,  E,  F, F#,  G, G#,  A, A#, B
    private static readonly int[] ChromaticPitchPositions = {
        11, 12, 16, 21, 13, 20, 18, 19, 15, 17, 14, 10
    };

    private static readonly int[] CircleOfFifthsPositions = {
        11, 19, 16, 17, 13, 10, 18, 12, 15, 21, 14, 20
    };

    private static readonly int[] RandomPitchPositions = {
        10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21
    };

    private int[] CurrentPitchPositions
    {
        get
        {
            return layoutMode switch
            {
                PitchLayoutMode.Chromatic => ChromaticPitchPositions,
                PitchLayoutMode.CircleOfFifths => CircleOfFifthsPositions,
                PitchLayoutMode.Random => RandomPitchPositions,
                _ => CircleOfFifthsPositions
            };
        }
    }

    public int GetPositionForPitchClass(int pitchClass)
    {
        if (pitchClass < 0 || pitchClass > 11)
        {
            Debug.LogWarning("Invalid pitch class. Must be between 0 and 11.");
            return -1;
        }

        return CurrentPitchPositions[pitchClass];
    }

    void Update()
    {}
}
