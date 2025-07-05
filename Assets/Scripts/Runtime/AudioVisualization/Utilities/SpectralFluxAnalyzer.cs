using System.Collections.Generic;
using UnityEngine;

public class SpectralFluxAnalyzer : MonoBehaviour
{
    [Tooltip("Size of the sliding window used for threshold detection")]
    public int thresholdWindowSize = 50;

    [Tooltip("Multiplier for threshold sensitivity")]
    public float thresholdMultiplier = 1.5f;

    private List<SpectralFluxInfo> spectralFluxSamples = new List<SpectralFluxInfo>();
    private float[] previousSpectrumData;
    private int indexToProcess = 0;
	public event System.Action<float> OnPeakDetected; // float = time of peak

    public class SpectralFluxInfo
    {
        public float time;
        public float spectralFlux;
        public float threshold;
        public float prunedSpectralFlux;
        public bool isPeak;
    }

    public void AnalyzeSpectrum(float[] spectrum, float time)
    {
	// Set spectrum
	setCurSpectrum(spectrum);

	// Get current spectral flux from spectrum
	SpectralFluxInfo curInfo = new SpectralFluxInfo();
	curInfo.time = time;
	curInfo.spectralFlux = calculateRectifiedSpectralFlux ();
	spectralFluxSamples.Add (curInfo);

	// We have enough samples to detect a peak
	if (spectralFluxSamples.Count >= thresholdWindowSize) {
		// Get Flux threshold of time window surrounding index to process
		spectralFluxSamples[indexToProcess].threshold = getFluxThreshold (indexToProcess);

		// Only keep amp amount above threshold to allow peak filtering
		spectralFluxSamples[indexToProcess].prunedSpectralFlux = getPrunedSpectralFlux(indexToProcess);

		// Now that we are processed at n, n-1 has neighbors (n-2, n) to determine peak
		int indexToDetectPeak = indexToProcess - 1;

		bool curPeak = isPeak (indexToDetectPeak);

		if (curPeak) {
			spectralFluxSamples [indexToDetectPeak].isPeak = true;
		}
		indexToProcess++;
	}
	else {
		Debug.Log(string.Format("Not ready yet.  At spectral flux sample size of {0} growing to {1}", spectralFluxSamples.Count, thresholdWindowSize));
	}    }

    public static float CalculateRectifiedSpectralFlux(float[] curSpectrum, float[] prevSpectrum)
    {
        float sum = 0f;
        for (int i = 0; i < curSpectrum.Length; i++)
        {
            sum += Mathf.Max(0f, curSpectrum[i] - prevSpectrum[i]);
        }
        return sum;
    }

    private float GetFluxThreshold(int index)
    {
        int start = Mathf.Max(0, index - thresholdWindowSize / 2);
        int end = Mathf.Min(spectralFluxSamples.Count - 1, index + thresholdWindowSize / 2);

        float sum = 0f;
        for (int i = start; i <= end; i++)
        {
            sum += spectralFluxSamples[i].spectralFlux;
        }

        float avg = sum / (end - start + 1);
        return avg * thresholdMultiplier;
    }

    private float GetPrunedSpectralFlux(int index)
    {
        return Mathf.Max(0f, spectralFluxSamples[index].spectralFlux - spectralFluxSamples[index].threshold);
    }

    private bool IsPeak(int index)
    {
        if (index <= 0 || index >= spectralFluxSamples.Count - 1)
        {
            return false;
        }

        float current = spectralFluxSamples[index].prunedSpectralFlux;
        return current > spectralFluxSamples[index - 1].prunedSpectralFlux &&
               current > spectralFluxSamples[index + 1].prunedSpectralFlux;
    }

    private void SetCurrentSpectrum(float[] spectrum)
    {
        spectrum.CopyTo(previousSpectrumData, 0);
    }

    // Optional: public getter for external access
    public List<SpectralFluxInfo> GetFluxSamples()
    {
        return spectralFluxSamples;
    }
}
