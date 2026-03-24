using UnityEngine;

public class WinScript : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject hintOfWin;

    void OnTriggerEnter(Collider other)
{
    Debug.Log("OnTriggerEnter called with: " + other.name);

    if (other.CompareTag("Player"))
    {
       
       
        winPanel.SetActive(true); // Show the win panel
         GameManager.Instance.WinGame();
        // Step 2: Show Win Panel
        // WinPanelUI winPanel = 
       // GameManager.Instance.WinGame(); // reference assigned in Inspector
        // if (winPanel != null)
        //     winPanel.ShowPanel();
    }
}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && hintOfWin != null)
        {
            hintOfWin.SetActive(false);
        }
    }
}