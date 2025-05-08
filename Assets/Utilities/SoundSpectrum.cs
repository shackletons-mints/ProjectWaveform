using System.Collections.Generic;
using UnityEngine;

public class SoundSpectrum
{

    public float[] data;
    public int sampleRate;
    private int _size;
    // use this to filter out low values
    const float epsilon = 0.00001f;
    public SoundSpectrum(float[] spectrumData, int spectrumSize, int sampleRate)
    {
        data = spectrumData;
        this.sampleRate = sampleRate;
        _size = spectrumSize;
    }

    public string GetSmallestValue() 
    {
        float min = 1;
        foreach (var val in data)
        {
            if (val < min && val > epsilon)
            {
                min = val;
            }
        }

        return $"{min}";
    }

    public string GetLargestValue() 
    {
        float max = -1;
        foreach (var val in data)
        {
            if (val > max)
            {
                max = val;
            }
        }
        return $"{max}";
    }

    public string GetEstimatedPitch()
    {
        int maxHarmonics = 4;
        return $"{EstimatePitchHPS(maxHarmonics)}";
    }

    float EstimatePitchHPS(int maxHarmonics)
    {
        int len = data.Length;
        float[] hps = new float[len];

        // Copy original Data
        for (int i = 0; i < len; i++)
            hps[i] = data[i];

        // Multiply with downsampled versions
        for (int h = 2; h <= maxHarmonics; h++)
        {
            for (int i = 0; i < len / h; i++)
            {
                hps[i] *= data[i * h];
            }
        }

        // Find index of max value
        int maxIndex = 1;
        float maxValue = hps[1]; // skip bin 0 (DC component)

        for (int i = 2; i < len / maxHarmonics; i++) // Limit search range
        {
            if (hps[i] > maxValue)
            {
                maxValue = hps[i];
                maxIndex = i;
            }
        }

        float freqResolution = (sampleRate / 2f) / len;
        return maxIndex * freqResolution;
    }

    public override string ToString()
    {
        string output = "";
        for (int i = 0; i < data.Length; i++)
        {
            float value = data[i];
            if (value > epsilon)
            {
                output += $"[{i}]={value:F5} ";
            }
        }
        return output.Trim();
    }
}
