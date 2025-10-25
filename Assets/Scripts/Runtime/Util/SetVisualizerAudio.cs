using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using AudioVisualization;

public class SetVisualizerAudio : MonoBehaviour
{
	[Header("References")]
	public AudioClip audioClip;
	public AudioSource audioSource;

	public void SetAudio()
	{
        if (AudioVisualizer.Instance != null)
        {
            AudioVisualizer.Instance.SetAudio(audioClip, audioSource);
        }
        else
        {
            Debug.LogError("AudioVisualizer.Instance is null. Ensure the AudioVisualizer object is present in the scene and the game is running.");
        }
	}
}

