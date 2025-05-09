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

    public string GetSmallestValue() 
    {
        float min = 1;
        foreach (var val in frequencySpectrum)
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
        foreach (var val in frequencySpectrum)
        {
            if (val > max)
            {
                max = val;
            }
        }
        return $"{max}";
    }

    public string GetEstimatedPitch(int maxHarmonics)
    {
        int smallestLength = Mathf.CeilToInt(_spectrumSize / (float)maxHarmonics);
        float[] hps = new float[smallestLength];

        Array.Copy(frequencySpectrum, hps, smallestLength);

        // downsample - gives us the fundamental freq
        for (int h = 2; h <= maxHarmonics; h++)
        {
            for (int i = 0; i < smallestLength; i++)
            {
                int idx = i * h;
                if (idx < _spectrumSize)
                {
                    hps[i] *= frequencySpectrum[idx];
                }
                else
                {
                    hps[i] *= 0f; // pad with zero
                }
            }
        }

        // find max value which, in theory, is the dominant freq
        // which should point us in the direction of the pitch
        int maxIndex = 1;
        float maxValue = hps[1];

        for (int i = 2; i < smallestLength; i++)
        {
            if (hps[i] > maxValue)
            {
                maxValue = hps[i];
                maxIndex = i;
            }
        }

        float freqResolution = (sampleRate / 2f) / _spectrumSize;
        float estimatedPitch = maxIndex * freqResolution;
        return $"{estimatedPitch}";
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
