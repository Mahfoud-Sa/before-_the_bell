using UnityEngine;

public class ShowConfirmScript : MonoBehaviour
{
       public GameObject ConfirmMenuPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
public void ToggleConfirmMenu()

    {
        if (ConfirmMenuPanel != null)
        {
            // Switch between active and inactive
            ConfirmMenuPanel.SetActive(!ConfirmMenuPanel.activeSelf);
        }
        else
        {
            Debug.LogWarning("ConfirmMenuPanel  is not assigned in the Inspector!");
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
