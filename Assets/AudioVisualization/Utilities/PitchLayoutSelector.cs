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
        11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0
    };

    private static readonly int[] CircleOfFifthsPositions = {
        5, 10, 3, 8, 1, 6, 11, 4, 9, 2, 7, 0
    };

    private static readonly int[] RandomPitchPositions = {
        3, 8, 11, 5, 7, 10, 2, 6, 0, 9, 1, 4
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
