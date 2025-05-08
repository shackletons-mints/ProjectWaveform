using System.Collections.Generic;
using UnityEngine;

public class SparseVector
{
    public List<(int index, float value)> vector = new List<(int, float)>();

    public SparseVector(float[] spectrum)
    {
        for (int i = 0; i < spectrum.Length; i++)
        {
            const float epsilon = 0.00001f;
            if (Mathf.Abs(spectrum[i]) > epsilon)
            {
                vector.Add((i, spectrum[i]));
            }
        }
    }

    public override string ToString()
    {
        string output = "";
        foreach (var (index, value) in vector)
        {
            output += $"[{index}]={value:F5} ";
        }
        return output.Trim();
    }
}
