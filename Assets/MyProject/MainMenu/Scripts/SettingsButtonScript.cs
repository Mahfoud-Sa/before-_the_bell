using UnityEngine;
using UnityEngine.Rendering;

public class SettingsButtonScript : MonoBehaviour
{
    public GameObject settingsMenuPanel;
    public Volume postProcessVolume; // Assign your Volume here in Inspector

    // Optional: for smooth fade
    public float fadeSpeed = 2f;
    private float targetWeight = 0f;

    void Update()
    {
        // Smooth fade for Volume (optional)
        if (postProcessVolume != null)
        {
            postProcessVolume.weight = Mathf.MoveTowards(
                postProcessVolume.weight,
                targetWeight,
                fadeSpeed * Time.deltaTime
            );
        }
    }

    public void ToggleSettingsMenu()
    {
        if (settingsMenuPanel != null)
        {
            bool isActive = !settingsMenuPanel.activeSelf;
            settingsMenuPanel.SetActive(isActive);

            // Toggle Volume based on settings menu state
            if (postProcessVolume != null)
            {
                // Instant toggle:
                // postProcessVolume.enabled = isActive;

                // Smooth fade toggle:
                targetWeight = isActive ? 1f : 0f;
            }
        }
        else
        {
            Debug.LogWarning("SettingsMenuPanel is not assigned in the Inspector!");
        }
    }
}