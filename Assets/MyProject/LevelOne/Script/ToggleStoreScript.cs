using UnityEngine;

public class ToggleStoreScript : MonoBehaviour
{
     public GameObject StoreMenuPanel;
   // public Volume postProcessVolume; // Assign your Volume here in Inspector

    // Optional: for smooth fade
    public float fadeSpeed = 2f;
    private float targetWeight = 0f;

   

    public void ToggleStoreMenu()
    {
        if (StoreMenuPanel != null)
        {
            bool isActive = !StoreMenuPanel.activeSelf;
            StoreMenuPanel.SetActive(isActive);

            // Toggle Volume based on settings menu state
           
        }
        else
        {
            Debug.LogWarning("StoreMenuPanel is not assigned in the Inspector!");
        }
    }
}
