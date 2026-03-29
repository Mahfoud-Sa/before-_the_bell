using UnityEngine;

public class ToggleToDoPanel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject ToDoPanel;
   // public Volume postProcessVolume; // Assign your Volume here in Inspector

    // Optional: for smooth fade
    public float fadeSpeed = 2f;
    private float targetWeight = 0f;

   

    public void ToggleToDoMenu()
    {
        if (ToDoPanel != null)
        {
            bool isActive = !ToDoPanel.activeSelf;
            ToDoPanel.SetActive(isActive);

            // Toggle Volume based on settings menu state
           
        }
        else
        {
            Debug.LogWarning("ToDoPanel is not assigned in the Inspector!");
        }
    }
}


