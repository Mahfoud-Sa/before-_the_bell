using UnityEngine;

public class WinScript : MonoBehaviour
{
    public GameObject objectToActivate;
    public GameObject player;
    public GameObject hintOfWin;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Player has collided with the trigger, activate the specified game object
            if (objectToActivate != null)
            {
                hintOfWin.SetActive(true);
                if (player.GetComponent<ItemCollector>().CheckWinStatus()) {
                    hintOfWin.SetActive(false);
                    objectToActivate.SetActive(true);
                    Time.timeScale = 0;
                }
                
               
            }
            
            // You can add more logic or actions here if needed

            // Disable the trigger itself if you want it to activate only once
            //gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hintOfWin.SetActive(false);
        }
    }
}
