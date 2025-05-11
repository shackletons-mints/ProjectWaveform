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

    public float GetEstimatedPitch(int maxHarmonics)
    {
        int maxIndex = 0;
        float maxMagnitude = 0f;

        for (int i = 0; i < _spectrumSize; i++)
        {
            if (frequencySpectrum[i] > maxMagnitude)
            {
                maxMagnitude = frequencySpectrum[i];
                maxIndex = i;
            }
        }

        float y0 = frequencySpectrum[maxIndex - 1];
        float y1 = frequencySpectrum[maxIndex];
        float y2 = frequencySpectrum[maxIndex + 1];

        float x0 = maxIndex - 1;
        float x1 = maxIndex;
        float x2 = maxIndex + 1;

        float denom = (x0 - x1) * (x0 - x2) * (x1 - x2);

        float a = (y0 * (x1 - x2) + y1 * (x2 - x0) + y2 * (x0 - x1)) / denom;
        float b = (y0 * (x2 * x2 - x1 * x1) + y1 * (x0 * x0 - x2 * x2) + y2 * (x1 * x1 - x0 * x0)) / denom;

        float trueIndex = -b / (2f * a);

        float dominantFrequency = (trueIndex * sampleRate) / (2f * _spectrumSize);
        return dominantFrequency;
    }

    public void LogSpectrumAnalysis()
    {
        float smallest = this.GetSmallestValue();
        float largest = this.GetLargestValue();
        float pitch = this.GetEstimatedPitch(4);
        Debug.Log("Largest Value: " + $"{largest}");
        Debug.Log("Smallest Value: " + $"{smallest}");
        Debug.Log("Estimated Pitch: " + $"{pitch}");
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
