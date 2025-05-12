using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundSpectrum
{

    public float[] frequencySpectrum;
    public int sampleRate;
    private int _spectrumSize;
    // use this to filter out low values
    const float epsilon = 0.00001f;
    public SoundSpectrum(float[] spectrumData, int spectrumSize, int sampleRate)
    {
        frequencySpectrum = spectrumData;
        this.sampleRate = sampleRate;
        _spectrumSize = spectrumSize;
    }

    public float GetSmallestValue() 
    {
        float min = 1;
        foreach (var val in frequencySpectrum)
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
        foreach (var val in frequencySpectrum)
        {
            if (val > max)
            {
                max = val;
            }
        }
        return max;
    }

    public void LogSpectrumAnalysis()
    {
        float smallest = this.GetSmallestValue();
        float largest = this.GetLargestValue();
        Debug.Log("Largest Value: " + $"{largest}");
        Debug.Log("Smallest Value: " + $"{smallest}");
    }

    public override string ToString()
    {
        string output = "";
        for (int i = 0; i < frequencySpectrum.Length; i++)
        {
            float value = frequencySpectrum[i];
            if (value > epsilon)
            {
                output += $"[{i}]={value:F5} ";
            }
        }
        return output.Trim();
    }
}
