using UnityEngine;
using UnityEngine.UI;

public class AxeActionTriggerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject actionButton;   // The whole button
    public Image actionImage;         // The icon inside the button
    public Sprite axeIcon;            // Axe sprite

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            actionImage.sprite = axeIcon;
            actionButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            actionButton.SetActive(false);
        }
    }
}
