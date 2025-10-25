using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class PositionInFrontOfCamera : MonoBehaviour
{
    [Header("Position Settings")]
    [Tooltip("Distance from the camera to spawn the object")]
    public float spawnDistance = 0.75f;

    [Tooltip("Horizontal offset (left/right) from camera center")]
    public float horizontalOffset = 0.34f;

    [Tooltip("Vertical offset (up/down) from camera center")]
    public float verticalOffset = -0.17f;

    [Tooltip("Match the camera's rotation when spawning")]
    public bool matchCameraRotation = true;

    [Tooltip("Stick to relative position to camera")]
    public bool sticky = false;

    [Tooltip("The GameObject that will be positioned and oriented.")]
    public GameObject objectToPosition;

    [Header("Spawn Timing")]
    [Tooltip("Delay before initial positioning (useful if camera isn't ready)")]
    public float initialDelay = 0.1f;

    void Start()
    {
        if (initialDelay > 0)
        {
            StartCoroutine(DelayedSpawn());
        }
        else
        {
            SpawnInFrontOfCamera();
        }
    }

    private IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(initialDelay);
        SpawnInFrontOfCamera();
    }

    void Update()
    {
        if (sticky)
        {
            SpawnInFrontOfCamera();
        }
    }

    [ContextMenu("Spawn In Front Of Camera")]
    public void SpawnInFrontOfCamera()
    {
        if (Camera.main == null)
        {
            Debug.LogError("No main camera found in scene!");
            return;
        }

        // Calculate the spawn position
        Vector3 spawnPosition = CalculateSpawnPosition();

        // Move this GameObject to the calculated position
        transform.position = spawnPosition;

        // Match camera rotation - this is usually what you want
        if (matchCameraRotation)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
        else
        {
            // If not matching rotation, at least face the camera's forward direction
            transform.forward = Camera.main.transform.forward;
        }

        Debug.Log($"Spawned {gameObject.name} at position: {spawnPosition}");
    }

    private Vector3 CalculateSpawnPosition()
    {
        Transform cameraTransform = Camera.main.transform;
        
        // Get camera's position and forward direction
        Vector3 cameraPos = cameraTransform.position;
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        Vector3 cameraUp = cameraTransform.up;

        // Calculate base position in front of camera
        Vector3 basePosition = cameraPos + (cameraForward * spawnDistance);

        // Apply offsets using camera's local axes
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