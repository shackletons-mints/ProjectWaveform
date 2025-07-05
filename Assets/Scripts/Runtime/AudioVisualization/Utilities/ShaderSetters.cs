		// public static void SetRippleOrigin(AudioVisualizer visualizer, int pointIndex)
		// {

		// 	Vector3 rippleOrigin = visualizer.sphereSurfacePoints.surfacePoints[pointIndex].position;
		// 	visualizer.rippleShader.SetVector("_RippleOrigin", rippleOrigin);
		// }

		// public static void SetRippleFrequency(AudioVisualizer visualizer)
		// {
		// 	float energy = 0f;
		// 	for (int i = 0; i < visualizer.spectrumData.Length; i++)
		// 	{
		// 		energy += visualizer.spectrumData[i];
		// 	}

		// 	float delta = energy - visualizer.previousSpectrumEnergy;
		// 	visualizer.previousSpectrumEnergy = Mathf.Lerp(visualizer.previousSpectrumEnergy, energy, Time.deltaTime * 4f);

		// 	// Frequency limits
		// 	float minFreq = 0.5f;
		// 	float maxFreq = 8f;
		// 	float growthSpeed = 3.5f;
		// 	float normalDecaySpeed = 3f;
		// 	float silentDecaySpeed = 10f;  // fast decay for silence

		// 	bool isSilent = energy < 0.001f;

		// 	if (delta > 0.0005f)
		// 	{
		// 		visualizer.currentRippleFrequency += delta * growthSpeed;
		// 	}
		// 	else
		// 	{
		// 		float decaySpeed = isSilent ? silentDecaySpeed : normalDecaySpeed;

		// 		visualizer.currentRippleFrequency = Mathf.Lerp(
		// 			visualizer.currentRippleFrequency,
		// 			minFreq,
		// 			Time.deltaTime * decaySpeed
		// 		);
		// 	}

		// 	visualizer.currentRippleFrequency = Mathf.Clamp(
		// 		visualizer.currentRippleFrequency,
		// 		minFreq,
		// 		maxFreq
		// 	);

		// 	visualizer.rippleShader.SetFloat("_RippleFrequency", visualizer.currentRippleFrequency);
		// }

		// public static void SetRippleDensity(AudioVisualizer visualizer)
		// {
		// 	int activeBands = 0;
		// 	float threshold = 0.005f;

		// 	for (int i = 0; i < visualizer.spectrumData.Length; i++)
		// 	{
		// 		if (visualizer.spectrumData[i] > threshold)
		// 			activeBands++;
		// 	}

		// 	float fullness = (float)activeBands / visualizer.spectrumData.Length;
		// 	float curved = Mathf.Pow(fullness, 0.75f);
		// 	float rippleDensity = Mathf.Lerp(2f, 20f, curved);

		// 	visualizer.currentRippleDensity = Mathf.Lerp(
		// 		visualizer.currentRippleDensity,
		// 		rippleDensity,
		// 		Time.deltaTime * visualizer.smoothingSpeed
		// 	);

		// 	visualizer.rippleShader.SetFloat("_RippleDensity", visualizer.currentRippleDensity);
		// }

		// public static void SetEffectRadius(AudioVisualizer visualizer)
		// {
		// 	float weightedSum = 0f;
		// 	float weightTotal = 0f;

		// 	for (int i = 0; i < visualizer.spectrumData.Length; i++)
		// 	{
		// 		float weight = 1f - (float)i / visualizer.spectrumData.Length; // Lower freqs heavier
		// 		weightedSum += visualizer.spectrumData[i] * weight;
		// 		weightTotal += weight;
		// 	}

		// 	float average = (weightTotal > 0f) ? weightedSum / weightTotal : 0f;
		// 	float curved = Mathf.Pow(average, 0.5f);
		// 	float radius = Mathf.Lerp(0.1f, 2f, curved);

		// 	visualizer.currentEffectRadius = Mathf.Lerp(
		// 		visualizer.currentEffectRadius,
		// 		radius,
		// 		Time.deltaTime * visualizer.smoothingSpeed
		// 	);

		// 	visualizer.rippleShader.SetFloat("_EffectRadius", visualizer.currentEffectRadius);
		// }

		// public static void SetRippleAmplitude(AudioVisualizer visualizer, float overrideVal)
		// {
		// 	if (overrideVal == 0f)
		// 	{
		// 		visualizer.rippleShader.SetFloat("_RippleAmplitude", overrideVal);
		// 	}
		// 	else
		// 	{

		// 		float peak = 0f;
		// 		for (int i = 0; i < visualizer.spectrumData.Length; i++)
		// 		{
		// 			peak = Mathf.Max(peak, visualizer.spectrumData[i]);
		// 		}

		// 		float curved = Mathf.Pow(peak, 0.35f); // more responsive for quiet dynamics
		// 		float amplitude = Mathf.Lerp(0.05f, 0.19f, curved);

		// 		visualizer.rippleShader.SetFloat("_RippleAmplitude", amplitude);
		// 	}
		// }

