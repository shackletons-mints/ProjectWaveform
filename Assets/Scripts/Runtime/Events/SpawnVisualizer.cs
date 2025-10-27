using System.Collections;
using AudioVisualization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SpawnVisualizer : MonoBehaviour
{
    public static SpawnVisualizer Instance { get; private set; }

    [Header("References")]
    public GameObject prefab;
    public float distanceFromCamera = 1f;
    public GameObject visualizer;
    public AudioVisualizer audioVisualizer;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        Transform cam = Camera.main.transform;
        Vector3 targetPosition = cam.position + (cam.forward * distanceFromCamera);
        Quaternion targetRotation = cam.rotation;

        if (visualizer == null)
            visualizer = Instantiate(prefab, targetPosition, targetRotation);

        Rigidbody rb = visualizer.GetComponentInChildren<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    IEnumerator Start()
    {
        GrowVisuals();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(2);
    }

    public void GrowVisuals()
    {
        StartCoroutine(GrowOverTime(visualizer));
    }

    public void DropVisuals()
    {
        Rigidbody rb = visualizer.GetComponentInChildren<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }

    private IEnumerator GrowOverTime(GameObject obj)
    {
        float value = 0.01f;
        Vector3 scale = new Vector3(value, value, value);
        float targetValue = 1f;

        while (obj.transform.localScale.x < targetValue)
        {
            obj.transform.localScale += scale;
            yield return null;
        }
        DropVisuals();
    }
}
