using UnityEngine;

public class ToggleLevelsMenuScript : MonoBehaviour
{
        public GameObject levelMenuPanel;
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
            Debug.LogWarning("levelMenuPanel is not assigned in the Inspector!");
        }
    }
}
