using UnityEngine;

public class ExitInfoPanelButton : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public GameObject InfoMenuPanel;

   public void ToggleInfoMenu()
    {
        if (InfoMenuPanel != null)
        {
            // Switch between active and inactive
            InfoMenuPanel.SetActive(!InfoMenuPanel.activeSelf);
            Time.timeScale = 1f;
        }
        else
        {
            Debug.LogWarning("InfoMenuPanel  is not assigned in the Inspector!");
        }
    }
    void Awake()
    {
        Time.timeScale = 0f;
    }
    void Start()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

