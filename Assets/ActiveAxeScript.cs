using UnityEngine;

public class ActiveAxeScript : MonoBehaviour
{
     
    [Header("UI")]
    [SerializeField] private GameObject AxeButtonUI;

    private bool canUse = true;
    private bool playerInside = false;

    private void Start()
    {
        if (AxeButtonUI != null)
            AxeButtonUI.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            if (AxeButtonUI != null)
                AxeButtonUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (AxeButtonUI != null)
                AxeButtonUI.SetActive(false);
        }
    }
}
