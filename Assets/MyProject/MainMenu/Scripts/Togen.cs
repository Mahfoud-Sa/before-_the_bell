using UnityEngine;

public class Togen : MonoBehaviour
{
        public GameObject levelMenuPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
     public void ToggleLevelsMenu()
    {
        if (levelMenuPanel != null)
        {
            bool isActive = !levelMenuPanel.activeSelf;
            levelMenuPanel.SetActive(isActive);

            // Toggle Volume based on settings menu state
          
        }
        else
        {
            Debug.LogWarning("SettingsMenuPanel is not assigned in the Inspector!");
        }
    }
}
