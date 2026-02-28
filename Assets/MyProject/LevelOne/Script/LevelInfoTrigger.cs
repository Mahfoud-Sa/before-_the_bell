using UnityEngine;

public class LevelInfoTrigger : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel;
    private bool hasTriggered = false;

    private void Start()
    {
        // Make sure panel is hidden when level starts
        if (infoPanel != null)
            infoPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            infoPanel.SetActive(true);
        }
    }
}