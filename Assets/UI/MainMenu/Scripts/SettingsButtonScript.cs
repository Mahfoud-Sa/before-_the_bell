using UnityEngine;
using UnityEngine.Rendering;

public class SettingsButtonScript : MonoBehaviour
{
    public GameObject settingsMenuPanel;
    public Volume postProcessVolume;

    public float fadeSpeed = 2f;
    private float targetWeight = 0f;

    void Update()
    {
        // if (postProcessVolume != null)
        // {
        //     // IMPORTANT: use unscaled time (works even when paused)
        //     postProcessVolume.weight = Mathf.MoveTowards(
        //         postProcessVolume.weight,
        //         targetWeight,
        //         fadeSpeed * Time.unscaledDeltaTime
        //     );
        // }
    }

    public void ToggleSettingsMenu()
    {
        if (settingsMenuPanel != null)
        {
            bool isActive = !settingsMenuPanel.activeSelf;
            settingsMenuPanel.SetActive(isActive);

            // Smooth fade
            if (postProcessVolume != null)
            {
                targetWeight = isActive ? 1f : 0f;
            }

            // 🔥 IMPORTANT: control time here too (just in case)
          //  Time.timeScale = isActive ? 0f : 1f;
        }
        else
        {
            Debug.LogWarning("SettingsMenuPanel is not assigned!");
        }
    }
}