using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpectrumAnalysis
{
    public float[] spectrumData;

    // use this to filter out low values
    const float epsilon = 0.00001f;

    public SpectrumAnalysis(float[] spectrumData)
    {
        this.spectrumData = spectrumData;
    }

    public float GetSmallestValue()
    {
        float min = 1;
        foreach (var val in spectrumData)
        {
            if (val < min && val > epsilon)
            {
                min = val;
            }
        }

        return min;
    }

    public float GetLargestValue()
    {
        float max = -1;
        foreach (var val in spectrumData)
        {
            if (val > max)
            {
                max = val;
            }
        }
        return max;
    }

    public void Log()
    {
        float smallest = this.GetSmallestValue();
        float largest = this.GetLargestValue();
        Debug.Log("Largest Value: " + $"{largest}");
        Debug.Log("Smallest Value: " + $"{smallest}");
    }

    public override string ToString()
    {
        string output = "";
        for (int i = 0; i < spectrumData.Length; i++)
        {
            float value = spectrumData[i];
            if (value > epsilon)
            {
                output += $"[{i}]={value:F5} ";
            }
        }
        return output.Trim();
    }
}
