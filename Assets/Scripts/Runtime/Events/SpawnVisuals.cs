using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SpawnVisuals: MonoBehaviour
{
	public static SpawnVisuals Instance { get; private set; }

	[Header("References")]
    public GameObject prefab;
	public GameObject visuals;
    public float distanceFromCamera = 1f;

	void Awake()
	{
		if (Instance != null && Instance != this)
			Destroy(this);
		else
			Instance = this;

        Transform cam = Camera.main.transform;
        Vector3 targetPosition = new Vector3(0f,0.25f,2f);
        Quaternion targetRotation = Quaternion.LookRotation(cam.forward, cam.up);

		if (visuals == null)
			visuals = Instantiate(prefab, targetPosition, targetRotation);
	}

	IEnumerator Start()
	{
		GrowVisuals();
		yield return new WaitForSeconds(0.6f);
		DropVisuals();
		yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(2);
	}

	public void GrowVisuals()
	{
		StartCoroutine(GrowOverTime(visuals));
	}

	public void DropVisuals()
	{
        Rigidbody rb = visuals.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.isKinematic = false;
			rb.useGravity = true;
		}
	}

	private IEnumerator GrowOverTime(GameObject obj)
	{
		Vector3 targetScale = Vector3.one;
		float value = 0.001f;
		while (obj.transform.localScale.x < targetScale.x)
		{
			obj.transform.localScale += new Vector3(value, value, value);
			yield return null;
		}

		obj.transform.localScale = targetScale;
	}
}
