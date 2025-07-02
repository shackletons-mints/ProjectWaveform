using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class VisualDistanceNotifier : MonoBehaviour
{
    [Header("References")]
    public GameObject visuals;
    public Camera xrCamera;
    public GameObject leftHand;
    public GameObject tooltip;
	public Renderer buttonARenderer;
	public Material originalMaterial;

    [Header("Settings")]
    public float triggerDistance = 6f;

    private bool _isTooltipShown = false;

    void Update()
    {
        if (visuals == null || xrCamera == null || leftHand == null) return;

        float distance = Vector3.Distance(visuals.transform.position, xrCamera.transform.position);

        if (distance > triggerDistance && !_isTooltipShown)
        {
            ShowTooltipAndHighlight();
        }
        else if (distance <= triggerDistance && _isTooltipShown)
        {
            HideTooltipAndHighlight();
        }
    }

    void ShowTooltipAndHighlight()
    {
        _isTooltipShown = true;
        if (tooltip != null)
        {
			tooltip.SetActive(true);
			Canvas.ForceUpdateCanvases();

			tooltip.transform.position = leftHand.transform.position + leftHand.transform.up * 0.3f;
			tooltip.transform.rotation = Quaternion.LookRotation(tooltip.transform.position - xrCamera.transform.position);
        }

		if (buttonARenderer != null)
		{
			buttonARenderer.material.color = Color.cyan;
		}
    }

    void HideTooltipAndHighlight()
    {
        _isTooltipShown = false;
        if (tooltip != null)
        {
            tooltip.SetActive(false);
        }

		if (buttonARenderer != null)
		{
			buttonARenderer.material= originalMaterial;
		}
    }
}

