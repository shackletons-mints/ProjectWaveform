using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SpawnVisuals: MonoBehaviour
{
	public static GameObject Instance;

	[Header("References")]
    public GameObject prefab;
	public GameObject visuals;
    public float distanceFromCamera = 1f;

	void Awake()
	{
        Transform cam = Camera.main.transform;
        Vector3 targetPosition = new Vector3(0f,1f,2f);
        Quaternion targetRotation = Quaternion.LookRotation(cam.forward, cam.up);

		if (visuals == null)
		{
			visuals = Instantiate(prefab, targetPosition, targetRotation);

			Instance = visuals;
		}
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
		StartCoroutine(GrowOverTime(visuals, 1f));
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

	private IEnumerator GrowOverTime(GameObject obj, float duration)
	{
		Vector3 initialScale = obj.transform.localScale;
		Vector3 targetScale = Vector3.one;
		float elapsed = 0;

		while (elapsed < duration)
		{
			obj.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / duration);
			elapsed += Time.deltaTime;
			yield return null;
		}

		obj.transform.localScale = targetScale;
	}
}
