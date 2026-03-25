using UnityEngine;

public class InfoTrigger : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject infoPanel;

    [Header("Settings")]
    public bool showOnlyOnce = true;

    private bool hasShown = false;

    private void Start()
    {
        if (infoPanel != null)
            infoPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (showOnlyOnce && hasShown) return;

        if (infoPanel != null)
            infoPanel.SetActive(true);

        hasShown = true;
    }

    public void ClosePanel()
    {
        if (infoPanel != null)
            infoPanel.SetActive(false);
    }
}