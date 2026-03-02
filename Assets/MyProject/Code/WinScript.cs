using UnityEngine;

public class WinScript : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject hintOfWin;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered win trigger");
  if (winPanel != null)
                    winPanel.SetActive(true);
            if (hintOfWin != null)
                hintOfWin.SetActive(true);

            ItemCollector collector = other.GetComponent<ItemCollector>();
            if (collector != null && collector.CheckWinStatus())
            {
                if (hintOfWin != null)
                    hintOfWin.SetActive(false);

                if (winPanel != null)
                    winPanel.SetActive(true);

                Time.timeScale = 0f;
            }
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