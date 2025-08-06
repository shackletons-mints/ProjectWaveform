using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SpawnVisuals: MonoBehaviour
{
	public static SpawnVisuals Instance { get; private set; }

	[Header("References")]
    public GameObject prefab;
    public float distanceFromCamera = 1f;

	public GameObject visuals;

	void Awake()
	{
		if (Instance != null && Instance != this)
			Destroy(this);
		else
			Instance = this;

		Transform cam = Camera.main.transform;
		Vector3 targetPosition = new Vector3(0f, 1.5f, 2f);
		Quaternion targetRotation = Quaternion.LookRotation(cam.forward, cam.up);

		if (visuals == null)
			visuals = Instantiate(prefab, targetPosition, targetRotation);
			
		Rigidbody rb = visuals.GetComponentInChildren<Rigidbody>();
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
		StartCoroutine(GrowOverTime(visuals));
	}

	public void DropVisuals()
	{
        Rigidbody rb = visuals.GetComponentInChildren<Rigidbody>();
		if (rb != null)
		{
			rb.isKinematic = false;
			rb.useGravity = true;
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
