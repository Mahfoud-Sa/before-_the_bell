using UnityEngine;
using UnityEngine.UI;

public class TogglePanelButton : MonoBehaviour
{
    [Header("Target Panel")]
    public GameObject targetPanel;

    [Header("Button Image (Optional)")]
    public Image buttonImage;

    public void TogglePanel()
    {
        if (targetPanel == null)
        {
            Debug.LogWarning("Target Panel not assigned!");
            return;
        }

        targetPanel.SetActive(!targetPanel.activeSelf);
    }
}