using UnityEngine;

public class ActivateOnCollision : MonoBehaviour
{
    public GameObject objectToActivate;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Player has collided with the trigger, activate the specified game object
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
                Time.timeScale = 0;
            }

            // You can add more logic or actions here if needed

            // Disable the trigger itself if you want it to activate only once
            gameObject.SetActive(false);
        }
    }
}
