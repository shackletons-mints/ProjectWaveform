using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PositionInFrontOfCamera : MonoBehaviour
{
    [Header("References")]
    [Header("Camera Settings")]
    [Tooltip("The camera to spawn the object in front of")]
    public Camera targetCamera;

    [Header("Position Settings")]
    [Tooltip("Distance from the camera to spawn the object")]
    public float spawnDistance = 0.75f;

    [Tooltip("Horizontal offset (left/right) from camera center")]
    public float horizontalOffset = 0.34f;

    [Tooltip("Vertical offset (up/down) from camera center")]
    public float verticalOffset = -0.17f;

    [Tooltip("Match the camera's rotation when spawning")]
    public bool matchCameraRotation = false;

    [Tooltip("Stick to relative position to camera")]
    public bool sticky = false;
    
    [Tooltip("The GameObject that will be positioned and oriented.")]
    public GameObject objectToPosition; // Renamed to avoid conflict with inherited 'gameObject'

    void Start()
    {
		SpawnInFrontOfCamera();
    }

    [ContextMenu("Spawn In Front Of Camera")]
    public void SpawnInFrontOfCamera()
    {
        if (targetCamera == null)
        {
            Debug.LogError("No target camera assigned!");
            return;
        }

        // Calculate the spawn position
        Vector3 spawnPosition = CalculateSpawnPosition();

        // Move this GameObject to the calculated position
        transform.position = spawnPosition;

        // Optionally match camera rotation
        if (matchCameraRotation)
        {
            transform.rotation = targetCamera.transform.rotation;
        }

        Debug.Log($"Spawned {gameObject.name} at position: {spawnPosition}");
    }

    private Vector3 CalculateSpawnPosition()
    {
        // Get camera's position and forward direction
        Vector3 cameraPos = targetCamera.transform.position;
        Vector3 cameraForward = targetCamera.transform.forward;
        Vector3 cameraRight = targetCamera.transform.right;
        Vector3 cameraUp = targetCamera.transform.up;

        // Calculate base position in front of camera
        Vector3 basePosition = cameraPos + (cameraForward * spawnDistance);

        // Apply offsets
        Vector3 finalPosition =
            basePosition + (cameraRight * horizontalOffset) + (cameraUp * verticalOffset);

        return finalPosition;
    }

    // Method to update position in real-time (useful for testing)
    [ContextMenu("Update Position")]
    public void UpdatePosition()
    {
        SpawnInFrontOfCamera();
    }
}

